Public Class PI
    Inherits PageBase
    Private _DetailRef As IBUS.iCart(Of IBUS.iCartLine) = Nothing
    Public ReadOnly Property myOrderDetail As IBUS.iCart(Of IBUS.iCartLine)
        Get
            If IsNothing(_DetailRef) Then
                _DetailRef = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Me.MasterRef.org)
            End If
            Return _DetailRef
        End Get
    End Property

    Private _Company As IBUS.iCustomer = Nothing
    Public ReadOnly Property myCompany As IBUS.iCustomer
        Get
            If IsNothing(_Company) Then
                _Company = Pivot.NewObjCustomer.getByErpIdOrg(Me.MasterRef.AccErpId, Me.MasterRef.org)
            End If
            Return _Company
        End Get
    End Property
    Public Function crs() As String
        Return Util.getCurrSign(MasterRef.currency)
    End Function
    Dim isANA As Boolean = False
    Private _currencySign As String = "", _OrderId As String = "", _IsInternalUserMode As Boolean = True
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.soldtoshiptoUC.QM = MasterRef
        Me.Orderinfo1.QM = MasterRef
        If Not IsNothing(UID) AndAlso UID <> "" Then
            _OrderId = UID

            If Role.IsUsaUser Then
                isANA = True
            End If
            If MasterRef.org = "US01" Then
                Me.trTax.Visible = True
            End If
            initInterface()
        End If
    End Sub
    Sub initInterface()

        Dim dtDetail As IBUS.iCartList = myOrderDetail.GetListAll(COMM.Fixer.eDocType.ORDER)
        'If Not IsNothing(MasterRef) And Not IsNothing(dtDetail) Then

        'Sold to and ship to information


        'Order information


        '    Dim soldTo As String = dtMaster.Rows(0).Item("SOLDTO_ID")
        '    Dim shipTo As String = dtMaster.Rows(0).Item("SHIPTO_ID")
        '    Dim dtSoldTo As DataTable = myCompany.GetDT(String.Format("company_id='{0}'", soldTo), "")
        '    Dim dtshipTo As DataTable = myCompany.GetDT(String.Format("company_id='{0}'", shipTo), "")
        '    If dtSoldTo.Rows.Count > 0 And dtshipTo.Rows.Count > 0 Then
        '        Me.lbSoldName.Text = dtSoldTo.Rows(0).Item("company_name") & "(" & dtSoldTo.Rows(0).Item("company_id") & ")"
        '        Me.lbSoldAtt.Text = dtMaster.Rows(0).Item("Attention")
        '        Me.lbSoldAddr.Text = dtSoldTo.Rows(0).Item("Address")
        '        Me.lbSoldTel.Text = dtSoldTo.Rows(0).Item("tel_no")
        '        Me.lbSoldFax.Text = dtSoldTo.Rows(0).Item("fax_no")

        '        Me.lbShipName.Text = dtshipTo.Rows(0).Item("company_name") & "(" & dtshipTo.Rows(0).Item("company_id") & ")"
        '        Me.lbShipAtt.Text = dtMaster.Rows(0).Item("customer_Attention")
        '        Me.lbShipAddr.Text = dtshipTo.Rows(0).Item("Address")
        '        Me.lbShipTel.Text = dtshipTo.Rows(0).Item("tel_no")
        '        Me.lbShipFax.Text = dtshipTo.Rows(0).Item("fax_no")
        '    End If
        '    Me.lbPO.Text = dtMaster.Rows(0).Item("PO_NO")
        '    Dim SONO As String = ""
        '    If dtMaster.Rows(0).Item("ORDER_STATUS") <> "" Then
        '        SONO = dtMaster.Rows(0).Item("Order_ID")
        '    End If
        '    Me.lbSO.Text = SONO
        '    Me.lbOrderDate.Text = CDate(dtMaster.Rows(0).Item("Order_date")).ToString("yyyy/MM/dd")
        '    Me.lbPayTerm.Text = ""
        '    Me.lbReqdate.Text = CDate(dtMaster.Rows(0).Item("Required_date")).ToString("yyyy/MM/dd")
        '    Me.lbIncoterm.Text = dtMaster.Rows(0).Item("INCOTERM")
        '    Me.lbPlacedBy.Text = dtMaster.Rows(0).Item("CREATED_BY")
        '    Me.lbIncotermText.Text = dtMaster.Rows(0).Item("INCOTERM_TEXT")
        '    Me.lbFreight.Text = dtMaster.Rows(0).Item("FREIGHT")
        '    If Double.TryParse(Me.lbFreight.Text, 0) AndAlso CDbl(Me.lbFreight.Text) = 0 Then Me.lbFreight.Text = "TBD"
        '    Me.lbChannel.Text = ""
        '    Me.lbisPartial.Text = dtMaster.Rows(0).Item("PARTIAL_FLAG")
        '    Me.lbShipCond.Text = Glob.shipCode2Txt(dtMaster.Rows(0).Item("SHIP_CONDITION"))
        '    Me.lbOrderNote.Text = dtMaster.Rows(0).Item("ORDER_NOTE")
        '    Me.lbSalesNote.Text = dtMaster.Rows(0).Item("SALES_NOTE")
        '    Me.lbOPNote.Text = dtMaster.Rows(0).Item("OP_NOTE")
        '    'Me.lbPJNote.Text = dtMaster.Rows(0).Item("prj_Note")
        'End If
        Me.gv1.DataSource = dtDetail
        Me.gv1.DataBind()
        'Me.trEUOPN.Visible = False

    End Sub

    Public Function getDescForPN(ByVal PN As String, ByVal Description As String) As String
        If Not String.IsNullOrEmpty(Description.ToString.Trim) Then
            Return Description
        End If
        Dim DTSAPPRODUCT As IBUS.iProd = Pivot.FactProd.getProdByPartNo(PN, MasterRef.org)
        If Not IsNothing(DTSAPPRODUCT) Then
            Return DTSAPPRODUCT.partDesc
        End If
        Return ""
    End Function

    Protected Sub gv1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ListP As IBUS.iCartList = myOrderDetail.GetListAll(COMM.Fixer.eDocType.ORDER)
        Dim total As Decimal = ListP.getTotalAmount
        Dim freight As Decimal = 0
        freight = getFreight()
        If freight > 0 Then
            Me.trFreight.Visible = True
            Me.lbFt.Text = freight
            'Me.lbFreight.Text = freight
        End If
        Dim taxA As Decimal = 0
        Dim taxR As Decimal = 0
        If MasterRef.org = "US01" AndAlso Role.IsInternalUser Then
            taxA = ListP.GetTaxableAmount(getShipTo())
            taxR = getTax()
        End If
        Me.lbtax.Text = IIf(taxR = 0, "N\A", FormatNumber(taxR, 4) * 100 & "%")
        Me.lbTotal.Text = FormatNumber(total + freight + (taxA * taxR), 2)
    End Sub
    Function getShipTo() As String
        Dim P As List(Of IBUS.iPartnerLine) = Pivot.NewObjPartner.GetListAll(MasterRef.Key, COMM.Fixer.eDocType.ORDER)
        Dim strShiptoId As String = ""
        If IsNothing(P) Then
            strShiptoId = MasterRef.AccErpId
        Else
            strShiptoId = CType(P.Where(Function(x) x.TYPE = "S").FirstOrDefault, IBUS.iPartnerLine).ERPID
        End If
        Return strShiptoId
    End Function
    Function getTax() As String
        Dim taxr As Decimal = 0
        If Not IsNothing(MasterRef) Then
            If MasterRef.isExempt = 0 Then
                Dim _txtTempZipCode As String = SAPDAL.SAPDAL.getUSZipcodeByShipToID(getShipTo())
                If Not String.IsNullOrEmpty(_txtTempZipCode) Then
                    taxr = SAPDAL.SAPDAL.getSalesTaxByZIP(_txtTempZipCode)
                End If
            End If
        End If
        Return taxr
    End Function
    Protected Function getFreight() As Decimal
        Dim v As Decimal = 0
        Dim myFT As IBUS.iCond = Pivot.NewObjCond
        Dim DT As List(Of IBUS.iCondLine) = myFT.GetListAll(UID)
        If Not IsNothing(DT) Then
            For Each X As IBUS.iCondLine In DT
                If X.Type = "ZHDA" Then
                    v = v - 0
                Else
                    v = v + X.Value
                End If
            Next
        End If
        Return v
    End Function

    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim DBITEM As IBUS.iCartLine = CType(e.Row.DataItem, IBUS.iCartLine)
            If DBITEM.ewFlag.Value = 99 Then
                CType(e.Row.FindControl("lbew"), Label).Text = "36"
            End If
            If DBITEM.ewFlag.Value = 999 Then
                CType(e.Row.FindControl("lbew"), Label).Text = "3"
            End If
            Dim dueDate As String = Now.Date
            dueDate = IIf(CDate(DBITEM.dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat) = CDate("1900/01/01").ToString(Pivot.CurrentProfile.DatePresentationFormat), "TBD", IIf(isANA, CDate(DBITEM.dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat), CDate(DBITEM.dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat)))
            If Not DBITEM.partNo.Value.ToString.StartsWith("AGS-") And _
                DBITEM.itemType.Value = COMM.Fixer.eItemType.Others And DBITEM.satisfyflag.Value = 0 And dueDate <> "TBD" Then
                e.Row.Cells(5).Text = "<font color='#FF0000'>For Reference Only</font>" & "<br/>" & dueDate
            End If

            If DBITEM.itemType.Value = COMM.Fixer.eItemType.Parent And DBITEM.satisfyflag.Value = 0 Then
                e.Row.Cells(5).Text = "<font color='#FF0000'>For Reference Only</font>" & "<br/>" & dueDate
            End If

            If DBITEM.parentLineNo.Value > COMM.Fixer.StartLine Then
                e.Row.Cells(5).Text = ""
                e.Row.Cells(6).Text = ""
                e.Row.Cells(10).Text = ""
                e.Row.Cells(11).Text = ""
            End If
            If DBITEM.itemType.Value = COMM.Fixer.eItemType.Parent Then
                Dim Price As Decimal = 0
                Dim Amt As Decimal = 0
                Dim subL As IBUS.iCartList = CType(CType(sender, GridView).DataSource, IBUS.iCartList)
                subL = subL.Merge2NewCartList(subL.Where(Function(x) x.parentLineNo.Value = DBITEM.parentLineNo.Value))
                Amt = subL.getTotalAmount()
                If DBITEM.Qty.Value > 0 AndAlso Amt > 0 Then
                    Price = Amt / DBITEM.Qty.Value
                End If
                e.Row.Cells(10).Text = MasterRef.currency & FormatNumber(Price, 2)
                e.Row.Cells(11).Text = MasterRef.currency & FormatNumber(Amt, 2)
            End If
        End If
        If MasterRef.org.ToString.Trim.Equals("US01", StringComparison.OrdinalIgnoreCase) Then
            If e.Row.RowType = DataControlRowType.Header Then
                CType(e.Row.FindControl("lbHDueDate"), Label).Text = "Available Date"
                CType(e.Row.FindControl("lbHReqDate"), Label).Text = "Req deliv date"
            End If
            If e.Row.RowType <> DataControlRowType.EmptyDataRow Then
                e.Row.Cells(7).Visible = False
            End If
        End If
    End Sub

End Class