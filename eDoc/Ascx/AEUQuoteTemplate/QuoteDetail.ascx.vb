Imports System.Drawing

Public Class QuoteDetail1
    Inherits System.Web.UI.UserControl
    Private _UID As String = String.Empty
    Public Property UID As String
        Get
            Return _UID
        End Get
        Set(ByVal value As String)
            _UID = value
        End Set
    End Property

    Dim MasterRef As IBUS.iDocHeaderLine = Nothing
    Dim myOrderDetail As IBUS.iCart(Of IBUS.iCartLine) = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MasterRef = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)

            If MasterRef.isShowListPrice = 1 Then
                Me.gv1.Columns(10).Visible = True
            Else
                Me.gv1.Columns(10).Visible = False
            End If
            If MasterRef.isShowDueDate = 1 Then
                Me.gv1.Columns(5).Visible = True
            Else
                Me.gv1.Columns(5).Visible = False
            End If
            If MasterRef.isShowDiscount = 1 Then
                Me.gv1.Columns(12).Visible = True
            Else
                Me.gv1.Columns(12).Visible = False
            End If

            myOrderDetail = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org)
            Dim dtDetail As IBUS.iCartList = myOrderDetail.GetListAll(COMM.Fixer.eDocType.EQ)
            Me.gv1.DataSource = dtDetail
            Me.gv1.DataBind()
        End If
    End Sub
    Public Function crs() As String
        Return MasterRef.currency ' Util.getCurrSign(MasterRef.currency)
    End Function
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
        Dim ListP As IBUS.iCartList = myOrderDetail.GetListAll(COMM.Fixer.eDocType.EQ)
        If ListP Is Nothing Then Exit Sub
        trsubtotal.Visible = False
        trFreight.Visible = False
        trTax.Visible = False
        trbr.Visible = False
        trtotal.Visible = True
        Dim _QMExtension As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
        If _QMExtension IsNot Nothing AndAlso _QMExtension.IsShowTotal = 0 Then
            trtotal.Visible = False
            Exit Sub
        Else
            trbr.Visible = True
        End If
        Dim total As Decimal = ListP.getTotalAmount
        Dim freight As Decimal = 0, Tax As Decimal = 0
        freight = CDec(MasterRef.freight) 'getFreight()
        If freight > 0 Then
            Me.trFreight.Visible = True
            Me.lbFt.Text = FormatNumber(freight, 2)
            trFreight.Visible = True
        End If
        Tax = CDec(MasterRef.tax)
        If Tax > 0 Then
            LitTax.Text = FormatNumber(Tax * 100, 2)
            Me.lbtax.Text = FormatNumber(Tax * (total + freight), 2)
            trTax.Visible = True
        End If
        If freight > 0 OrElse Tax > 0 Then
            trsubtotal.Visible = True : trFreightDesc.Visible = True
        End If
        If freight = 0 AndAlso Tax = 0 Then
            trbr.Visible = False
        End If
        Me.lbTotal.Text = FormatNumber(total, 2)
        Me.lbTotal2.Text = FormatNumber(total + freight + (Tax * (total + freight)), 2)
    End Sub
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
            dueDate = IIf(CDate(DBITEM.dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat) = CDate("1900/01/01").ToString(Pivot.CurrentProfile.DatePresentationFormat), "TBD", IIf(False, CDate(DBITEM.dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat), CDate(DBITEM.dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat)))
            If Not DBITEM.partNo.Value.ToString.StartsWith("AGS-") And _
                DBITEM.itemType.Value = COMM.Fixer.eItemType.Others And DBITEM.satisfyflag.Value = 0 And dueDate <> "TBD" Then
                e.Row.Cells(5).Text = "<font color='#FF0000'>For Reference Only</font>" & "<br/>" & dueDate
            End If

            If DBITEM.itemType.Value = COMM.Fixer.eItemType.Parent And DBITEM.satisfyflag.Value = 0 Then
                e.Row.Cells(5).Text = "<font color='#FF0000'>For Reference Only</font>" & "<br/>" & dueDate
            End If

            If DBITEM.parentLineNo.Value > COMM.Fixer.StartLine Then
                If MasterRef.isLumpSumOnly = 1 Then
                    'e.Row.Cells(5).Text = ""
                    e.Row.Cells(6).Text = ""
                    e.Row.Cells(10).Text = ""
                    e.Row.Cells(11).Text = ""
                    e.Row.Cells(12).Text = ""
                    e.Row.Cells(13).Text = ""
                End If
            End If
            Dim _BTOSLastLineNo As Integer = 0


            If DBITEM.itemType.Value = COMM.Fixer.eItemType.Parent Then


                'Frank(2013/12/30)
                Dim _SubTotal As Decimal = 0, _ListPriceSubTotalAmount As Decimal = 0

                Dim DTL As IBUS.iCartList = DTList.Merge2NewCartList(DTList.Where(Function(X) X.parentLineNo.Value = DBITEM.lineNo.Value))

                If Not IsNothing(DTL) AndAlso DTL.Count > 0 Then
                    _ListPriceSubTotalAmount = DTL.getListPriceTotalAmount()
                    _SubTotal = DTL.getTotalAmount()

                    e.Row.Cells(10).Text = FormatNumber(_ListPriceSubTotalAmount / DBITEM.Qty.Value, 2) & " " & MasterRef.currency
                    e.Row.Cells(11).Text = FormatNumber(_SubTotal / DBITEM.Qty.Value, 2) & " " & MasterRef.currency
                    If _ListPriceSubTotalAmount = 0 Then
                        e.Row.Cells(12).Text = ""
                    Else
                        e.Row.Cells(12).Text = FormatNumber(((_SubTotal - _ListPriceSubTotalAmount) / _ListPriceSubTotalAmount) * 100, 2) & "%"
                    End If
                    e.Row.Cells(13).Text = FormatNumber(_SubTotal, 2) & " " & MasterRef.currency
                End If
                '_BTOSLastLineNo = DTList.getBTOSLastItemLineNo(UID, DBITEM.lineNo.Value)

                e.Row.CssClass = "QDtext3"
            Else
                If DBITEM.listPrice.Value = 0 Then
                    e.Row.Cells(10).Text = "TBD"
                    e.Row.Cells(12).Text = ""
                Else
                    If MasterRef.isLumpSumOnly <> 1 Then
                        e.Row.Cells(12).Text = FormatNumber(((DBITEM.listPrice.Value - DBITEM.newunitPrice.Value) / DBITEM.listPrice.Value) * 100, 2) & "%"
                    End If
                End If

            End If



            If DBITEM.parentLineNo.Value > 1 Then
                Dim aaa As IEnumerable(Of IBUS.iCartLine) = DTList.Where(Function(X) X.parentLineNo.Value = DBITEM.parentLineNo.Value)
                _BTOSLastLineNo = aaa.Last.lineNo.Value
            End If


            If DBITEM.itemType.Value = COMM.Fixer.eItemType.Others And DBITEM.lineNo.Value < 100 Then
                e.Row.CssClass = "QDtext3 QDbr1"
            ElseIf DBITEM.itemType.Value = COMM.Fixer.eItemType.Others And DBITEM.lineNo.Value > 100 And DBITEM.lineNo.Value = _BTOSLastLineNo Then
                e.Row.CssClass = "QDbr1"
            End If


        End If



        'If MasterRef.org.ToString.Trim.Equals("US01", StringComparison.OrdinalIgnoreCase) Then
        '    If e.Row.RowType = DataControlRowType.Header Then
        '        CType(e.Row.FindControl("lbHDueDate"), Label).Text = "Available Date"
        '        CType(e.Row.FindControl("lbHReqDate"), Label).Text = "Req deliv date"
        '    End If
        '    If e.Row.RowType <> DataControlRowType.EmptyDataRow Then
        '        e.Row.Cells(7).Visible = False
        '    End If
        'End If

        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
            Select Case MasterRef.ISVIRPARTONLY
                Case 0
                    e.Row.Cells(3).Visible = False
                Case 1
                    e.Row.Cells(2).Visible = False
            End Select
        End If

    End Sub

    Dim DTList As IBUS.iCartList = Nothing

    Private Sub gv1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv1.DataBinding
        DTList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Pivot.CurrentProfile.getCurrOrg).GetListAll(COMM.Fixer.eDocType.EQ)
    End Sub


End Class