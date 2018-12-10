Public Class JPAOnlineQuoteTemplateV2
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
    Private _currencySign As String = "", _QuoteId As String = "", _IsInternalUserMode As Boolean = True, _IsBTosQuotation As Boolean = False
    'Sold to
    Private _lbSoldtoCompany As String = "", _lbSoldtoAddr As String, _lbSoldtoAddr2 As String, _lbSoldtoTel As String, _lbSoldtoMobile As String = "", _lbSoldtoAttention As String
    'Ship to
    Private _lbShiptoCompany As String = "", _lbShiptoAddr As String, _lbShiptoAddr2 As String, _lbShiptoTel As String, _lbShiptoMobile As String = "", _lbShiptoAttention As String, _lbShiptoCO As String = ""
    'Bill to
    Private _lbBilltoCompany As String = "", _lbBilltoAddr As String, _lbBilltoAddr2 As String, _lbBilltoTel As String, _lbBilltoMobile As String = "", _lbBilltoAttention As String
    Private _SalesPerson As String, _lbExternalNote As String = "", _IsLumpSumOnly As Boolean = False, _RunTimeURL As String = Util.GetRuntimeSiteUrl
    Private _TimeSpan As TimeSpan
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
    'Public Class QuoteItem
    '    Public Property partno As String
    '    Public Property description As String
    '    Public Property lineNo As Integer
    '    Public Property qty As Integer
    '    Public Property unitPrice As Decimal
    '    Public Property VATinc As Decimal
    '    Public Property EwFlag As Integer
    '    Public Property EwFlagPrice As Decimal
    '    Public Property deliveryPlant As String
    'End Class
    Dim aptQD As New EQDSTableAdapters.QuotationDetailTableAdapter
    Dim _tax As Decimal = CDec(0.05)
    Dim dtM As IBUS.iDocHeaderLine, dtDetail As IBUS.iCartList
    Protected Sub RepeaterDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RepeaterDetail.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            'dtM = Pivot.NewObjDocHeader.GetByDocID(_QuoteId)
            Dim currentcartline As IBUS.iCartLine = CType(e.Item.DataItem, IBUS.iCartLine)
            Dim btospatno As HtmlInputText = CType(e.Item.FindControl("btospatno"), HtmlInputText)
            If btospatno IsNot Nothing AndAlso dtM.ISVIRPARTONLY = 1 Then
                If currentcartline.VirtualPartNo.Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(currentcartline.VirtualPartNo.Value) Then
                    btospatno.Value = currentcartline.VirtualPartNo.Value
                End If
            End If
            Dim EWAmount As HtmlInputText = CType(e.Item.FindControl("EWAmount"), HtmlInputText)
            Dim EwFlagPrice As Decimal = currentcartline.Qty.Value * currentcartline.newunitPrice.Value * Business.getRateByEWItem(Business.getEWItemByMonth(currentcartline.ewFlag.Value, dtM.DocReg), dtM.DocReg) * (1 + _tax)
            EWAmount.Value = "Extended Warranty for " + currentcartline.ewFlag.Value.ToString + " Months: ¥ " + FormatNumber(EwFlagPrice, 0, , , TriState.True)
            Dim posttax As HtmlInputText = CType(e.Item.FindControl("posttax"), HtmlInputText)
            Dim newunitPrice As HtmlInputText = CType(e.Item.FindControl("newunitPrice"), HtmlInputText)
            posttax.Value = "¥ " + FormatNumber(currentcartline.newunitPrice.Value * currentcartline.Qty.Value * (1 + _tax) + EwFlagPrice, 0, , , TriState.True)
            newunitPrice.Value = "¥ " + FormatNumber(currentcartline.newunitPrice.Value, 0, , TriState.True)
            If currentcartline.itemType.Value = COMM.Fixer.eItemType.Parent Then
                Dim gvdetail As GridView = CType(e.Item.FindControl("gv1"), GridView)
                Dim newCart As IBUS.iCartList = dtDetail.Merge2NewCartList(dtDetail.Where(Function(x) x.parentLineNo.Value = currentcartline.lineNo.Value))
                newunitPrice.Value = "¥ " + FormatNumber(newCart.getTotalAmount(), 0, , , TriState.True)
                Dim Ew As IBUS.iCartLine = newCart.Where(Function(p) p.partNo.Value.ToString.StartsWith(COMM.Fixer.AGSEWPrefix, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault()
                If Ew IsNot Nothing Then
                    EwFlagPrice = FormatNumber(Ew.newunitPrice.Value, 0, , , TriState.True) * (1 + _tax)
                Else
                    EwFlagPrice = 0
                End If
                EWAmount.Value = "Extended Warranty for " + currentcartline.ewFlag.Value.ToString + " Months: ¥ " + FormatNumber(EwFlagPrice, 0, , , TriState.True)
                posttax.Value = "¥ " + FormatNumber(newCart.getTotalAmount() * (1 + _tax) + EwFlagPrice, 0, , , TriState.True)
                gvdetail.DataSource = newCart
                gvdetail.DataBind()
                Dim btosdetail As HtmlTableRow = CType(e.Item.FindControl("btosdetail"), HtmlTableRow)
                btosdetail.Visible = True
            End If

        End If
    End Sub
    Public Sub LoadData()
        dtM = Pivot.NewObjDocHeader.GetByDocID(_QuoteId, COMM.Fixer.eDocType.EQ)
        dtDetail = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, _QuoteId, dtM.org).GetListAll(COMM.Fixer.eDocType.EQ)
        If dtDetail.Where(Function(x) x.itemType.Value = COMM.Fixer.eItemType.Parent).Count > 0 Then Me._IsBTosQuotation = True
        If Not IsNothing(dtM) Then
            _currencySign = Util.getCurrSign(dtM.currency) : _IsLumpSumOnly = dtM.isLumpSumOnly
            _tax = dtM.tax
            If _tax < 0 Then _tax = 0.05
            FillQuoteInfo(dtM, dtDetail) 
        End If
        Dim result = dtDetail.Where(Function(p) Not p.partNo.Value.ToString.StartsWith(COMM.Fixer.AGSEWPrefix, StringComparison.CurrentCultureIgnoreCase) And (p.lineNo.Value < 100 Or p.itemType.Value = COMM.Fixer.eItemType.Parent))
        RepeaterDetail.DataSource = result : RepeaterDetail.DataBind()
        'For Each r As IBUS.iCartLine In dtDetail
        '    Dim a As IBUS.iCartLine = r

        '    If r.itemType.Value = COMM.Fixer.eItemType.Parent Then
        '        Dim newCart As IBUS.iCartList = dtDetail.Merge2NewCartList(dtDetail.Where(Function(x) x.parentLineNo.Value = a.parentLineNo.Value))
        '        newCart.getTotalAmount()
        '    End If
        'Next


        'Nada revised to convenient maintenance (removed dbac part of quoteDetail)...........................................................................................
        'Dim result As List(Of QuoteItem) = (From D In MyDC.QuotationDetails Where D.quoteId = QuoteId Order By D.line_No Ascending
        '                                     Select New QuoteItem With {
        '                                        .partno = D.partNo,
        '                                        .description = D.description,
        '                                        .lineNo = D.line_No,
        '                                        .qty = D.qty,
        '                                        .unitPrice = D.newUnitPrice,
        '                                        .VATinc = D.newUnitPrice * D.qty * (1 + _tax),
        '                                        .EwFlag = D.ewFlag,
        '                                        .EwFlagPrice = 0,
        '                                        .deliveryPlant = D.deliveryPlant
        '                                          }).ToList()
        'Dim result As New List(Of QuoteItem)
        'For Each r As IBUS.iCartLine In dtDetail
        '    Dim ri As New QuoteItem
        '    ri.partno = r.partNo.Value
        '    ri.description = r.partDesc.Value
        '    ri.lineNo = r.lineNo.Value
        '    ri.qty = r.Qty.Value
        '    ri.unitPrice = r.newunitPrice.Value
        '    ri.VATinc = r.newunitPrice.Value * r.Qty.Value * (1 + _tax)
        '    ri.EwFlag = r.ewFlag.Value
        '    ri.EwFlagPrice = 0
        '    ri.deliveryPlant = r.divPlant.Value
        '    result.Add(ri)
        'Next
        '/Nada revised to convenient maintenance (removed dbac part of quoteDetail)...........................................................................................

        'If Not _IsBTosQuotation Then
        '    For Each i As QuoteItem In result
        '        If i.EwFlag > 0 Then
        '            i.EwFlagPrice = i.qty * i.unitPrice * Business.getRateByEWItem(Business.getEWItemByMonth(i.EwFlag, dtM.DocReg), dtM.DocReg) * (1 + _tax)
        '            i.VATinc += i.EwFlagPrice
        '        End If
        '    Next
        'End If
        ''If result.Count = 0 Then Exit Sub
        'If _IsBTosQuotation Then
        '    'Frank 2013/04/02
        '    'Dim totalprice As Decimal = result.Sum(Function(p) p.unitPrice * p.qty)
        '    Dim totalprice As Decimal = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, QuoteId, Pivot.CurrentProfile.getCurrOrg).GetListAll.getTotalAmount
        '    Dim Btos100 As QuoteItem = result.FirstOrDefault()
        '    Dim QM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteId)
        '    If QM IsNot Nothing AndAlso Not IsDBNull(QM.customId) AndAlso Not String.IsNullOrEmpty(QM.customId) Then
        '        Btos100.description = QM.customId.ToString.Trim
        '    End If
        '    Dim myQD As New quotationDetail("EQ", "quotationDetail")
        '    If result.Where(Function(p) p.partno.StartsWith(COMM.Fixer.AGSEWPrefix, StringComparison.CurrentCultureIgnoreCase)).Count > 0 Then
        '        Btos100.EwFlagPrice = result.Where(Function(p) p.partno.StartsWith(COMM.Fixer.AGSEWPrefix, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault.unitPrice * (1 + _tax)
        '    End If
        '    Btos100.unitPrice = totalprice
        '    Btos100.VATinc = totalprice * (1 + _tax)
        '    result.Clear()
        '    result.Add(Btos100)
        '    RepeaterDetail.DataSource = result
        '    RepeaterDetail.DataBind()
        '    'trrd2.Visible = False
        'Else

        '    'If result.Count > 5 Then
        '    ' RepeaterDetail.DataSource = result.Take(5) : RepeaterDetail.DataBind()
        '    ' RepeaterDetail2.DataSource = result.Skip(5) : RepeaterDetail2.DataBind()
        '    ' Else
        '    'Dim 
        '    'For Each i In result
        '    '    If Not i.partno.ToString.StartsWith(COMM.Fixer.AGSEWPrefix, StringComparison.CurrentCultureIgnoreCase) Then
        '    '    End If
        '    'Next
        '    ' Dim AGSEWitems As List(Of QuoteItem) = result.Where(Function(p) p.partno.StartsWith(COMM.Fixer.AGSEWPrefix, StringComparison.CurrentCultureIgnoreCase)).ToList()
        '    result.RemoveAll(Function(p) p.partno.StartsWith(COMM.Fixer.AGSEWPrefix, StringComparison.CurrentCultureIgnoreCase))
        '    RepeaterDetail.DataSource = result : RepeaterDetail.DataBind()
        '    ' trrd2.Visible = False
        '    ' End If
        'End If
    End Sub
    Protected Sub FillQuoteInfo(ByRef QuoteMasterRow As IBUS.iDocHeaderLine, ByRef QuoteDetailTb As IBUS.iCartList)
        'Dim dt As EQDS.QuotationMasterDataTable = aptQM.GetQuoteMasterById(_QuoteId)
        'Dim decSubTotal As Decimal = aptQD.getTotalAmount(_QuoteId) + Business.getTotalAmount_EW(_QuoteId)
        Dim decSubTotal As Decimal = QuoteDetailTb.getTotalAmount

        If QuoteDetailTb.Count > 0 Then
            With QuoteMasterRow

                'Me.LabelQuoteID.Text = _QuoteId
                Me.LabelQuoteID.Text = .quoteNo
                Me.quoteDate.Text = Util.GetLocalTime("AJP", .DocDate).ToString("MM/dd/yyyy")
                'Getting sold to, ship to and bill to data from [eQuotation].[dbo].[EQPARTNER]
                Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
                Dim SoldToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "SOLDTO")

                'Ryan 20161116 Add for notes field
                LitNotes.Text = .quoteNote

                'Dim ShipToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "S")
                'Dim BillToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "B")
                If SoldToTable.Count > 0 Then
                    Dim SoldToRow As EQDS.EQPARTNERRow = SoldToTable(0)
                    Me.SoldToCompany.Text = SoldToRow.NAME
                    'If Not String.IsNullOrEmpty(SoldToRow.ERPID) Then lblSoldtoERPID.Text = "<span style='background-color:#EFF580; font-weight:bold'>&nbsp;" + SoldToRow.ERPID + "&nbsp;</span>"
                    'Me._lbSoldtoAddr = SoldToRow.STREET + " " + SoldToRow.CITY + " " + SoldToRow.STATE + " " + SoldToRow.COUNTRY + " " + SoldToRow.ZIPCODE
                    'Me._lbSoldtoAddr2 = SoldToRow.STREET2 : 
                    Me.CustomerPhone.Text = SoldToRow.TEL
                    'Me._lbSoldtoMobile = SoldToRow.MOBILE : 
                    If Not String.IsNullOrEmpty(SoldToRow.ATTENTION.ToString.Trim) Then
                        Me.CustomerContact.Text = SoldToRow.ATTENTION + " 樣"
                    End If
                End If

                'Ryan 20161221 CustomerOffice.Text data source will be Quotation Extension table, comment out existing code 
                'CustomerOffice.Text = Pivot.NewObjPatch.GetCustomerOffice("CRM", String.Format("select TOP 1 isnull(LOC,'') as  CustomerOffice  from S_ORG_EXT where ROW_ID='{0}'", QuoteMasterRow.AccRowId))
                Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
                If Not _ME Is Nothing Then
                    CustomerOffice.Value = _ME.JPCustomerOffice
                End If


                'Frank 2012/09/06 Get Sales Representative by EmployeeID of EQPartner or quotation creator
                Dim Employee1 As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "E")
                If Employee1.Count > 0 Then
                    Dim Employee1Row As EQDS.EQPARTNERRow = Employee1(0)
                    'Me.SaleName.Text = "Sales Representative: " & Relics.SAPDAL.GetSalesRepresentativeByEmployeeID(Employee1Row.ERPID, .createdBy)
                    Me.SaleName.Text = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Employee1Row.ERPID, .CreatedBy)
                End If
                Me.expiredDate.Text = Util.GetLocalTime("AJP", .expiredDate).ToString("MM/dd/yyyy")
                Me.PaymentTerm.Text = SAPTools.GetPaymentMethodNameByValue(.paymentTerm)
                If Me.PaymentTerm.Text = "0" Then Me.PaymentTerm.Text = "TBD"
                Me.Creator.Text = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(String.Empty, .CreatedBy)
            End With
        End If
    End Sub

    Protected Sub initGV(ByRef QuoteDetailTb As IBUS.iCartList)

        'Frank:Refresh product inventory information
        Business.RefreshQuotationInventory(_QuoteId)

        Dim myQD As New QuotationDetail("EQ", "quotationDetail")
        Dim DT As DataTable = myQD.GetDT(String.Format("quoteId='{0}'", _QuoteId), "line_No")


        'If DT.Rows.Count > 17 Then
        '    Me.gv1.DataSource = DtSelectTop(17, DT) : Me.gv1.DataBind()
        '    Me.gv2.DataSource = DtSelectBottom(17, DT) : Me.gv2.DataBind()
        'Else
        '    Me.gv1.DataSource = DT : Me.gv1.DataBind()
        '    trGV2.Visible = False
        'End If

    End Sub

    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim currentcartline As IBUS.iCartLine = CType(e.Row.DataItem, IBUS.iCartLine)
            Dim btospatno As HtmlInputText = CType(e.Row.FindControl("btospatno"), HtmlInputText)
            If btospatno IsNot Nothing AndAlso dtM.ISVIRPARTONLY = 1 Then
                If currentcartline.VirtualPartNo.Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(currentcartline.VirtualPartNo.Value) Then
                    btospatno.Value = currentcartline.VirtualPartNo.Value
                End If
            End If
        End If
        '    Dim myQM As New quotationMaster("EQ", "quotationMaster"), myQD As New quotationDetail("EQ", "quotationDetail")
        '    Dim dt As DataTable = myQM.GetDT(String.Format("quoteId='{0}'", _QuoteId), "")

        '    Dim DBITEM As DataRowView = CType(e.Row.DataItem, DataRowView)
        '    Dim line_no As Integer = gv1.DataKeys(e.Row.RowIndex).Value
        '    'Dim part_no As String = e.Row.Cells(1).Text.Trim
        '    Dim part_no As String = DBITEM.Item("partNo").ToString
        '    Dim ListPice As Decimal = CDbl(CType(e.Row.FindControl("lbListPrice"), Label).Text)
        '    Dim UnitPrice As Decimal = CDbl(CType(e.Row.FindControl("lbUnitPrice"), Label).Text)
        '    Dim qty As Decimal = CInt(CType(e.Row.FindControl("lbGVQty"), Label).Text)
        '    Dim itp As Decimal = DBITEM.Item("newITP").ToString
        '    Dim Discount As Decimal = 0.0, SubTotal As Decimal = 0.0, ewPrice As Decimal = 0.0

        '    If DBITEM.Item("ewflag") = 99 Then
        '        CType(e.Row.FindControl("lbew"), Label).Text = "36"
        '    End If
        '    If DBITEM.Item("ewflag") = 999 Then
        '        CType(e.Row.FindControl("lbew"), Label).Text = "3"
        '    End If

        '    ewPrice = FormatNumber(Business.getRateByEWItem(Business.getEWItemByMonth(CInt(CType(e.Row.FindControl("lbew"), Label).Text)), DBITEM.Item("deliveryPlant")) * UnitPrice, 2)

        '    CType(e.Row.FindControl("gv_lbEW"), Label).Text = ewPrice

        '    If ListPice = 0 Then
        '        CType(e.Row.FindControl("lbDisc"), Label).Text = "TBD"
        '    Else
        '        Discount = FormatNumber((ListPice - UnitPrice) / ListPice, 2)
        '        If ListPice < UnitPrice Then
        '            Discount = 0
        '        End If
        '        CType(e.Row.FindControl("lbDisc"), Label).Text = Discount * 100 & "%"
        '    End If
        '    SubTotal = FormatNumber(qty * UnitPrice, 2)
        '    CType(e.Row.FindControl("lbSubTotal"), Label).Text = SubTotal

        '    If myQD.isBtoParentItem(_QuoteId, line_no) = 1 Then
        '        CType(e.Row.FindControl("lbDisc"), Label).Text = ""
        '        Dim totalamont = myQD.getTotalAmount(_QuoteId) + myQD.getTotalAmount_EW(_QuoteId)
        '        CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(totalamont, 2)
        '        CType(e.Row.FindControl("lbUnitPrice"), Label).Text = FormatNumber(totalamont / DBITEM.Item("qty"), 2)
        '    End If

        '    'If myQD.isBtoChildItem(_QuoteId, line_no) = 1 Or DBITEM.Item("partNo").ToString.StartsWith("AGS-EW") Then
        '    'If dt.Rows(0).Item("isLumpSumOnly") = 1 And (_IsInternalUserMode = False) Then
        '    '    CType(e.Row.FindControl("lbUnitPriceSign"), Label).Visible = False
        '    '    CType(e.Row.FindControl("lbUnitPrice"), Label).Visible = False
        '    '    CType(e.Row.FindControl("lbSubTotalSign"), Label).Visible = False
        '    '    CType(e.Row.FindControl("lbSubTotal"), Label).Visible = False
        '    'End If
        '    'End If

        '    Dim QMDT As New DataTable
        '    Dim ORG As String = dt.Rows(0).Item("org")
        '    Dim strStatusCode As String = "", strStatusDesc As String = ""
        '    'If myQD.isBtoParentItem(_QuoteId, line_no) = 0 And (Not DBITEM.Item("partNo").ToString.ToLower.StartsWith("ags-")) And Business.isInvalidPhaseOutV2(part_no, ORG, strStatusCode, strStatusDesc, 0) Then
        '    If myQD.isBtoParentItem(_QuoteId, line_no) = 0 AndAlso part_no.ToLower.StartsWith("ags-") = False AndAlso Business.isInvalidPhaseOutV2(part_no, ORG, strStatusCode, strStatusDesc, 0) Then
        '        CType(e.Row.FindControl("lbdescription"), Label).Text = "(Phase Out)"
        '        CType(e.Row.FindControl("lbdescription"), Label).ForeColor = Drawing.Color.Red
        '    End If

        '    'Frank 2012/08/01:If line no is bigger than 100 then to show the category instead of the part_no
        '    If line_no Mod 100 > 0 AndAlso line_no > 100 Then
        '        e.Row.Cells(1).Text = DBITEM.Item("category").ToString
        '        'ElseIf line_no < 100 Then
        '        'e.Row.Cells(0).Text = line_no * 100
        '    End If

        '    'Frank If due date is 12/31/9999 then replace it by TBD
        '    '1.For BTOS:If 100 LINE available date=12/31/9999 then display TBD string.
        '    '  For BTOS:If COMPONENT stock is enough then display original available date(due date), else display TBD string
        '    '2.For not BTOS:If COMPONENT available date=12/31/9999 then display TBD string.
        '    If CType(e.Row.FindControl("lbDueDate"), Label).Text.Equals("12/31/9999") Then CType(e.Row.FindControl("lbDueDate"), Label).Text = "TBD"
        '    'If CType(e.Row.FindControl("lbDueDate"), Label).Text.Equals("12/31/9999") Then
        '    '    If Me._IsBTosQuotation Then
        '    '        'If line_no Mod 100 = 0 AndAlso line_no >= 100 Then
        '    '        '    CType(e.Row.FindControl("lbDueDate"), Label).Text = "TBD"
        '    '        'End If
        '    '        CType(e.Row.FindControl("lbDueDate"), Label).Text = "TBD"
        '    '    Else
        '    '        CType(e.Row.FindControl("lbDueDate"), Label).Text = "TBD"
        '    '    End If
        '    'End If

        '    'Frank 2012/07/31:Base on the column "PRINTOUT_FORMAT" to decide the part_no and unit price pisplay logic.
        '    Dim dtM As EQDS.QuotationMasterDataTable = aptQM.GetQuoteMasterById(_QuoteId)
        '    Dim _qmrow As EQDS.QuotationMasterRow = dtM.Rows(0)
        '    Dim _pri_format As EnumSetting.USPrintOutFormat = _qmrow.PRINTOUT_FORMAT
        '    '_pri_format = EnumSetting.USPrintOutFormat.MAIN_ITEM_ONLY
        '    '_pri_format = EnumSetting.USPrintOutFormat.SUB_ITEM_WITH_SUB_ITEM_PRICE
        '    '_pri_format=EnumSetting.USPrintOutFormat.SUB_ITEM_WITHOUT_SUB_ITEM_PRICE
        '    Select Case _pri_format
        '        Case EnumSetting.USPrintOutFormat.MAIN_ITEM_ONLY
        '            If line_no Mod 100 > 0 AndAlso line_no > 100 Then
        '                e.Row.Visible = False
        '            End If
        '        Case EnumSetting.USPrintOutFormat.SUB_ITEM_WITH_SUB_ITEM_PRICE
        '            If line_no Mod 100 > 0 AndAlso line_no > 100 Then
        '                e.Row.Cells(1).Text = DBITEM.Item("partNo").ToString
        '            End If
        '        Case EnumSetting.USPrintOutFormat.SUB_ITEM_WITHOUT_SUB_ITEM_PRICE
        '            If line_no Mod 100 > 0 AndAlso line_no > 100 Then
        '                e.Row.Cells(1).Text = "Other"
        '                e.Row.Cells(8).Text = ""
        '                e.Row.Cells(10).Text = ""
        '            End If
        '        Case EnumSetting.USPrintOutFormat.SUB_ITEM_WITHPARTNO_WITHOUT_SUB_ITEM_PRICE
        '            If line_no Mod 100 > 0 AndAlso line_no > 100 Then
        '                e.Row.Cells(1).Text = DBITEM.Item("partNo").ToString
        '                e.Row.Cells(8).Text = ""
        '                e.Row.Cells(10).Text = ""
        '            End If

        '    End Select

        'End If

    End Sub
    Protected Function SetDisplay(ByVal ewprice As Object) As String
        If IsNumeric(ewprice) AndAlso CInt(ewprice) = 0 Then
            Return "class=""hide"""
        End If
        Return ""
    End Function
    Public Shared Function DtSelectTop(ByVal TopItem As Integer, ByVal oDT As DataTable) As DataTable
        If oDT.Rows.Count < TopItem Then
            Return oDT
        End If
        Dim NewTable As DataTable = oDT.Clone()
        Dim rows As DataRow() = oDT.Select("1=1")
        For i As Integer = 0 To TopItem - 1
            NewTable.ImportRow(DirectCast(rows(i), DataRow))
        Next
        Return NewTable
    End Function
    Public Shared Function DtSelectBottom(ByVal TopItem As Integer, ByVal oDT As DataTable) As DataTable
        If oDT.Rows.Count < TopItem Then
            Return oDT
        End If
        Dim NewTable As DataTable = oDT.Clone()
        Dim rows As DataRow() = oDT.Select("1=1")
        For i As Integer = 0 To oDT.Rows.Count - 1
            If i >= TopItem Then
                NewTable.ImportRow(DirectCast(rows(i), DataRow))
            End If
        Next
        Return NewTable
    End Function

  
End Class