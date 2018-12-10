Public Class JPAOnlineQuoteTemplateV3
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

    Dim aptQD As New EQDSTableAdapters.QuotationDetailTableAdapter
    Dim _tax As Decimal = CDec(0.05)
    Dim dtM As IBUS.iDocHeaderLine, dtDetail As IBUS.iCartList, dtME As Quote_Master_Extension
    Protected Sub RepeaterBtosDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RepeaterBtosDetail.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
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
            Dim newunitPrice As HtmlInputText = CType(e.Item.FindControl("newunitPrice"), HtmlInputText)
            newunitPrice.Value = "¥ " + FormatNumber(currentcartline.newunitPrice.Value, 0, , TriState.True)
            Dim subtotal As HtmlInputText = CType(e.Item.FindControl("subtotal"), HtmlInputText)
            subtotal.Value = "¥ " + FormatNumber(currentcartline.newunitPrice.Value * currentcartline.Qty.Value, 0, , TriState.True)
            If currentcartline.itemType.Value = COMM.Fixer.eItemType.Parent Then
                Dim gvdetail As GridView = CType(e.Item.FindControl("gv1"), GridView)
                Dim newCart As IBUS.iCartList = dtDetail.Merge2NewCartList(dtDetail.Where(Function(x) x.parentLineNo.Value = currentcartline.lineNo.Value))
                newunitPrice.Value = "¥ " + FormatNumber(newCart.getTotalAmount() / currentcartline.Qty.Value, 0, , , TriState.True)
                subtotal.Value = "¥ " + FormatNumber(newCart.getTotalAmount(), 0, , , TriState.True)
                Dim Ew As IBUS.iCartLine = newCart.Where(Function(p) p.partNo.Value.ToString.StartsWith(COMM.Fixer.AGSEWPrefix, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault()
                If Ew IsNot Nothing Then
                    EwFlagPrice = FormatNumber(Ew.newunitPrice.Value, 0, , , TriState.True) * (1 + _tax)
                Else
                    EwFlagPrice = 0
                End If
                EWAmount.Value = "Extended Warranty for " + currentcartline.ewFlag.Value.ToString + " Months: ¥ " + FormatNumber(EwFlagPrice, 0, , , TriState.True)
                gvdetail.DataSource = newCart
                gvdetail.DataBind()
                Dim btosdetail As HtmlTableRow = CType(e.Item.FindControl("btosdetail"), HtmlTableRow)
                btosdetail.Visible = True
            End If

            If currentcartline.ewFlag.Value.ToString.Equals("0") Then
                Dim trEW As HtmlTableRow = CType(e.Item.FindControl("trEWAmount"), HtmlTableRow)
                trEW.Visible = False
            End If
        End If
    End Sub
    Public Sub LoadData()
        dtM = Pivot.NewObjDocHeader.GetByDocID(_QuoteId, COMM.Fixer.eDocType.EQ)
        dtDetail = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, _QuoteId, dtM.org).GetListAll(COMM.Fixer.eDocType.EQ)
        dtME = MyQuoteX.GetMasterExtension(_QuoteId)

        If Not dtDetail Is Nothing AndAlso dtDetail.Where(Function(x) x.itemType.Value = COMM.Fixer.eItemType.Parent).Count > 0 Then Me._IsBTosQuotation = True
        If Not IsNothing(dtM) Then
            _currencySign = Util.getCurrSign(dtM.currency) : _IsLumpSumOnly = dtM.isLumpSumOnly
            _tax = dtM.tax
            If _tax < 0 Then _tax = 0.05
            FillQuoteInfo(dtM, dtDetail)
        End If
        If Me._IsBTosQuotation Then
            trBTOS.Visible = True
            Dim result = dtDetail.Where(Function(p) Not p.partNo.Value.ToString.StartsWith(COMM.Fixer.AGSEWPrefix, StringComparison.CurrentCultureIgnoreCase) And (p.lineNo.Value < 100 Or p.itemType.Value = COMM.Fixer.eItemType.Parent))
            RepeaterBtosDetail.DataSource = result : RepeaterBtosDetail.DataBind()
        Else
            trLooseItems.Visible = True
            Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", "select line_no, partno, description as part_desc, qty, newUnitPrice from quotationdetail where quoteid = '" + _QuoteId + "'")
            gvLooseItems.DataSource = dt : gvLooseItems.DataBind()
        End If
        If Not IsNothing(dtME) AndAlso Not String.IsNullOrEmpty(dtME.JPCustomerOffice) Then
            If dtME.JPCustomerOffice.Equals("1") Then
                AdvantechAddr.Value = "〒1110032 東京都台東区浅草6-16-3"
                AdvantechTel.Value = "03-6802-1021"
                AdvantechFax.Value = "03-6802-1022"
            ElseIf dtME.JPCustomerOffice.Equals("2") Then
                trAdvantechAddr2.Visible = True

                AdvantechAddr.Value = "〒5420081 大阪府大阪市中央区南船場1-10-20"
                AdvantechAddr2.Value = "南船場M21ビル6F"
                AdvantechTel.Value = "06-6267-1887"
                AdvantechFax.Value = "06-6267-1886"
            ElseIf dtME.JPCustomerOffice.Equals("3") Then
                AdvantechAddr.Value = "〒4600008 名古屋市中区栄4-3-26　昭和ビル9階"
                AdvantechTel.Value = "052-241-2490"
                AdvantechFax.Value = "06-6267-1886"
            End If
        Else
            AdvantechAddr.Value = "〒1110032 東京都台東区浅草6-16-3"
            AdvantechTel.Value = "03-6802-1021"
            AdvantechFax.Value = "03-6802-1022"
        End If

    End Sub
    Protected Sub FillQuoteInfo(ByRef QuoteMasterRow As IBUS.iDocHeaderLine, ByRef QuoteDetailTb As IBUS.iCartList)

        If Not QuoteMasterRow Is Nothing Then


            Dim dt_AJPExtension As DataTable = tbOPBase.dbGetDataTable("EQ", String.Format("select * from QuotationExtensionAJP where quoteid = '{0}'", _QuoteId))

            Dim strSAP As String = " SELECT a.kunnr, b.post_code1, b.street, b.str_suppl3, b.name2 " +
                                   " FROM SAPRDP.KNA1 a inner join SAPRDP.ADRC b on a.adrnr = b.addrnumber " +
                                   " inner join saprdp.knvv c On a.KUNNR = c.KUNNR WHERE a.kunnr = '" + dtM.AccErpId + "'"
            Dim dt_SAPAddress As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSAP)

            Dim _CustomerName As String = String.Empty
            Dim _CustomerName2 As String = String.Empty
            Dim _CustomerAddress As String = String.Empty
            Dim _CustomerAddress2 As String = String.Empty
            Dim _CustomerTel As String = Nothing
            Dim _CustomerFax As String = Nothing
            Dim _CustomerContact As String = String.Empty
            Dim _CustomerPaymentTerm As String = String.Empty
            Dim _CustomerEM As String = String.Empty
            Dim _CustomerShipMethod As String = String.Empty
            Dim _SalesName As String = String.Empty
            Dim _Creator As String = String.Empty

            If Not dt_SAPAddress Is Nothing AndAlso dt_SAPAddress.Rows.Count > 0 Then
                _CustomerName2 = dt_SAPAddress.Rows(0).Item("NAME2")

                If Not String.IsNullOrEmpty(dt_SAPAddress.Rows(0).Item("post_code1").ToString.Replace("　", " ")) Then
                    _CustomerAddress += "〒" + dt_SAPAddress.Rows(0).Item("post_code1").ToString.Replace("　", " ") + " "
                End If
                _CustomerAddress += dt_SAPAddress.Rows(0).Item("street").ToString.Replace("　", " ")

                'If address.length is too long, then split it to two parts.
                If (_CustomerAddress + dt_SAPAddress.Rows(0).Item("str_suppl3").ToString.Replace("　", " ")).Length > 25 Then
                    _CustomerAddress2 = dt_SAPAddress.Rows(0).Item("str_suppl3").ToString.Replace("　", " ").Trim
                Else
                    _CustomerAddress += dt_SAPAddress.Rows(0).Item("str_suppl3").ToString.Replace("　", " ")
                End If
            End If

            If Not dt_AJPExtension Is Nothing AndAlso dt_AJPExtension.Rows.Count > 0 Then
                _CustomerName = dt_AJPExtension.Rows(0).Item("CustomerName")
                _CustomerName2 = dt_AJPExtension.Rows(0).Item("CustomerName2")
                _CustomerAddress = dt_AJPExtension.Rows(0).Item("CustomerAddr")
                _CustomerAddress2 = dt_AJPExtension.Rows(0).Item("CustomerAddr2")
                _CustomerTel = dt_AJPExtension.Rows(0).Item("CustomerTel")
                _CustomerFax = dt_AJPExtension.Rows(0).Item("CustomerFax")
                '_CustomerContact = dt_AJPExtension.Rows(0).Item("CustomerContact")
                _CustomerPaymentTerm = dt_AJPExtension.Rows(0).Item("CustomerPaymentTerm")
                _CustomerEM = dt_AJPExtension.Rows(0).Item("CustomerEM")
                _CustomerShipMethod = dt_AJPExtension.Rows(0).Item("CustomerShipMethod")
                _SalesName = dt_AJPExtension.Rows(0).Item("SalesName")
                _Creator = dt_AJPExtension.Rows(0).Item("Creator")
            End If

            If Not String.IsNullOrEmpty(_CustomerAddress2) AndAlso Not String.IsNullOrWhiteSpace(_CustomerAddress2) Then
                Me.CustomerAddr2.Visible = True
            End If

            With QuoteMasterRow
                Me.LabelQuoteID.Value = .quoteNo
                Me.quoteDate.Value = Util.GetLocalTime("AJP", .DocDate).ToString("MM/dd/yyyy")
                'Getting sold to, ship to and bill to data from [eQuotation].[dbo].[EQPARTNER]
                Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
                Dim SoldToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "SOLDTO")
                If SoldToTable.Count > 0 Then
                    Dim SoldToRow As EQDS.EQPARTNERRow = SoldToTable(0)
                    Me.CustomerName.Value = IIf(String.IsNullOrEmpty(_CustomerName), SoldToRow.NAME, _CustomerName)
                    Me.CustomerTel.Value = IIf(_CustomerTel Is Nothing, SoldToRow.TEL, _CustomerTel)
                    Me.CustomerFax.Value = IIf(_CustomerFax Is Nothing, Business.GetAJPAccountFax(QuoteId, MasterRef.DocType), _CustomerFax)
                End If

                Me.CustomerName2.Value = IIf(String.IsNullOrEmpty(_CustomerName2), "", _CustomerName2)
                Me.CustomerAddr.Value = IIf(String.IsNullOrEmpty(_CustomerAddress), "", _CustomerAddress)
                Me.CustomerAddr2.Value = IIf(String.IsNullOrEmpty(_CustomerAddress2), "", _CustomerAddress2)
                Me.CustomerEM.Value = IIf(String.IsNullOrEmpty(_CustomerEM), "", _CustomerEM)
                Me.CustomerContact.Value = IIf(String.IsNullOrEmpty(.attentionEmail), "", .attentionEmail)

                Me.CustomerPaymentTerm.Value = IIf(String.IsNullOrEmpty(_CustomerPaymentTerm), SAPTools.GetPaymentMethodNameByValue(.paymentTerm), _CustomerPaymentTerm)
                If Me.CustomerPaymentTerm.Value = "0" Then Me.CustomerPaymentTerm.Value = "TBD"

                Me.CustomerShipMethod.Value = IIf(String.IsNullOrEmpty(_CustomerShipMethod), "別途打ち合わせ", _CustomerShipMethod)
                Me.txtNotes.Text = .quoteNote

                'Frank 2012/09/06 Get Sales Representative by EmployeeID of EQPartner or quotation creator
                Dim Employee1 As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "E")
                If Employee1.Count > 0 Then
                    Dim Employee1Row As EQDS.EQPARTNERRow = Employee1(0)
                    Me.SalesName.Value = IIf(String.IsNullOrEmpty(_SalesName), SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Employee1Row.ERPID, .CreatedBy), _SalesName)
                End If
                Me.expiredDate.Value = Util.GetLocalTime("AJP", .expiredDate).ToString("MM/dd/yyyy")

                Me.Creator.Value = IIf(String.IsNullOrEmpty(_Creator), SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(String.Empty, .CreatedBy), _Creator)
            End With
        End If

        If Not QuoteDetailTb Is Nothing AndAlso QuoteDetailTb.Count > 0 Then
            'Fill total amount info
            Dim L As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org).GetListAll(COMM.Fixer.eDocType.EQ)
            If _currencySign.Equals("&yen;") Then
                lbPretaxAmount.Text = _currencySign + Decimal.Round(L.getTotalAmount, 0).ToString("N0")
                lbTaxAmount.Text = _currencySign + Decimal.Round(L.GetTaxableAmount(dtM.AccErpId) * dtM.tax, 0).ToString("N0")
                lbPosttaxAmount.Text = _currencySign + Decimal.Round(L.GetTaxableAmount(dtM.AccErpId) * (1 + dtM.tax), 0).ToString("N0")
            Else
                lbPretaxAmount.Text = _currencySign + Decimal.Round(L.getTotalAmount, 0).ToString("N")
                lbTaxAmount.Text = _currencySign + Decimal.Round(L.GetTaxableAmount(dtM.AccErpId) * dtM.tax, 0).ToString("N")
                lbPosttaxAmount.Text = _currencySign + Decimal.Round(L.GetTaxableAmount(dtM.AccErpId) * (1 + dtM.tax), 0).ToString("N")
            End If
        End If
    End Sub

    Protected Sub initGV(ByRef QuoteDetailTb As IBUS.iCartList)

        'Frank:Refresh product inventory information
        Business.RefreshQuotationInventory(_QuoteId)

        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim DT As DataTable = myQD.GetDT(String.Format("quoteId='{0}'", _QuoteId), "line_No")

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
    End Sub

    Protected Sub gvLooseItems_RowDataBound(sender As Object, e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim DBITEM As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim unitprice As Decimal = Convert.ToDecimal(DBITEM.Item("newunitPrice").ToString)
            Dim qty As Integer = Convert.ToInt32(DBITEM.Item("qty").ToString)

            'currency round
            If _currencySign.Equals("&yen;") Then
                e.Row.Cells(4).Text = _currencySign + Decimal.Round(unitprice, 0).ToString("N0")
                e.Row.Cells(5).Text = _currencySign + Decimal.Round(unitprice * qty, 0).ToString("N0")
            Else
                e.Row.Cells(4).Text = _currencySign + unitprice.ToString("N")
                e.Row.Cells(5).Text = _currencySign + (unitprice * qty).ToString("N")
            End If

        End If

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