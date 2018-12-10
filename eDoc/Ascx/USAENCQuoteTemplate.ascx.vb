Public Class USAENCQuoteTemplate
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

    Public _QuoteNo As String = String.Empty

    Public _currencySign As String = "", _QuoteId As String = "", _IsInternalUserMode As Boolean = True, _IsBTosQuotation As Boolean = False
    'Sold to
    Public _lbSoldtoCompany As String = "", _lbSoldtoAddr As String, _lbSoldtoAddr2 As String, _lbSoldtoTel As String, _lbSoldtoMobile As String = "", _lbSoldtoAttention As String
    'Ship to
    Public _lbShiptoCompany As String = "", _lbShiptoAddr As String, _lbShiptoAddr2 As String, _lbShiptoTel As String, _lbShiptoMobile As String = "", _lbShiptoAttention As String, _lbShiptoCO As String = ""
    'Bill to
    Public _lbBilltoCompany As String = "", _lbBilltoAddr As String, _lbBilltoAddr2 As String, _lbBilltoTel As String, _lbBilltoMobile As String = "", _lbBilltoAttention As String
    Public _SalesPerson As String, _lbExternalNote As String = "", _IsLumpSumOnly As Boolean = False, _RunTimeURL As String = Util.GetRuntimeSiteUrl
    Public _LeadTime As String = "", _warranty As String = ""
    Public _TimeSpan As TimeSpan
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


    Private _IsV2_0Quot As Boolean = False
    Dim aptQD As New EQDSTableAdapters.QuotationDetailTableAdapter
    Private QuoteHeaderLine As IBUS.iDocHeaderLine = Nothing
    Public Sub LoadData()
        'Dim dtM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteId, COMM.Fixer.eDocType.EQ)
        QuoteHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteId, COMM.Fixer.eDocType.EQ)

        Me._QuoteNo = QuoteHeaderLine.quoteNo
        Dim dtDetail As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, _QuoteId, QuoteHeaderLine.org).GetListAll(COMM.Fixer.eDocType.EQ)


        If Not IsNothing(QuoteHeaderLine) Then

            Dim QM As IBUS.iDocHeader = New DOCH.DocHeader
            Me._IsV2_0Quot = QM.IsV2_0Quotation(_QuoteId)

            _currencySign = Util.getCurrSign(QuoteHeaderLine.currency) : _IsLumpSumOnly = QuoteHeaderLine.isLumpSumOnly
            FillQuoteInfo(QuoteHeaderLine, dtDetail) : initGV(dtDetail)
        End If
    End Sub

    ''' <summary>
    ''' Showing up the office information by SaleOffice
    ''' </summary>
    ''' <param name="_SALESOFFICE"></param>
    ''' <remarks></remarks>
    Public Sub SetOfficeInformation(ByVal _SALESOFFICE As String)
        If _SALESOFFICE Is Nothing Then _SALESOFFICE = ""
        Me.officeInformation_USA.Visible = False : Me.officeInformation_Mexico.Visible = False

        Select Case Business.GetQuotationNumberType(Me._QuoteNo)
            Case EnumSetting.QuotationNumberType.AUSQ
                Me.officeInformation_Mexico.Visible = False
                Me.officeInformation_USA.Visible = True
            Case EnumSetting.QuotationNumberType.AMXQ
                Me.officeInformation_Mexico.Visible = True
                Me.officeInformation_USA.Visible = False
        End Select

    End Sub

    Public Sub setOfficeContactInfoBySales(ByVal SalesEmail As String, ByVal Creator As String)

        Select Case Business.GetQuotationNumberType(_QuoteNo)
            Case EnumSetting.QuotationNumberType.AUSQ
                'Generating USA version template
                Dim strMilpitasAddr As String = "380 Fairview Way, Milpitas, CA 95035 USA", strMilpitasTel As String = "1-888-576-9668"
                Dim strCincinnatiAddr As String = "11380 Reed Hartman Highway Cincinnati, OH 45241, USA", strCincinnatiTel As String = "1-888-576-9668"
                Dim _CincinnatiGroupName As String = "EMPLOYEES.Cincinnati"

                Dim _MailGroupObj As New PROF.MailGroup
                Dim _IsEMPLOYEESCincinnati As Boolean = False
                Dim _dt As DataTable = Nothing

                'Get mail groups of "Prepared for"
                If Not String.IsNullOrEmpty(SalesEmail) Then
                    _dt = _MailGroupObj.GetMailGroupAsDatatable(SalesEmail)
                End If
                If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                    Dim _rows() As DataRow = _dt.Select("Name='" & _CincinnatiGroupName & "'")
                    If (_rows IsNot Nothing AndAlso _rows.Count > 0) Then
                        _IsEMPLOYEESCincinnati = True
                    End If
                Else
                    'Get mail groups of "quote creator"
                    If Not String.IsNullOrEmpty(Creator) Then
                        _dt = _MailGroupObj.GetMailGroupAsDatatable(Creator)
                    End If
                    If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                        Dim _rows() As DataRow = _dt.Select("Name='" & _CincinnatiGroupName & "'")
                        If (_rows IsNot Nothing AndAlso _rows.Count > 0) Then
                            _IsEMPLOYEESCincinnati = True
                        End If
                    End If
                End If

                ''If Pivot.CurrentProfile.GroupBelTo.Contains(("EMPLOYEES.Cincinnati").ToUpper) Then
                'If _IsEMPLOYEESCincinnati Then
                '    litOfficeAddr.Text = strCincinnatiAddr : litOfficeTelTime1.Text = strCincinnatiTel + " 8 am- 8 pm (EST) Mon-Fri"
                '    litOfficeTelTime2.Text = strCincinnatiTel + " 8 am- 8 pm (EST) Mon-Fri" : litOfficeTel3.Text = strCincinnatiTel
                'Else
                '    litOfficeAddr.Text = strMilpitasAddr : litOfficeTelTime1.Text = strMilpitasTel + " 8 am- 8 pm (EST) Mon-Fri"
                '    litOfficeTelTime2.Text = strMilpitasTel + " 8 am- 8 pm (EST) Mon-Fri" : litOfficeTel3.Text = strMilpitasTel
                'End If

            Case EnumSetting.QuotationNumberType.AMXQ
                'Generating Mexico version template
                Dim strMeXicoTel As String = "1-800-467-2415"
                'litOfficeTelTime2.Text = strMeXicoTel & " 8 am- 8 pm (EST) Mon-Fri" : litOfficeTel3.Text = strMeXicoTel

        End Select




    End Sub

    Protected Sub FillQuoteInfo(ByRef QuoteMasterRow As IBUS.iDocHeaderLine, ByRef QuoteDetailTb As IBUS.iCartList)
        'Dim dt As EQDS.QuotationMasterDataTable = aptQM.GetQuoteMasterById(_QuoteId)
        'Dim decSubTotal As Decimal = aptQD.getTotalAmount(_QuoteId) + Business.getTotalAmount_EW(_QuoteId)

        'Dim decSubTotal As Decimal = QuoteDetailTb.getTotalAmount

        If QuoteDetailTb IsNot Nothing AndAlso QuoteDetailTb.Count > 0 Then
            Dim decSubTotal As Decimal = QuoteDetailTb.getTotalAmount

            With QuoteMasterRow
                'Frank 2012/07/16:To show the office information by SaleOffice
                'Me.SetOfficeInformation("2410")
                Me.SetOfficeInformation(.AccGroupCode) : Me.LabelQuoteID.Text = .quoteNo : Me.LabelQuoteRevisionNumber.Text = .Revision_Number
                'Frank 2012/08/09:Change the quoteDate to US local time 
                'Me.quoteDate.Text = Util.TransferToLocalTime(Me._TimeSpan, .quoteDate).ToString("MM/dd/yyyy")
                Me.quoteDate.Text = Util.GetLocalTime("AUS", .DocDate).ToString("MM/dd/yyyy")


                'Getting sold to, ship to and bill to data from [eQuotation].[dbo].[EQPARTNER]
                Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
                Dim SoldToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "SOLDTO")
                Dim ShipToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "S")
                Dim BillToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "B")
                If SoldToTable.Count > 0 Then
                    Dim SoldToRow As EQDS.EQPARTNERRow = SoldToTable(0)
                    Me._lbSoldtoCompany = SoldToRow.NAME
                    If Not String.IsNullOrEmpty(SoldToRow.ERPID) Then lblSoldtoERPID.Text = "<span style='background-color:#EFF580; font-weight:bold'>&nbsp;" + SoldToRow.ERPID + "&nbsp;</span>"
                    Me._lbSoldtoAddr = SoldToRow.STREET + " " + SoldToRow.CITY + " " + SoldToRow.STATE + " " + SoldToRow.COUNTRY + " " + SoldToRow.ZIPCODE
                    Me._lbSoldtoAddr2 = SoldToRow.STREET2 : Me._lbSoldtoTel = SoldToRow.TEL
                    Me._lbSoldtoMobile = SoldToRow.MOBILE : Me._lbSoldtoAttention = SoldToRow.ATTENTION
                End If
                If ShipToTable.Count > 0 Then
                    Dim ShipToRow As EQDS.EQPARTNERRow = ShipToTable(0)
                    Me._lbShiptoCompany = ShipToRow.NAME
                    If Not String.IsNullOrEmpty(ShipToRow.ERPID) Then lblShipToERPID.Text = "<span style='background-color:#EFF580; font-weight:bold'>&nbsp;" + ShipToRow.ERPID + "&nbsp;</span>"
                    Me._lbShiptoAddr = ShipToRow.STREET + " " + ShipToRow.CITY + " " + ShipToRow.STATE + " " + ShipToRow.COUNTRY + " " + ShipToRow.ZIPCODE
                    Me._lbShiptoAddr2 = ShipToRow.STREET2 : Me._lbShiptoTel = ShipToRow.TEL
                    Me._lbShiptoMobile = ShipToRow.MOBILE : Me._lbShiptoAttention = ShipToRow.ATTENTION
                    If .CARE_ON IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(.CARE_ON) Then Me._lbShiptoCO = .CARE_ON
                    'Esther 20160603 remove tax and list TBD
                    'If Not String.IsNullOrEmpty(ShipToRow.STATE) AndAlso ShipToRow.STATE.Equals("TX", StringComparison.OrdinalIgnoreCase) Then
                    '    Me.tax.Text = FormatNumber((Business.GetTaxableAmount(.Key, ShipToRow.ERPID) + .freight) * .tax, 2)
                    'Else
                    '    Me.tax.Text = FormatNumber(Business.GetTaxableAmount(.Key, ShipToRow.ERPID) * .tax, 2)
                    'End If
                End If
                If BillToTable.Count > 0 Then
                    Dim BillToRow As EQDS.EQPARTNERRow = BillToTable(0)
                    Me._lbBilltoCompany = BillToRow.NAME
                    If Not String.IsNullOrEmpty(BillToRow.ERPID) Then lblBillToERPID.Text = "<span style='background-color:#EFF580; font-weight:bold'>&nbsp;" + BillToRow.ERPID + "&nbsp;</span>"
                    Me._lbBilltoAddr = BillToRow.STREET + " " + BillToRow.CITY + " " + BillToRow.STATE + " " + BillToRow.COUNTRY + " " + BillToRow.ZIPCODE
                    Me._lbBilltoAddr2 = BillToRow.STREET2 : Me._lbBilltoTel = BillToRow.TEL
                    Me._lbBilltoMobile = BillToRow.MOBILE : Me._lbBilltoAttention = BillToRow.ATTENTION

                End If
                'Frank 2012/09/06 Get Sales Representative by EmployeeID of EQPartner or quotation creator
                Dim Employee1 As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "E")
                Dim _salescode As String = String.Empty
                If Employee1.Count > 0 Then
                    Dim Employee1Row As EQDS.EQPARTNERRow = Employee1(0) : _salescode = Employee1Row.ERPID
                    'lblSalesPerson.Text = "Sales Representative: " & SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Employee1Row.ERPID, .CreatedBy)
                End If
                Dim salestel As String = String.Empty
                'Matt 20151007:We can if we are going to be using 888-576-9668 and that number is only going to be used for IAG, yes!
                'That number is paid for by the Cincinnati office so that is the one we should be using for IA only
                'If Role.IsAonlineUsaIag Then
                '    'salestel = "1-888-576-9668"
                '    salestel = "Ignore"
                'End If
                lblSalesPerson.Text = "Sales Representative: " & SAPDAL.SAPDAL.GetSalesRepresentativeForUSAonline(_salescode, .CreatedBy, salestel)


                'Frank 2015/10/06 setOfficeContactInfoBySales這個Function內本來就沒有處理.SalesEmail所以我先刪除這個參數
                'If .salesEmail IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(.salesEmail) Then
                '    setOfficeContactInfoBySales(.salesEmail)
                'End If
                setOfficeContactInfoBySales(.salesEmail, .CreatedBy)

                If .PO_NO IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(.PO_NO) Then
                    'lblPO.Text = "PO#: " + .PO_NO
                    lblPO1.Text = .PO_NO
                End If

                'Frank 2012/08/09:Change the expiredDate to US local time
                'Me.expiredDate.Text = Util.TransferToLocalTime(Me._TimeSpan, .expiredDate).ToString("MM/dd/yyyy")
                Me.expiredDate.Text = Util.GetLocalTime("AUS", .expiredDate).ToString("MM/dd/yyyy")

                'ship Term
                'Me.shipTerm.Text = SAPTools.GetShipMethodNameByValue(.shipTerm)
                'If Me.shipTerm.Text = "0" Then Me.shipTerm.Text = "TBD"
                '20120803 TC: Use incoterm for shipTerm instead (per Cathee's request)
                Me.shipTerm.Text = SAPTools.GetIncotermName(.Inco1) + " " + .Inco2
                'payment Term
                Me.paymentTerm.Text = SAPTools.GetPaymentMethodNameByValue(.paymentTerm)
                If Me.paymentTerm.Text = "0" Then Me.paymentTerm.Text = "TBD"

                Me.freight.Text = IIf(IsDBNull(.freight) Or .freight = 0, "TBD", _currencySign + .freight.ToString("n2"))

                'Esther 20160603 remove tax and list TBD
                'Me.lbTaxRate.Text = FormatNumber(.tax * 100, 2) & "%"
                Dim dTax As Decimal = 0
                'If Double.TryParse(Me.tax.Text, dTax) = False OrElse CDbl(Me.tax.Text) = 0 Then
                '    Me.tax.Text = "TBD"
                'End If
                Me.tax.Text = "TBD"

                'Ming 20150529 show Recycling Fee 
                Dim _RecyclingFee As Decimal = QuoteDetailTb.getTotalRecyclingFee
                RecyclingFee.Text = _currencySign + _RecyclingFee.ToString("n2")
                Me.lbSubTotal1.Text = _currencySign + (decSubTotal - _RecyclingFee).ToString("n2")
                Dim t As Decimal = decSubTotal.ToString()
                Me.lbTotal.Text = _currencySign + (decSubTotal + .freight + dTax).ToString("n2")
                'Me.lbTaxRate.Text = "0"
                'If t <> 0 Then

                'Dim _txtTempZipCode As String = SAPTools.getUSZipcodeByQuiteID(.quoteId)
                'If Not String.IsNullOrEmpty(_txtTempZipCode) Then
                '    Me.lbTaxRate.Text = FormatNumber(SAPTools.getSalesTaxByZIP(_txtTempZipCode) * 100, 2) & "%"
                'End If
                'End If
                Dim lt As Decimal = aptQD.getTotalListAmount(_QuoteId)
                '20120719 Rudy: Load external note(Order Note)
                Dim eqNoteAdt As New EQDSTableAdapters.QuotationNoteTableAdapter
                Dim dtEqNote As EQDS.QuotationNoteDataTable = eqNoteAdt.GetNoteTextBYQuoteId(.Key)
                For Each row As EQDS.QuotationNoteRow In dtEqNote.Rows
                    If row.notetype.ToUpper = "ORDERNOTE" AndAlso Not String.IsNullOrEmpty(row.HtmlNoteText) Then
                        _lbExternalNote += row.HtmlNoteText + "<br/>"
                    End If
                Next
                Me.LabelNote.Text = _lbExternalNote

                If shipTerm.Text.ToUpper = "DUE" Then Me.tbSM.Visible = False
                If _IsInternalUserMode = False Then Me.tdBill.Visible = False

                Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(.Key)
                If _ME IsNot Nothing Then
                    'Frank 我暫時把AENC要的LeadTime放在Master Extension表的engineer tel欄位中
                    Me.LabelLeadTime.Text = _ME.Engineer_Telephone
                    Me.LabelWarranty.Text = _ME.Warranty
                    Me.lbInsideSales.Text = _ME.Engineer
                End If

            End With
        End If
    End Sub

    Private _breakpintstrarr() As String

    Protected Sub initGV(ByRef QuoteDetailTb As IBUS.iCartList)

        'Frank:Refresh product inventory information
        Business.RefreshQuotationInventory(_QuoteId)

        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim DT As DataTable = myQD.GetDT(String.Format("quoteId='{0}'", _QuoteId), "line_No")


        '-----Frank 20160523 load discount price by qty scales-------------------
        Dim _pri_format As EnumSetting.USPrintOutFormat = QuoteHeaderLine.PRINTOUT_FORMAT
        If _pri_format = EnumSetting.USPrintOutFormat.SUB_ITEM_WITH_BREAKPOINTS Then
            Dim _dtQuoteDetailMasterBreakPoint As DataTable = New DataTable
            _dtQuoteDetailMasterBreakPoint = tbOPBase.dbGetDataTable("EQ", "Select QuoteID,BreakPoints From QuotationMaster_BreakPoint where quoteid='" & UID & "'")

            lbSubTotalTitle.Visible = False
            lbSubTotal1.Visible = False

            lbTotalTitle.Visible = False
            lbTotal.Visible = False

            Dim _breakpintstr As String = "1,50,100"

            If _dtQuoteDetailMasterBreakPoint IsNot Nothing AndAlso _dtQuoteDetailMasterBreakPoint.Rows.Count > 0 Then

                _breakpintstr = _dtQuoteDetailMasterBreakPoint.Rows(0).Item("BreakPoints").ToString

            Else

            End If

            _breakpintstrarr = _breakpintstr.Split(",")

            For i As Integer = 0 To _breakpintstrarr.Length - 1
                DT.Columns.Add("QTYBreakPoint" & _breakpintstrarr(i), GetType(System.Decimal))

                Dim bf As BoundField = New BoundField
                bf.HeaderText = _breakpintstrarr(i)
                bf.DataField = "QTYBreakPoint" & _breakpintstrarr(i)
                bf.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                bf.ItemStyle.Width = 60



                Me.gv1.Columns.Add(bf)

            Next

            Dim _dtQuoteDetailQuantityBreakPoint As DataTable = New DataTable
            _dtQuoteDetailQuantityBreakPoint = tbOPBase.dbGetDataTable("EQ", "Select QuoteID,line_No,partNo,QuantityBreakPoint,SAPQuantityUnitPrice,CustomerQuantityUnitPrice From QuotationDetail_BreakPoints where quoteid='" & UID & "'")

            Dim _foundrow() As DataRow = Nothing
            For Each _row As DataRow In DT.Rows

                For i As Integer = 0 To _breakpintstrarr.Length - 1

                    _foundrow = _dtQuoteDetailQuantityBreakPoint.Select("line_no=" & _row.Item("line_no") & " and QuantityBreakPoint=" & _breakpintstrarr(i))
                    'If _foundrow.Length > 0 Then _row.Item("QTYBreakPoint" & _breakpintstrarr(i)) = _currencySign & _foundrow(0).Item("CustomerQuantityUnitPrice")
                    If _foundrow.Length > 0 Then _row.Item("QTYBreakPoint" & _breakpintstrarr(i)) = _foundrow(0).Item("CustomerQuantityUnitPrice")
                Next

            Next

            Dim _subtotalrow As DataRow = DT.NewRow

            For i As Integer = 0 To DT.Columns.Count - 1
                Select Case i
                    Case 2
                        _subtotalrow.Item(i) = 9999
                    Case Else
                        _subtotalrow.Item(i) = DT.Rows(0).Item(i)
                End Select
            Next

            For i As Integer = 0 To _breakpintstrarr.Length - 1
                _subtotalrow.Item("QTYBreakPoint" & _breakpintstrarr(i)) = DT.Compute("Sum(QTYBreakPoint" & _breakpintstrarr(i) & ")", String.Empty)
            Next
            DT.Rows.Add(_subtotalrow)

            DT.AcceptChanges()

            '            Me.gv1.ShowFooter = True

        End If
        '---------------------End--------------------------------------------

        Me.gv1.DataSource = DT : Me.gv1.DataBind()
    End Sub

    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.DataRow Then
            Dim hRow As GridViewRow = Nothing
            If e.Row.RowType = DataControlRowType.Header Then
                hRow = e.Row
            Else
                hRow = gv1.HeaderRow
            End If
            If Not IsNothing(hRow) Then

                Dim lineNoCell As TableCell = Nothing
                lineNoCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "No.")

                Dim partNoCell As TableCell = Nothing
                partNoCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "Part No")

                Dim unitPriceCell As TableCell = Nothing
                unitPriceCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "Unit Price")

                Dim descriptionCell As TableCell = Nothing
                descriptionCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "Description")

                Dim extendedPriceCell As TableCell = Nothing
                extendedPriceCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "Extended Price")

                Dim virtualPartNoCell As TableCell = Nothing
                virtualPartNoCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "Virtual Part No")

                Dim listPriceCell As TableCell = Nothing
                listPriceCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "List Price")

                Dim discCell As TableCell = Nothing
                discCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "Disc.")

                Dim availableDateCell As TableCell = Nothing
                availableDateCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "Available Date")

                Dim availableQtyCell As TableCell = Nothing
                availableQtyCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "Available Qty.")

                Dim orderQtyCell As TableCell = Nothing
                orderQtyCell = COMM.Util.getGridViewCellByHeader(hRow, e.Row, "Order Qty.")


                'Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteId, COMM.Fixer.eDocType.EQ)

                Dim _pri_format As EnumSetting.USPrintOutFormat = QuoteHeaderLine.PRINTOUT_FORMAT


                If e.Row.RowType = DataControlRowType.DataRow Then
                    Dim myQD As New quotationDetail("EQ", "quotationDetail")


                    Dim DBITEM As DataRowView = CType(e.Row.DataItem, DataRowView)
                    Dim line_no As Integer = gv1.DataKeys(e.Row.RowIndex).Value
                    Dim HigherLevel As Integer = DBITEM.Item("HigherLevel")
                    'Dim part_no As String = e.Row.Cells(1).Text.Trim
                    Dim part_no As String = DBITEM.Item("partNo").ToString
                    Dim ListPice As Decimal = CDbl(CType(e.Row.FindControl("lbListPrice"), Label).Text)
                    Dim UnitPrice As Decimal = CDbl(CType(e.Row.FindControl("lbUnitPrice"), Label).Text)
                    Dim qty As Decimal = CInt(CType(e.Row.FindControl("lbGVQty"), Label).Text)
                    Dim itp As Decimal = DBITEM.Item("newITP").ToString
                    Dim Discount As Decimal = 0.0, SubTotal As Decimal = 0.0
                    Dim IsBtoParentItem As Boolean = False

                    IsBtoParentItem = myQD.isBtoParentItemV2(line_no)

                    If ListPice = 0 Then
                        CType(e.Row.FindControl("lbDisc"), Label).Text = "TBD"
                    Else
                        Discount = FormatNumber((ListPice - UnitPrice) / ListPice, 2)
                        If ListPice < UnitPrice Then
                            Discount = 0
                        End If
                        CType(e.Row.FindControl("lbDisc"), Label).Text = Discount * 100 & "%"
                    End If
                    SubTotal = FormatNumber(qty * UnitPrice, 2)
                    CType(e.Row.FindControl("lbSubTotal"), Label).Text = SubTotal

                    'If myQD.isBtoParentItem(_QuoteId, line_no) = 1 Then
                    If IsBtoParentItem Then
                        CType(e.Row.FindControl("lbDisc"), Label).Text = ""

                        'Frank(2013 / 9 / 17)
                        Dim totalamont As Decimal = 0
                        Dim DTL As IBUS.iCartList = DTList.Merge2NewCartList(DTList.Where(Function(X) X.parentLineNo.Value = DBITEM.Item("line_No")))
                        totalamont = DTL.getTotalAmount()

                        'If Me._IsV2_0Quot Then
                        '    totalamont = DTList.getTotalAmount()
                        'Else
                        '    Dim DTL As IBUS.iCartList = DTList.Merge2NewCartList(DTList.Where(Function(X) X.parentLineNo.Value = DBITEM.Item("line_No")))
                        '    totalamont = DTL.getTotalAmount()
                        'End If

                        CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(totalamont, 2)
                        CType(e.Row.FindControl("lbUnitPrice"), Label).Text = FormatNumber(totalamont / DBITEM.Item("qty"), 2)
                    End If

                    'If myQD.isBtoChildItem(_QuoteId, line_no) = 1 Or DBITEM.Item("partNo").ToString.StartsWith("AGS-EW") Then
                    'If dt.Rows(0).Item("isLumpSumOnly") = 1 And (_IsInternalUserMode = False) Then
                    '    CType(e.Row.FindControl("lbUnitPriceSign"), Label).Visible = False
                    '    CType(e.Row.FindControl("lbUnitPrice"), Label).Visible = False
                    '    CType(e.Row.FindControl("lbSubTotalSign"), Label).Visible = False
                    '    CType(e.Row.FindControl("lbSubTotal"), Label).Visible = False
                    'End If
                    'End If

                    Dim QMDT As New DataTable
                    'Dim ORG As String = dt.org
                    Dim ORG As String = QuoteHeaderLine.org
                    Dim strStatusCode As String = "", strStatusDesc As String = ""
                    'If myQD.isBtoParentItem(_QuoteId, line_no) = 0 And (Not DBITEM.Item("partNo").ToString.ToLower.StartsWith("ags-")) And Business.isInvalidPhaseOutV2(part_no, ORG, strStatusCode, strStatusDesc, 0) Then
                    'If myQD.isBtoParentItem(_QuoteId, line_no) = 0 AndAlso part_no.ToLower.StartsWith("ags-") = False AndAlso Business.isInvalidPhaseOutV2(part_no, ORG, strStatusCode, strStatusDesc, 0) Then
                    'If Not IsBtoParentItem AndAlso part_no.ToLower.StartsWith("ags-") = False AndAlso Business.isInvalidPhaseOutV2(part_no, ORG, strStatusCode, strStatusDesc, 0) Then
                    If Not IsBtoParentItem AndAlso part_no.ToLower.StartsWith("ags-") = False AndAlso Business.isInvalidPhaseOutV2(part_no, ORG, strStatusCode, strStatusDesc, 0, False) Then
                        CType(e.Row.FindControl("lbdescription"), Label).Text = "(Phase Out)"
                        CType(e.Row.FindControl("lbdescription"), Label).ForeColor = Drawing.Color.Red
                    End If

                    'Frank 2012/08/01:If line no is bigger than 100 then to show the category instead of the part_no
                    If HigherLevel > COMM.CartFixer.StartLine Then
                        e.Row.Cells(1).Text = DBITEM.Item("category").ToString
                        'ElseIf line_no < 100 Then
                        'e.Row.Cells(0).Text = line_no * 100
                    End If

                    'Frank If due date is 12/31/9999 then replace it by TBD
                    '1.For BTOS:If 100 LINE available date=12/31/9999 then display TBD string.
                    '  For BTOS:If COMPONENT stock is enough then display original available date(due date), else display TBD string
                    '2.For not BTOS:If COMPONENT available date=12/31/9999 then display TBD string.
                    If CType(e.Row.FindControl("lbDueDate"), Label).Text.Equals("12/31/9999") Then CType(e.Row.FindControl("lbDueDate"), Label).Text = "TBD"


                    'Frank 2012/07/31:Base on the column "PRINTOUT_FORMAT" to decide the part_no and unit price pisplay logic.

                    'Dim _pri_format As EnumSetting.USPrintOutFormat = dt.PRINTOUT_FORMAT
                    'Dim _pri_format As EnumSetting.USPrintOutFormat = QuoteHeaderLine.PRINTOUT_FORMAT
                    '_pri_format = EnumSetting.USPrintOutFormat.MAIN_ITEM_ONLY
                    '_pri_format = EnumSetting.USPrintOutFormat.SUB_ITEM_WITH_SUB_ITEM_PRICE
                    '_pri_format=EnumSetting.USPrintOutFormat.SUB_ITEM_WITHOUT_SUB_ITEM_PRICE
                    Select Case _pri_format
                        Case EnumSetting.USPrintOutFormat.MAIN_ITEM_ONLY
                            If line_no Mod 100 > 0 AndAlso line_no > 100 Then
                                e.Row.Visible = False
                            End If
                        Case EnumSetting.USPrintOutFormat.SUB_ITEM_WITH_SUB_ITEM_PRICE
                            If line_no Mod 100 > 0 AndAlso line_no > 100 Then
                                'Nada revised
                                If Not IsNothing(partNoCell) Then
                                    partNoCell.Text = DBITEM.Item("partNo").ToString
                                End If
                            End If
                        Case EnumSetting.USPrintOutFormat.SUB_ITEM_WITHOUT_SUB_ITEM_PRICE
                            If line_no Mod 100 > 0 AndAlso line_no > 100 Then
                                'e.Row.Cells(1).Text = "Other"
                                'Nada revised
                                If Not IsNothing(partNoCell) Then
                                    'partNoCell.Text = "Other"
                                    'ICC 2015/5/28 Change display text from Other to Category
                                    Dim category As String = DBITEM.Item("Category").ToString()
                                    Dim lastWord As Integer = category.ToUpper().LastIndexOf("FOR")
                                    If lastWord > -1 Then
                                        partNoCell.Text = category.Substring(0, lastWord).Trim()
                                    Else
                                        partNoCell.Text = category
                                    End If
                                End If
                                If Not IsNothing(unitPriceCell) Then
                                    unitPriceCell.Text = ""
                                End If
                                If Not IsNothing(extendedPriceCell) Then
                                    extendedPriceCell.Text = ""
                                End If
                                'e.Row.Cells(9).Text = ""
                                'e.Row.Cells(11).Text = ""
                            End If
                        Case EnumSetting.USPrintOutFormat.SUB_ITEM_WITHPARTNO_WITHOUT_SUB_ITEM_PRICE
                            If line_no Mod 100 > 0 AndAlso line_no > 100 Then
                                If Not IsNothing(partNoCell) Then
                                    partNoCell.Text = DBITEM.Item("partNo").ToString
                                End If
                                'e.Row.Cells(1).Text = DBITEM.Item("partNo").ToString
                                If Not IsNothing(unitPriceCell) Then
                                    unitPriceCell.Text = ""
                                End If
                                If Not IsNothing(extendedPriceCell) Then
                                    extendedPriceCell.Text = ""
                                End If
                            End If
                        Case EnumSetting.USPrintOutFormat.SUB_ITEM_WITH_BREAKPOINTS
                            For i As Integer = e.Row.Cells.Count - 1 To (e.Row.Cells.Count - 1 - Me._breakpintstrarr.Count) Step -1
                                e.Row.Cells(i).Text = _currencySign & e.Row.Cells(i).Text
                            Next
                            If lineNoCell.Text = "9999" Then
                                lineNoCell.Text = ""
                                partNoCell.Text = ""
                                descriptionCell.Text = "TOTALS"
                                descriptionCell.HorizontalAlign = HorizontalAlign.Right
                            End If



                            'Case EnumSetting.USPrintOutFormat.IS_SHOW_VIRTUAL_ITEM_ONLY
                            '    e.Row.Cells(1).Text = DBITEM.Item("partNo").ToString
                            '    If Not IsDBNull(DBITEM.Item("custMaterial")) AndAlso Not String.IsNullOrEmpty(DBITEM.Item("custMaterial")) Then
                            '        e.Row.Cells(1).Text = DBITEM.Item("custMaterial").ToString
                            '    End If
                    End Select

                End If


                'Frank 20160530
                If _pri_format = EnumSetting.USPrintOutFormat.SUB_ITEM_WITH_BREAKPOINTS Then
                    orderQtyCell.Visible = False
                    unitPriceCell.Visible = False
                    availableQtyCell.Visible = False
                    extendedPriceCell.Visible = False
                End If


                'Nada revised.......................
                'Dim _Vonly As COMM.Fixer.eIsVirNoOnly = dt.ISVIRPARTONLY
                Dim _Vonly As COMM.Fixer.eIsVirNoOnly = QuoteHeaderLine.ISVIRPARTONLY
                If _Vonly = COMM.Fixer.eIsVirNoOnly.CustPartNoOnly Then
                    If Not IsNothing(partNoCell) Then
                        partNoCell.Visible = False
                    End If
                End If
                If _Vonly = COMM.Fixer.eIsVirNoOnly.PartNoOnly Then
                    If Not IsNothing(virtualPartNoCell) Then
                        virtualPartNoCell.Visible = False
                    End If
                End If


                If Not IsNothing(listPriceCell) Then
                    listPriceCell.Visible = False
                End If
                If Not IsNothing(discCell) Then
                    discCell.Visible = False
                End If

                'Me.gv1.Columns(5).Visible = False : Me.gv1.Columns(7).Visible = False
                'Me.gv1.Columns(9).Visible = False
                If _IsInternalUserMode = False Then
                    If Not IsNothing(availableDateCell) Then
                        availableDateCell.Visible = False
                    End If
                    If Not IsNothing(availableQtyCell) Then
                        availableQtyCell.Visible = False
                    End If
                    'Me.gv1.Columns(3).Visible = False   'Frank 2012/07/31: hiding Available(Req Ship) date
                    'Me.gv1.Columns(4).Visible = False   'Frank 2012/09/1: hiding Available qty
                End If
                '/Nada revised.......................

            End If
        End If
    End Sub

    Public Function getLocalTime(ByVal org As String, ByVal ServerTime As DateTime) As DateTime
        Return Util.GetLocalTime(org, ServerTime)
    End Function
    Public Function GetRuntimeSiteUrl()
        Return Util.GetRuntimeSiteUrl()
    End Function
    Dim DTList As IBUS.iCartList = Nothing
    Private Sub gv1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv1.DataBinding
        DTList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Pivot.CurrentProfile.getCurrOrg).GetListAll(COMM.Fixer.eDocType.EQ)
    End Sub

End Class