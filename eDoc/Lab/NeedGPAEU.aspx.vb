Public Class NeedGPAEU
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim _dt As DataTable = Nothing
        Dim _str As New StringBuilder

        _str.AppendLine(" Select a.QuoteID,b.quoteNo,b.createdBy,CONVERT(char(10), b.createdDate,126) as CreatedDate,c.so_no,c.order_date,b.quoteToErpId,b.quotetoName,b.currency,b.org,b.quotetoRowid ")
        _str.AppendLine(" ,b.ogroup,b.office ")
        '_str.AppendLine(" ,c.line_No,c.partNo,c.listPrice,c.unitPrice,c.newUnitPrice,c.itp,c.newItp ")
        _str.AppendLine(" From AEUMissGPControlQuoteID a inner join QuotationMaster b on a.QuoteID=b.quoteId ")
        _str.AppendLine(" left join quote_to_order_log c on a.QuoteID=c.quoteId ")
        _str.AppendLine(" Where b.docstatus=1 ")
        '_str.AppendLine(" order by a.QuoteID,c.line_No ")

        _dt = tbOPBase.dbGetDataTable("EQ", _str.ToString)
        _dt.Columns.Add("ISNeedGP")
        _dt.Columns("ISNeedGP").ReadOnly = False
        _dt.Columns.Add("Approver")
        _dt.Columns("Approver").ReadOnly = False
        _dt.Columns.Add("TotalListPrice")
        _dt.Columns("TotalListPrice").ReadOnly = False
        _dt.Columns.Add("TotalUnitPrice")
        _dt.Columns("TotalUnitPrice").ReadOnly = False
        _dt.Columns.Add("TotalITP")
        _dt.Columns("TotalITP").ReadOnly = False
        _dt.Columns.Add("Margin")
        _dt.Columns("Margin").ReadOnly = False

        Dim itp As Decimal = 0
        Dim partNo As String = String.Empty
        Dim Currency As String = String.Empty
        Dim COMPANY_ID As String = String.Empty
        Dim Quoteid As String = String.Empty
        Dim org As String = String.Empty
        Dim line_No As Integer = 0
        Dim _totalITP As Decimal = 0, _TotalUintPrice As Decimal = 0, _TotalListPrice As Decimal = 0, _marginPT As Boolean = True
        Dim myQD As New EQDSTableAdapters.QuotationDetailTableAdapter
        Dim QDDT As New EQDS.QuotationDetailDataTable
        Dim detail As New List(Of struct_GP_Detail)



        For Each _row As DataRow In _dt.Rows
            _totalITP = 0 : _TotalUintPrice = 0 : _TotalListPrice = 0
            Quoteid = _row.Item("QuoteID")
            'line_No = _row.Item("line_No")
            'partNo = _row.Item("partNo")
            Currency = _row.Item("Currency")
            COMPANY_ID = _row.Item("quoteToErpId")
            org = _row.Item("org")

            detail.Clear()
            QDDT = myQD.GetQuoteDetailById(Quoteid)
            For Each x As EQDS.QuotationDetailRow In QDDT
                Dim detailLine As New struct_GP_Detail
                detailLine.lineNo = x.line_No : detailLine.PartNo = x.partNo : detailLine.Price = x.newUnitPrice
                detailLine.QTY = x.qty : detailLine.Itp = x.newItp : detail.Add(detailLine)


            Next

            Dim L As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, Quoteid, org).GetListAll(COMM.Fixer.eDocType.EQ)
            'If IsNothing(L) Then
            '    _row.Item("TotalAmount") = "0.00"
            'Else
            '    _row.Item("TotalAmount") = L.getTotalAmount

            'End If


            Dim MSTD As Decimal = GPControl.getMarginWithOutAGS(detail)
            Dim MPTD As Decimal = GPControl.getMarginPTD(detail)
            Dim MLST As Decimal = -99999
            If MSTD <> -99999 Then
                MLST = MSTD
                _marginPT = False
            Else
                MLST = MPTD
                _marginPT = True
            End If

            For Each x As EQDS.QuotationDetailRow In QDDT
                _totalITP += x.itp * x.qty
                _TotalUintPrice += x.newUnitPrice * x.qty
                _TotalListPrice += x.listPrice * x.qty
                'If _marginPT Then
                '    If Business.IsPTD(x.partNo) Then
                '        _totalITP += x.itp * x.qty
                '        _TotalUintPrice += x.newUnitPrice * x.qty
                '    End If

                'Else
                '    If (Not Business.IsPTD(x.partNo)) _
                '        And SAPDAL.CommonLogic.NoStandardSensitiveITP(x.partNo) = False Then
                '        _TotalUintPrice += x.newUnitPrice * x.qty
                '        _totalITP += x.itp * x.qty
                '    End If
                'End If
            Next

            _row.Item("TotalITP") = _totalITP
            _row.Item("TotalUnitPrice") = _TotalUintPrice
            _row.Item("TotalListPrice") = _TotalListPrice


            If MLST = -99999 Then MLST = 0
            _row.Item("Margin") = FormatNumber(MLST * 100, 2)
            '_row.Item("GP") = Business.getMargin(Quoteid)

            If Business.isNeedGPControl(Quoteid, org) And _
               (GPControl.getLevel(_row.Item("quotetoRowid"), COMPANY_ID, detail, SAPDAL.SAPDAL.itpType.EU, _row.Item("office"), _row.Item("ogroup")) > 0 Or (org = "EU10" And GPControl.isDLGR(detail))) Then
                _row.Item("ISNeedGP") = "Yes"

                Dim dt As New DataTable
                GPControl.getLevelandAppoverList(_row.Item("quotetoRowid"), COMPANY_ID, dt, SAPDAL.SAPDAL.itpType.EU, _row.Item("office"), _row.Item("ogroup"))
                Dim Level As Integer = GPControl.getLevel(_row.Item("quotetoRowid"), COMPANY_ID, detail, SAPDAL.SAPDAL.itpType.EU, _row.Item("office"), _row.Item("ogroup"))

                _row.Item("Approver") = dt.Rows(0).Item("Approver")

            Else
                _row.Item("ISNeedGP") = "No"

            End If



            'itp = SAPDAL.SAPDAL.getItp("EU10", partNo, Currency, COMPANY_ID, SAPDAL.SAPDAL.itpType.EU)

            '_row.Item("itp") = itp
            '_row.Item("newItp") = itp

            '_str.Clear()
            '_str.AppendLine(" Update QuotationDetail Set itp=" & itp & ",Newitp=" & itp)
            '_str.AppendLine(" Where QuoteID='" & Quoteid & "'")
            '_str.AppendLine(" And line_No=" & line_No)

            'tbOPBase.dbExecuteNoQuery("EQ", _str.ToString)

        Next

        Me.GridView1.DataSource = _dt
        Me.GridView1.DataBind()


    End Sub

End Class