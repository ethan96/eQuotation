Imports SAPDAL

Public Class quoteModify
    Inherits PageBase
    Dim myQD As New quotationDetail("EQ", "quotationDetail")
    Public currencySign As String = "", _IsAnyPhaseOutProd As Boolean = False, org As String = String.Empty
    Dim _ProductList As New List(Of ProductX), QuoteMaster As Quote_Master = Nothing
    Dim Quotelist As List(Of QuoteItem) = Nothing
    Dim _isATW As Boolean, _IsAnyBelowMinimumPrice As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        QuoteMaster = MyQuoteX.GetQuoteMaster(UID)
        Me._isATW = Role.IsTWAonlineSales()
        If Not IsPostBack Then
            initGV(Request("UID"))
            currencySign = ""

            'Frank 20151126 Only AEU need to stop at this page first
            'If Role.IsUsaUser OrElse Role.IsFranchiser _
            '    OrElse Role.IsTWAonlineSales _
            '    OrElse Role.IsCNAonlineSales _
            '    OrElse Role.IsKRAonlineSales _
            '    OrElse Role.IsHQDCSales Then

            '    If Me._IsAnyPhaseOutProd = False Then Me.btnConfirm_Click(Me.btnConfirm, Nothing)

            'End If
            If Not Role.IsEUSales Then
                If Me._IsAnyPhaseOutProd = False Then Me.btnConfirm_Click(Me.btnConfirm, Nothing)
            End If

            ' Dim M As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
            'getPageStr(Request("UID"), QuoteMaster.DocReg)
            With QuoteMaster
                Me.lbCustomId.Text = .PO_NO
                Me.lbQuoteID.Text = .quoteNo
                Me.lbCreatedBy.Text = .createdBy
                Me.lbQuoteDate.Text = .quoteDate
                Me.lbPreparedBy.Text = .preparedBy
                Me.lbQuoteToName.Text = .quoteToName
                If .quoteToErpId <> "" Then
                    Me.lbQuoteToId.Text = "(" & .quoteToErpId & ")"
                End If
                Me.lbOffice.Text = .office
                Me.lbCurrency.Text = .currency
                Me.lbSalesEmail.Text = .salesEmail
                Me.lbDirectPhone.Text = .directPhone
                Me.lbAttention.Text = .attentionEmail
                Me.lbBankAccount.Text = .bankInfo
                Me.lbDeliveryDate.Text = .deliveryDate
                Me.lbExpiredDate.Text = .expiredDate
                Me.lbShippingTerms.Text = .shipTerm
                Me.lbPaymentTerms.Text = .paymentTerm
                Me.lbFreight.Text = IIf(.freight = "0", "TBD", .freight)
                Me.lbInsurance.Text = .insurance
                Me.lbSpecialCharge.Text = .specialCharge
                Me.lbTax.Text = .tax
                Me.lbQuoteNotes.Text = .quoteNote
                Me.lbRelatedInformation.Text = .relatedInfo
                Me.lbAddressInfo.Text = Business.getRUBAddress(.office, .ogroup, .DocReg)
                If .DelDateFlag = 1 Then
                    Me.lbDeliveryDate.Text = "To Be Verified"
                End If
                If Not IsNothing(Request("ROLE")) AndAlso Request("ROLE") = 1 Then
                    Me.trRelatedInfo.Visible = False
                End If
            End With
        End If
    End Sub
    'Sub getPageStr(ByVal UID As String, ByVal DocReg As COMM.Fixer.eDocReg)
    'Dim logoBlock As String = "", headerBlock As String = "", detailBlock As String = ""
    'Dim url As String = String.Format("~/Quote/{1}?UID={0}", UID, Business.getPiPage(UID, DocReg))
    'Dim MyDOC As New System.Xml.XmlDocument
    'Util.HtmlToXML(url, MyDOC)
    'Util.getXmlBlockByID("div", "divLogo", MyDOC, logoBlock)
    'Util.getXmlBlockByID("div", "divMaster", MyDOC, headerBlock)
    'Me.divContent.InnerHtml = logoBlock & headerBlock
    'End Sub
    Protected Sub initGV(ByVal UID As String)
        ' Dim DT As DataTable = myQD.GetDT(String.Format("quoteId='{0}'", UID), "line_No")
        Quotelist = MyQuoteX.GetQuoteList(UID)
        ' Dim QMDT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
        org = QuoteMaster.org
        ' Me._IsAnyPhaseOutProd = SAPDAL.SAPDAL.GetSAPProductInfo(QuoteMaster.OriginalQuoteID, DT)
        Dim _ProductX As New ProductX()
        For Each i As QuoteItem In Quotelist
            _ProductList.Add(New ProductX(i.partNo, org, i.deliveryPlant))
        Next
        _ProductList = _ProductX.GetProductInfo(_ProductList, org, _IsAnyPhaseOutProd)

        If Me._isATW Then
            Dim PNlist As New List(Of PNInfo)
            For Each _quoteitem In Quotelist
                If Not Business.IsPartInBTOS_CTOSMaterialGroup(_quoteitem.partNo) Then
                    Dim _pord As ProductX = _ProductList.Find(Function(x) x.PartNo = _quoteitem.partNo)
                    If _pord IsNot Nothing AndAlso Not _pord.IsPhaseOut Then
                        PNlist.Add(New PNInfo(_quoteitem.partNo, 1, _quoteitem.deliveryPlant))
                    End If
                End If
            Next
            If PNlist.Count > 0 Then
                'Here the line items of _order have been filtered by phase out status
                Dim _order As Advantech.Myadvantech.DataAccess.Order = MyQuoteX.getMultiPrice(UID, PNlist)

                For Each _quoteitem As QuoteItem In Quotelist
                    Dim _orderitem As Advantech.Myadvantech.DataAccess.Product = _order.LineItems.Find(Function(x) x.PartNumber = _quoteitem.partNo)

                    If Not _orderitem Is Nothing Then
                        _quoteitem.MinimumPrice = _orderitem.MinimumPrice
                        If _quoteitem.unitPrice < _orderitem.MinimumPrice Then
                            _IsAnyBelowMinimumPrice = True
                        End If
                    End If
                Next
            End If
        End If


        'If Not COMM.Util.IsTesting() Then
        If (Role.IsUsaUser OrElse Role.IsFranchiser) Then
            If _IsAnyPhaseOutProd = False Then Exit Sub
        End If
        'End If
        Me.gv1.DataSource = Quotelist
        Me.gv1.DataBind()
        'Frank 2012089: If there are any product in phase out status, then display below 3 columns to let user get the information.
        If Me._IsAnyPhaseOutProd OrElse Me._IsAnyBelowMinimumPrice Then
            Me.gv1.Columns(18).Visible = True : Me.btnConfirm.Enabled = False
        Else
            ' Me.gv1.Columns(18).Visible = False
            'Control btnConfirm enable status by Quote detail record count
            If Me.gv1.Rows.Count <= 0 Then
                Me.btnConfirm.Enabled = False
            Else
                Me.btnConfirm.Enabled = True
            End If
        End If
    End Sub
    Dim PhaseOutPartNo As New ArrayList
    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim _item As QuoteItem = CType(e.Row.DataItem, QuoteItem)
            Dim line_no As Integer = _item.line_No ' gv1.DataKeys(e.Row.RowIndex).Value
            'Dim part_no As String = DBITEM.Item("partNo").ToString
            'Dim ListPice As Decimal = CDbl(CType(e.Row.FindControl("lbListPrice"), Label).Text)
            'Dim UnitPrice As Decimal = CDbl(CType(e.Row.FindControl("txtUnitPrice"), TextBox).Text)
            Dim itp As Decimal = _item.newItp ' DBITEM.Item("newITP").ToString
            Dim qty As Decimal = CInt(CType(e.Row.FindControl("txtGVQty"), TextBox).Text)

            Dim Discount As Decimal = 0.0
            Dim SubTotal As Decimal = 0.0
            Dim ewPrice As Decimal = 0.0
            Dim DrpEW As DropDownList = CType(e.Row.FindControl("gv_drpEW"), DropDownList)

            If _item.ewFlag = 99 Then
                DrpEW.Items.Clear()
                DrpEW.Items.Add(New ListItem("without EW", "0"))
                DrpEW.Items.Add(New ListItem("36 months", "99"))
            End If
            If _item.ewFlag = 999 Then
                DrpEW.Items.Clear()
                DrpEW.Items.Add(New ListItem("without EW", "0"))
                DrpEW.Items.Add(New ListItem("3 months", "999"))
            End If

            DrpEW.SelectedValue = _item.ewFlag
            ewPrice = FormatNumber(Business.getRateByEWItem(Business.getEWItemByMonth(CInt(DrpEW.SelectedValue), MasterRef.DocReg), MasterRef.DocReg) * _item.unitPrice, 2)
            CType(e.Row.FindControl("gv_lbEW"), TextBox).Text = ewPrice

            If _item.listPrice = 0 Then
                e.Row.Cells(9).Text = "TBD"
                e.Row.Cells(11).Text = "TBD"
            Else
                Discount = FormatNumber((_item.listPrice - _item.unitPrice) / _item.listPrice, 2)
                If _item.listPrice < _item.unitPrice Then
                    Discount = 0
                End If
                e.Row.Cells(11).Text = Discount * 100 & "%"
            End If


            'SubTotal = FormatNumber(qty * (_item.unitPrice + ewPrice), 2)
            'CType(e.Row.FindControl("lbSubTotal"), Label).Text = SubTotal
            If _item.unitPrice <> 0 Then
                CType(e.Row.FindControl("lbMargin"), Label).Text = FormatNumber((_item.unitPrice - itp) * 100 / _item.unitPrice, 2) & "%"
            End If
            If _item.ItemTypeX = QuoteItemType.BtosParent Then
                e.Row.Cells(1).Text = ""
                e.Row.Cells(3).Text = ""
                e.Row.Cells(5).Text = ""
                e.Row.Cells(6).Text = ""
                e.Row.Cells(7).Text = ""
                e.Row.Cells(8).Text = ""
                e.Row.Cells(11).Text = ""
                e.Row.Cells(13).Text = ""
                e.Row.Cells(16).Text = ""
                Dim TAmt As Decimal = 0
                Dim TPrice As Decimal = 0
                Dim TLPrice As Decimal = 0
                'If Not IsNothing(DTList) Then
                '    Dim subL As IBUS.iCartList = DTList.Merge2NewCartList(DTList.Where(Function(x) x.parentLineNo.Value = DBITEM.Item("Line_No").ToString))
                '    If Not IsNothing(subL) Then
                '        TAmt = subL.getTotalAmount
                '        If TAmt > 0 AndAlso CInt(_item.qty) > 0 Then
                '            TPrice = TAmt / _item.qty
                '        End If
                '    End If
                '    TLPrice = subL.getTotalListAmount() / _item.qty
                'End If
                'e.Row.Cells(9).Text = TLPrice
                'CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(TAmt, 2)
                'CType(e.Row.FindControl("txtUnitPrice"), TextBox).Text = FormatNumber(TPrice, 2)
            End If
            If _item.IsEWpartnoX Then
                CType(e.Row.FindControl("txtUnitPrice"), TextBox).Enabled = False
                CType(e.Row.FindControl("txtGVQty"), TextBox).Enabled = False
                CType(e.Row.FindControl("btnSpecialItp"), Button).Enabled = False
            End If
            'Dim QMDT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
            'Dim ORG As String = QMDT.org
            ''If QMDT.Rows(0).Item("quoteToErpId") <> "" Then
            ''    ORG = Business.getOrgByErpId(QMDT.Rows(0).Item("quoteToErpId"))
            ''End If
            Dim strStatusCode As String = "", strStatusDesc As String = ""
            Dim _currproductX = _ProductList.Where(Function(p) p.PartNo = _item.partNo).FirstOrDefault
            If _currproductX IsNot Nothing AndAlso _currproductX.IsPhaseOut Then
                If Not PhaseOutPartNo.Contains(_currproductX.PartNo) Then PhaseOutPartNo.Add(_currproductX.PartNo)
                Dim txtDescription As TextBox = CType(e.Row.FindControl("txtDescription"), TextBox)
                If txtDescription IsNot Nothing Then
                    txtDescription.Text = "Status [ " + _currproductX.StatusCode + " ]: " + _currproductX.StatusDesc
                    txtDescription.ForeColor = Drawing.Color.Red
                End If
            End If

            'Frank 20180205 ATW need to check if unitprice is below minimum price
            If Me._isATW AndAlso _item.IsBelowMinimumPrice Then
                If Not PhaseOutPartNo.Contains(_currproductX.PartNo) Then PhaseOutPartNo.Add(_currproductX.PartNo)
                Dim txtDescription As TextBox = CType(e.Row.FindControl("txtDescription"), TextBox)
                If txtDescription IsNot Nothing Then
                    txtDescription.Text = "Below Minimum Price " & _item.MinimumPrice.ToString("N2")
                    txtDescription.ForeColor = Drawing.Color.Red
                End If
            End If

            If _currproductX IsNot Nothing Then

                CType(e.Row.FindControl("lbProductStatus"), Label).Text = "Status [ " + _currproductX.StatusCode + " ]: " + _currproductX.StatusDesc
                If _currproductX.IsPhaseOut Then
                    CType(e.Row.FindControl("lbProductStatus"), Label).ForeColor = Drawing.Color.Red
                End If
            End If

        End If
        'If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Then
        '    If myQD.isBtoOrder(Request("UID")) = 0 Then
        '        e.Row.Cells(3).Visible = False
        '    End If
        '    If myQD.isBtoOrder(Request("UID")) = 1 Then
        '        e.Row.Cells(6).Visible = False
        '    End If
        '    e.Row.Cells(7).Visible = False
        '    e.Row.Cells(8).Visible = False
        'End If
    End Sub
    Protected Sub txtGVQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As TextBox = CType(sender, TextBox)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value
        Dim Qty As String = obj.Text
        Dim _QuoteItem As QuoteItem = MyQuoteX.GetQuoteItem(Request("UID"), id)
        If _QuoteItem IsNot Nothing AndAlso Integer.TryParse(Qty, 0) Then
            Dim _oriqty As Integer = _QuoteItem.qty
            If _oriqty < 1 Then _oriqty = 1
            _QuoteItem.qty = Integer.Parse(Qty)
            If _QuoteItem.ItemTypeX = QuoteItemType.BtosParent Then

                Dim _sublist As List(Of QuoteItem) = _QuoteItem.ChildListX
                If _sublist.Count > 0 Then
                    For Each i As QuoteItem In _sublist
                        'i.qty = i.qty * 2
                        If (i.qty / _oriqty) < 1 Then
                            'i.qty = i.qty
                        Else
                            i.qty = (i.qty / _oriqty) * _QuoteItem.qty
                        End If
                        If i.qty < 1 Then i.qty = 1
                    Next
                End If
            ElseIf _QuoteItem.HigherLevel > 0 Then
            End If
            MyUtil.Current.CurrentDataContext.SubmitChanges()
        End If
        'If id = 100 Then
        '    myQD.Update(String.Format("quoteId='{0}'", Request("UID")), String.Format("qty='{0}'", Qty))
        '    '/ReCalDueDateForEachLine/'
        'Else
        '    myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", Request("UID"), id), String.Format("qty='{0}'", Qty))
        '    'ReCalDue(Request("UID"), id)
        'End If
    End Sub
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' Me.upbtnConfirm.Update()



        If Me._isATW Then
            Me.lbtotal.Text = MyQuoteX.GetATWTotalAmount(UID)
        Else
            Me.lbtotal.Text = MyQuoteX.GetTotalPrice(UID) ' FormatNumber(Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org).GetListAll(COMM.Fixer.eDocType.EQ).getTotalAmount, 2)
        End If

        'Dim QDDT As New DataTable
        'QDDT = myQD.GetDT(String.Format("quoteId='{0}'", Request("UID")), "line_No")
        Dim detail As New List(Of struct_GP_Detail)
        For Each x As QuoteItem In Quotelist
            Dim detailLine As New struct_GP_Detail
            detailLine.lineNo = x.line_No
            detailLine.PartNo = x.partNo
            detailLine.Price = x.newUnitPrice
            detailLine.QTY = x.qty
            detailLine.Itp = x.newItp
            detail.Add(detailLine)
        Next
        If detail.Count > 0 Then
            Dim MSTD As Decimal = GPControl.getMarginWithOutAGS(detail)
            '此语句调到第205行，提供页面执行效率
            'Dim MPTD As Decimal = GPControl.getMarginPTD(detail)
            Dim MLST As Decimal = -99999
            If MSTD <> -99999 Then
                MLST = MSTD
            Else
                MLST = GPControl.getMarginPTD(detail)
            End If
            Me.lbTotalMargin.Text = FormatNumber(MLST * 100, 2)
        End If

    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim QuoteID As String = Request("UID")

        Dim QM As IBUS.iDocHeader = New DOCH.DocHeader
        Dim _IsV2_0Quote As Boolean = QM.IsV2_0Quotation(QuoteID)
        If Not Business.IsOrderable(QuoteID) AndAlso Not _IsV2_0Quote Then
            Exit Sub
        End If
        Dim TID As String = Pivot.CurrentProfile.SSOID, USER As String = Pivot.CurrentProfile.UserId, COMPANY As String = "", ORG As String = ""
        Dim DTQM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(QuoteID, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(DTQM) Then
            COMPANY = DTQM.AccErpId
            If Role.IsFranchiser() Then
                COMPANY = Franchise.getSampleERPidForFranchise(DTQM.CreatedBy)
            End If
            If Business.is_Valid_Company_Id(COMPANY) = False Then
                Response.Write("invalid ERP ID.") : Response.End()
            End If
        End If
        ORG = DTQM.org
        Dim RURL As String = HttpContext.Current.Server.UrlEncode(String.Format("/Order/Quote2Cart.aspx?UID={0}", QuoteID)) ' Request.Url.AbsoluteUri 
        '20120801 TC: If user is not using eQuotation production site, then direct user to MyAdvantech testing site when flipping quote to order
        Dim strMyAdvantechUrl As String = "http://my.advantech.com"

        'If COMM.Util.IsTesting() Then strMyAdvantechUrl += ":4002"
        If COMM.Util.IsTesting() Then strMyAdvantechUrl = "http://172.20.1.30:4002"
        'If COMM.Util.IsTesting() Then strMyAdvantechUrl = "http://localhost:4002"
        'Response.Redirect(String.Format(strMyAdvantechUrl + "/ORDER/SSOENTER.ASPX?ID={0}&USER={1}&COMPANY={2}&RURL={3}&ORG={4}", TID, USER, HttpUtility.UrlEncode(COMPANY), RURL, ORG))
        Response.Redirect(String.Format(strMyAdvantechUrl + "/ORDER/Quote2CartV3.ASPX?UID={0}&USER={1}&COMPANY={2}&ORG={3}", QuoteID, USER, HttpUtility.UrlEncode(COMPANY), ORG))
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        initGV(Request("UID"))
        Me.gv1.DataBind()
    End Sub

    Protected Sub gv1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
        If PhaseOutPartNo IsNot Nothing AndAlso PhaseOutPartNo.Count > 0 Then
            trw.Visible = True ' tdedit.Visible = True
            Dim str As String = "The status of below Item(s) "

            str += " is(are) phase out or invalid.<br/> Please """
            str += String.Format(" <a href=""QuotationMaster.aspx?UID={0}"">click here</a>""", Request("UID"))
            str += "  to edit quotation."
            str += " <ul> "
            For Each PartNo As String In PhaseOutPartNo
                str += "<li> <font color=""red"">" + PartNo + " </font></li>"
            Next
            str += " </ul> "
            labWarn.Text = str
        End If
    End Sub

    Protected Sub BTedit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect(String.Format("~/Quote/QuotationMaster.aspx?UID={0}", Request("UID")))
    End Sub
    Dim DTList As IBUS.iCartList = Nothing

    Private Sub gv1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv1.DataBinding
        ' Dim DTList As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org).GetListAll(COMM.Fixer.eDocType.EQ)
    End Sub
End Class