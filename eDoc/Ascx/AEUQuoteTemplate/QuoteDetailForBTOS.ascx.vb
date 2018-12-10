Public Class QuoteDetailForBTOS
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

            myOrderDetail = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org)
            Dim dtDetail As IBUS.iCartList = myOrderDetail.GetListAll(COMM.Fixer.eDocType.EQ)
            Dim Btosline As IBUS.iCartLine = dtDetail.Where(Function(p) p.itemType.Value = COMM.Fixer.eItemType.Parent).FirstOrDefault()
            If Btosline IsNot Nothing Then
                LitBtosPartNO.Text = Btosline.partNo.Value
                LitQTY.Text = Btosline.Qty.Value
                LitUnitPrice.Text = FormatNumber(dtDetail.getTotalAmount() / Btosline.Qty.Value, 2)
                LitPrice.Text = FormatNumber(dtDetail.getTotalAmount(), 2)
            End If
            Me.gv1.DataSource = dtDetail.Where(Function(p) p.itemType.Value <> COMM.Fixer.eItemType.Parent)
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
        Dim total As Decimal = ListP.getTotalAmount
        Dim freight As Decimal = 0, Tax As Decimal = 0
        freight = CDec(MasterRef.freight) 'getFreight()
        If freight > 0 Then
            Me.trFreight.Visible = True
            Me.lbFt.Text = FormatNumber(freight, 2)
        Else
            trFreight.Visible = False
        End If
        Tax = CDec(MasterRef.tax)
        If Tax > 0 Then
            LitTax.Text = FormatNumber(Tax * 100, 2)
            Me.lbtax.Text = FormatNumber(Tax * (total + freight), 2)
        Else
            trTax.Visible = False
        End If
        If freight <= 0 AndAlso Tax <= 0 Then
            trline1.Visible = False
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
        Select Case MasterRef.ISVIRPARTONLY
            Case 0
                e.Row.Cells(3).Visible = False
            Case 1
                e.Row.Cells(2).Visible = False
        End Select
    End Sub
End Class