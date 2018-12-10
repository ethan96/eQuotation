Imports Advantech.Myadvantech.DataAccess

Public Class piHtml
    Inherits System.Web.UI.Page
    Private _UID As String = ""
    Public ReadOnly Property UID As String
        Get
            If Not IsNothing(Request("UID")) Then
                _UID = Request("UID")
            End If
            Return _UID
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

    Dim myQD As New quotationDetail("EQ", "quotationDetail")
    Dim _IsBTosQuotation As Boolean = False
    Public currencySign As String = ""
    Protected Sub initInterFace(ByVal UID As String)
        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)

        'Frank 2012/10/23:Get and write the real time part inventory and due date for non-ana quotation
        Dim aptQD As New EQDSTableAdapters.QuotationDetailTableAdapter

        Business.RefreshQuotationInventory(UID)
        updatedueDate()


        If Not IsNothing(dt) Then
            With dt
                Me.lbCustomId.Text = .CustomId
                Me.lbQuoteID.Text = .quoteNo
                Me.lbCreatedBy.Text = .CreatedBy
                Me.lbQuoteDate.Text = .DocDate
                Me.lbPreparedBy.Text = .preparedBy
                Me.lbQuoteToName.Text = .AccName
                If .AccErpId <> "" Then
                    Me.lbQuoteToId.Text = "(" & .AccErpId & ")"
                End If
                Me.lbOffice.Text = .AccOfficeName
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
                Me.lbAddressInfo.Text = Business.getRUBAddress(.AccOfficeName, .AccGroupName, .DocReg)
                If .delDateFlag = 1 Then
                    Me.lbDeliveryDate.Text = "To Be Verified"
                End If
                If Not IsNothing(Request("ROLE")) AndAlso Request("ROLE") = 1 Then
                    Me.trRelatedInfo.Visible = False

                End If
            End With

            Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(UID)
            If _QMaster Is Nothing Then Exit Sub
            Dim _ME As QuotationExtension = _QMaster.QuoteExtensionX
            Dim SB As New StringBuilder
            If _ME.ApprovalFlowType = eQApprovalFlowType.Normal Then
                trGP.Visible = False
                trRelatedInfo.Visible = False
            End If
            SB.AppendFormat("This quote must be approved through Approval Flow due to below issues:")
            SB.AppendFormat(" <ul>")
            If _ME.ApprovalFlowType = eQApprovalFlowType.GP Then
                SB.AppendFormat("<li>Below GP</li>")
            End If
            If _ME.ApprovalFlowType = eQApprovalFlowType.GPandExpiration Then
                SB.AppendFormat("<li>Below GP</li>")
                SB.AppendFormat("<li>Expiration date is more than 30 days</li>")
            End If
            If _ME.ApprovalFlowType = eQApprovalFlowType.ThirtyDaysExpiration Then
                SB.AppendFormat("<li>Expiration date is more than 30 days</li>")
            End If
            SB.AppendFormat("</ul>")
            LabelGP.Text = SB.ToString()
        End If
    End Sub

    Sub updatedueDate()
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("eq", String.Format("select isnull(max(duedate),getdate()) as duedate from quotationDetail where quoteid='{0}'", Request("UID")))
        If dt.Rows.Count > 0 Then
            Dim MDue As String = ""
            MDue = dt.Rows(0).Item("duedate")
            Pivot.NewObjDocHeader.Update(Request("UID"), String.Format("deliveryDate='{0}'", MDue), COMM.Fixer.eDocType.EQ)
        End If
    End Sub


    Protected Sub initGV(ByVal UID As String)
        'Dim DT As DataTable = myQD.GetDT(String.Format("quoteId='{0}'", UID), "line_No")
        Dim _Quotelist As List(Of QuoteItem) = MyQuoteX.GetQuoteList(UID)
        Me.gv1.DataSource = _Quotelist
        Me.gv1.DataBind()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            initInterFace(Request("UID"))
            initGV(Request("UID"))
        End If
    End Sub

    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        ' Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
        Dim dt As Quote_Master = MyQuoteX.GetQuoteMaster(UID)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim _QuoteItem As QuoteItem = CType(e.Row.DataItem, QuoteItem)
            Dim line_no As Integer = gv1.DataKeys(e.Row.RowIndex).Value
            Dim part_no As String = e.Row.Cells(2).Text.Trim
            Dim ListPice As Decimal = CDbl(CType(e.Row.FindControl("lbListPrice"), Label).Text)
            Dim UnitPrice As Decimal = CDbl(CType(e.Row.FindControl("lbUnitPrice"), Label).Text)
            Dim qty As Decimal = CInt(CType(e.Row.FindControl("lbGVQty"), Label).Text)
            Dim itp As Decimal = _QuoteItem.newItp
            Dim Discount As Decimal = 0.0
            Dim SubTotal As Decimal = 0.0
            Dim ewPrice As Decimal = 0.0
            If _QuoteItem.ewFlag = 99 Then
                CType(e.Row.FindControl("lbew"), Label).Text = "36"
            End If
            If _QuoteItem.ewFlag = 999 Then
                CType(e.Row.FindControl("lbew"), Label).Text = "3"
            End If
            ewPrice = FormatNumber(Business.getRateByEWItem(Business.getEWItemByMonth(CInt(_QuoteItem.ewFlag), dt.DocReg), dt.DocReg) * UnitPrice, 2)

            CType(e.Row.FindControl("gv_lbEW"), Label).Text = ewPrice
            If ListPice = 0 Then
                e.Row.Cells(7).Text = "TBD"
                e.Row.Cells(9).Text = "TBD"
            Else
                Discount = FormatNumber((ListPice - UnitPrice) / ListPice, 2)
                If ListPice < UnitPrice Then
                    Discount = 0
                End If
                e.Row.Cells(9).Text = Discount * 100 & "%"
            End If
            SubTotal = FormatNumber(qty * UnitPrice, 2)
            CType(e.Row.FindControl("lbSubTotal"), Label).Text = SubTotal
            If UnitPrice <> 0 Then
                CType(e.Row.FindControl("lbMargin"), Label).Text = FormatNumber((UnitPrice - itp) * 100 / UnitPrice, 2) & "%"
            End If
            If _QuoteItem.ItemTypeX = QuoteItemType.BtosParent Then
                e.Row.Cells(1).Text = ""
                e.Row.Cells(3).Text = ""
                e.Row.Cells(4).Text = ""
                e.Row.Cells(5).Text = ""
                e.Row.Cells(6).Text = ""

                'e.Row.Cells(8).Text = ""
                e.Row.Cells(9).Text = ""
                e.Row.Cells(11).Text = ""
                'e.Row.Cells(14).Text = ""
                CType(e.Row.FindControl("lbMargin"), Label).Text = Business.getMargin(Request("UID")) & "%"
                Dim TAmt As Decimal = _QuoteItem.ChildSubUnitPriceWithWarrantX
                Dim TPrice As Decimal = 0
                Dim TLPrice As Decimal = 0
                'If Not IsNothing(DTList) Then
                '    Dim subL As IBUS.iCartList = DTList.Merge2NewCartList(DTList.Where(Function(x) x.parentLineNo.Value = DBITEM.Item("Line_No").ToString))
                '    If Not IsNothing(subL) Then
                '        TAmt = subL.getTotalAmount
                '        If TAmt > 0 AndAlso CInt(QuoteItem.qty) > 0 Then
                '            TPrice = TAmt / QuoteItem.qty
                '        End If
                '    End If
                '    TLPrice = subL.getTotalListAmount() / QuoteItem.qty
                'End If

                e.Row.Cells(7).Text = _QuoteItem.ChildSubListPriceX

                CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(TAmt, 2)
                CType(e.Row.FindControl("lbUnitPrice"), Label).Text = FormatNumber(TAmt / _QuoteItem.qty, 2)
            End If
            If _QuoteItem.ItemTypeX = QuoteItemType.BtosPart Or _QuoteItem.IsEWpartnoX Then
                If dt.isLumpSumOnly = 1 And (Not IsNothing(Request("ROLE")) AndAlso Request("ROLE") = 1) Then
                    Try
                        CType(e.Row.FindControl("lbListPriceSign"), Label).Visible = False
                        CType(e.Row.FindControl("lbListPrice"), Label).Visible = False
                        CType(e.Row.FindControl("lbUnitPriceSign"), Label).Visible = False
                        CType(e.Row.FindControl("lbUnitPrice"), Label).Visible = False
                        CType(e.Row.FindControl("lbSubTotalSign"), Label).Visible = False
                        CType(e.Row.FindControl("lbSubTotal"), Label).Visible = False
                    Catch ex As Exception

                    End Try
                End If
            End If

            Dim ORG As String = dt.org
            'If QMDT.Rows(0).Item("quoteToErpId") <> "" Then
            '    ORG = Business.getOrgByErpId(QMDT.Rows(0).Item("quoteToErpId"))
            'End If
            Dim strStatusCode As String = "", strStatusDesc As String = ""
            If Not _QuoteItem.ItemTypeX = QuoteItemType.BtosParent And (Not _QuoteItem.IsEWpartnoX) And _
                Business.isInvalidPhaseOutV2(part_no, ORG, strStatusCode, strStatusDesc, 0) Then
                CType(e.Row.FindControl("lbdescription"), Label).Text = "(Phase Out)"
                CType(e.Row.FindControl("lbdescription"), Label).ForeColor = Drawing.Color.Red
            End If

            'Frank 2015/10/07 Special ITP can be appended after SPR no
            If String.IsNullOrEmpty(CType(e.Row.FindControl("lbSPRNO"), Label).Text) Then
                CType(e.Row.FindControl("lbSpecialITPSign"), Label).Visible = False
                CType(e.Row.FindControl("lbSpecialITP"), Label).Visible = False
            End If


        End If
        If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Then
            If myQD.isBtoOrder(Request("UID")) = 0 Then
                e.Row.Cells(1).Visible = False
            End If
            'If myQD.isBtoOrder(Request("UID")) = 1 Then
            e.Row.Cells(4).Visible = False
            ' End If
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False

            If dt.isShowListPrice = 0 Then
                e.Row.Cells(7).Visible = False
            End If
            If dt.isShowDiscount = 0 Then
                e.Row.Cells(9).Visible = False
            End If
            If dt.isShowDueDate = 0 Then
                e.Row.Cells(12).Visible = False
            End If
            If Not IsNothing(Request("ROLE")) AndAlso Request("ROLE") = 1 Then
                'ITP(FOB AESC) :   Margin
                e.Row.Cells(14).Visible = False : e.Row.Cells(15).Visible = False
                'SPRNO         :   Special ITP
                e.Row.Cells(16).Visible = False : e.Row.Cells(17).Visible = False
            End If
        End If

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim dtM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Request("UID"), COMM.Fixer.eDocType.EQ)
        If Not IsNothing(dtM) Then
            currencySign = Util.getCurrSign(dtM.currency)
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.lbtotal.Text = FormatNumber(Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org).GetListAll(COMM.Fixer.eDocType.EQ).getTotalAmount, 2)
        Me.lbTotalMargin.Text = Business.getMargin(Request("UID"))
        If Not IsNothing(Request("ROLE")) AndAlso Request("ROLE") = 1 Then
            Me.spMargin.Visible = False
            Me.spMargin1.Visible = False
        End If
    End Sub

    Dim DTList As IBUS.iCartList = Nothing
    Private Sub gv1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv1.DataBinding
        'Dim DTList As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org).GetListAll(COMM.Fixer.eDocType.EQ)
    End Sub
End Class