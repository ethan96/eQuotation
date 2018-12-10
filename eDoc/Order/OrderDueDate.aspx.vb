Public Class OrderDueDate
    Inherits PageBase
    Private _ISDIRSAP As String = ""
    Public ReadOnly Property ISDIRSAP As String
        Get
            If Not IsNothing(Request("ISDIRSAP")) Then
                _ISDIRSAP = Request("ISDIRSAP")
            End If
            Return _ISDIRSAP
        End Get
    End Property
    
    Private _DetailRef As IBUS.iCart(Of IBUS.iCartLine) = Nothing
    Public ReadOnly Property DetailRef As IBUS.iCart(Of IBUS.iCartLine)
        Get
            If IsNothing(_DetailRef) Then
                _DetailRef = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Me.MasterRef.org)
            End If
            Return _DetailRef
        End Get
    End Property

    Private _Company As IBUS.iCustomer = Nothing
    Public ReadOnly Property Company As IBUS.iCustomer
        Get
            If IsNothing(_Company) Then
                _Company = Pivot.NewObjCustomer
            End If
            Return _Company
        End Get
    End Property
    Private _CompanyLine As IBUS.iCustomerLine = Nothing
    Public ReadOnly Property CompanyLine As IBUS.iCustomerLine
        Get
            If IsNothing(_CompanyLine) Then
                _CompanyLine = Company.getByErpIdOrg(MasterRef.AccErpId, MasterRef.Org)
            End If
            Return _CompanyLine
        End Get
    End Property
    Public Function crs() As String
        Return Util.getCurrSign(MasterRef.currency)
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(MasterRef) Then
            If MasterRef.DocType <> COMM.Fixer.eDocType.ORDER Then
                Util.showMessage("Doc Type is invalid, Must be an Quotation.", "back")
            End If
            If MasterRef.qStatus <> COMM.Fixer.eDocStatus.ODRAFT Then
                Util.showMessage("Doc Status is invalid, Must be an Draft Order.", "back")
            End If


            If UID <> "" And Not IsPostBack Then
                CType(Me.ascxCalendar.FindControl("hCompany"), HiddenField).Value = MasterRef.AccErpId
                initInterface()
                '20111128 TC: Check if there is any item's status=O, and if yes, if the order qty<=ATP qty, if not, disable Confirm button and show warning message
                CheckStatusOItemATP()
                If ISDIRSAP = "1" AndAlso Me.btnConfirm.Enabled = True AndAlso Me.btnConfirm.Visible = True Then
                    Me.btnConfirm_Click(Me.btnConfirm, Nothing)
                End If
            End If
        End If
       
    End Sub

    Sub CheckStatusOItemATP()
        lbMsg.Text = ""
        Dim orderId As String = Trim(UID), OrgId As String = MasterRef.org
        Dim strSql As String = _
            " select a.partno, sum(a.QTY) as ORDER_QTY,  " + _
            " IsNull((select sum(z.ATP_QTY) from SAP_PRODUCT_ATP z where z.PART_NO=a.PARTNO and z.SALES_ORG='" + OrgId + "'),0) as ATP_QTY " + _
            " from eQuotation.DBO.QuotationDetail a inner join SAP_PRODUCT_ORG b on a.PARTNO=b.PART_NO  " + _
            " where a.QUOTEID='" + orderId + "' and b.ORG_ID='" + OrgId + "' and b.STATUS in ('O') " + _
            " group by a.PARTNO  "
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", strSql)
        If dt.Rows.Count > 0 Then
            Dim sbWarnMsg As New System.Text.StringBuilder
            For Each r As DataRow In dt.Rows
                Dim strTmpPN As String = r.Item("partno")
                Dim intTmpOrderQty As Integer = CInt(r.Item("ORDER_QTY")), intTmpATPQty As Integer = CInt(r.Item("ATP_QTY"))
                If intTmpOrderQty > 0 And intTmpOrderQty > intTmpATPQty Then
                    If btnConfirm.Enabled Then btnConfirm.Enabled = False
                    sbWarnMsg.AppendLine(String.Format("{0} is phased out inventory qty {1} is less than order qty {2}<br/>", _
                                                       strTmpPN, intTmpATPQty.ToString(), intTmpOrderQty.ToString()))
                End If
            Next
            If btnConfirm.Enabled = False Then lbMsg.Text = sbWarnMsg.ToString()
        End If
    End Sub

    Sub initInterface()
        If Not IsNothing(MasterRef) And Not IsNothing(DetailRef) Then
            litorderinfo.Text = UIUtil.GetAscxStr(MasterRef, 0) + UIUtil.GetAscxStr(MasterRef, 1)
        End If
        If IsNothing(DetailRef) Then
            Util.showMessage("There are no products in your shopping cart.")
            btnConfirm.Enabled = False : btnUpdate.Visible = False
            Exit Sub
        Else
            btnConfirm.Enabled = True : btnUpdate.Visible = True
        End If
        Me.gv1.DataSource = DetailRef.GetListAll(COMM.Fixer.eDocType.ORDER) : Me.gv1.DataBind()
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
        Dim L As IBUS.iCartList = DetailRef.GetListAll(COMM.Fixer.eDocType.ORDER)
        If IsNothing(L) Then
            Exit Sub
        End If
        Dim total As Decimal = L.getTotalAmount()
        Dim freight As Decimal = 0
        freight = getFreight()
        If freight > 0 Then
            Me.trFreight.Visible = True : Me.lbFt.Text = freight ': Me.lbFreight.Text = freight
        End If
        Me.lbTotal.Text = FormatNumber(total + freight, 2)
    End Sub
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


    Sub ReCalDue(ByVal order_id As String, ByVal line_no As String)
        Dim dt As IBUS.iCartLine = DetailRef.Item(line_no)
        If Not IsNothing(dt) Then
            Dim part_no As String = dt.partNo.Value, plant As String = dt.divPlant.Value
            Dim qty As String = dt.Qty.Value, req_date As String = dt.reqDate.Value, duedate As String = ""
            Dim inventory As Integer = 0, satisflag As Integer = 0, qtyCanbeConfirmed As Integer = 0
            SAPTools.getInventoryAndATPTable(part_no, plant, qty, duedate, 0, Nothing, req_date)
            DetailRef.UpdateByLine(String.Format("{0}='{1}'", DetailRef.Item1.dueDate.Name, duedate), line_no, COMM.Fixer.eDocType.ORDER)
        End If
    End Sub



    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each gvr As GridViewRow In gv1.Rows
            Dim reqTB As TextBox = CType(gvr.FindControl("txtreqdate"), TextBox)
            If Date.TryParse(reqTB.Text, Now) = False Then Exit Sub
            Dim req_date As Date = CDate(reqTB.Text)
            Dim LineNO As String = CType(gv1.DataKeys(gvr.DataItemIndex).Value, COMM.Field).Value
            DetailRef.UpdateByLine(String.Format("{0}='{1}'", DetailRef.Item1.reqDate.Name, req_date.ToShortDateString), LineNO, COMM.Fixer.eDocType.ORDER)
            ReCalDue(UID, LineNO)
        Next
        initInterface()
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        updateDueDateByCustCal()
        If Pivot.NewObjPatch.IsEUStockingCompany(MasterRef.AccErpId) Then
            AddEW()
        End If
        If ISDIRSAP = "1" Then
            Response.Redirect("~/order/OrderPreview.aspx?UID=" & UID & "&ISDIRSAP=1")
        End If
        Response.Redirect("~/order/OrderPreview.aspx?UID=" & UID)
    End Sub

    Sub AddEW()
        Dim dt As IBUS.iCartList = DetailRef.GetListAll(COMM.Fixer.eDocType.ORDER)
        If Not IsNothing(dt) Then
            Dim Count As Integer = 0
            Dim isorted As New SortedSet(Of Integer)
            For Each r As IBUS.iCartLine In dt
                If Pivot.NewObjPatch.IsEUStockingProgram(r.partNo.Value, r.Qty.Value, MasterRef.AccErpId) Then
                    DetailRef.UpdateByLine(String.Format("{0}='{1}'", DetailRef.Item1.ewFlag.Name, 3), r.lineNo.Value, COMM.Fixer.eDocType.ORDER)
                    isorted.Add(r.lineNo.Value)
                End If
            Next
            If Not IsNothing(isorted) AndAlso isorted.Count > 0 Then
                DetailRef.updateEW(isorted, MasterRef.DocReg, COMM.Fixer.eDocType.ORDER)
            End If
        End If
    End Sub

    Public Function FindControlRecursive(ByVal root As Control, ByVal id As String) As Control
        If root.ClientID = id Then Return root
        For Each c As Control In root.Controls
            Dim t As Control = FindControlRecursive(c, id)
            If Not IsNothing(t) Then Return t
        Next
        Return Nothing
    End Function
   

    Sub updateDueDateByCustCal()
        Dim dt As IBUS.iCartList = Nothing
        dt = DetailRef.GetListAll(COMM.Fixer.eDocType.ORDER)
        If Not IsNothing(dt) Then
            For Each r As IBUS.iCartLine In dt
                Dim temp As Date = Pivot.NewObjDoc.getNextCustDelDate(r.dueDate.Value, MasterRef.AccErpId)
                DetailRef.UpdateByLine(String.Format("{0} = '{1}'", r.dueDate.Name, temp.ToShortDateString), r.lineNo.Value, COMM.Fixer.eDocType.ORDER)
            Next
        End If
    End Sub
    Public Sub PickCalEnd(ByVal D As Date, ByVal N As String)
        Dim PickedCalDate As Date = D
        Dim DestC As String = N
        If DestC <> "" Then
            Dim t As TextBox = CType(FindControlRecursive(Me.Master.FindControl("ContentPlaceHolder1").FindControl("GV1"), DestC), TextBox)
            t.Text = PickedCalDate.ToString(Pivot.CurrentProfile.DatePresentationFormat)
        End If
        Me.UpReqDate.Update()
        Me.MPPickCalendar.Hide()
    End Sub
    Public Sub ShowCal(ByVal a As Object())
        Me.MPPickCalendar.Show()
    End Sub
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
            dueDate = IIf(CDate(DBITEM.dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat) = CDate("1900/01/01").ToString(Pivot.CurrentProfile.DatePresentationFormat), "TBD", CDate(DBITEM.dueDate.Value).ToString(Pivot.CurrentProfile.DatePresentationFormat))
            If Not DBITEM.partNo.Value.ToString.StartsWith("AGS-") And _
               DBITEM.itemType.Value = COMM.Fixer.eItemType.Others And DBITEM.satisfyflag.Value = 0 And dueDate <> "TBD" Then
                e.Row.Cells(5).Text = "<font color='#FF0000'>For Reference Only</font>" & "<br/>" & dueDate
            End If

            If DBITEM.itemType.Value = COMM.Fixer.eItemType.Parent And DBITEM.satisfyflag.Value = 0 Then
                e.Row.Cells(5).Text = "<font color='#FF0000'>For Reference Only</font>" & "<br/>" & dueDate
            End If
            If DBITEM.parentLineNo.Value > COMM.Fixer.StartLine Then
                CType(e.Row.FindControl("txtreqdate"), TextBox).Visible = False
                CType(e.Row.FindControl("ibtnReqDate"), ImageButton).Visible = False
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

    Protected Sub txtCustPN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As TextBox = CType(sender, TextBox)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim lineNo As Integer = Integer.Parse(CType(row.FindControl("HFlineNo"), HiddenField).Value.Trim())
        'Dim DBITEM As IBUS.iCartLine = CType(row.DataItem, IBUS.iCartLine)
        'Dim id As Integer = DBITEM.lineNo.Value ' Me.gv1.DataKeys(row.RowIndex).Value
        Dim CustPN As String = obj.Text.Trim
        DetailRef.UpdateByLine(String.Format("{0}=N'{1}'", DetailRef.Item1.CustMaterial.Name, CustPN), lineNo, COMM.Fixer.eDocType.ORDER)
        'myOrderDetail.Update(String.Format("order_id='{0}' and line_no='{1}'", OID, id), String.Format("CustMaterialNo='{0}'", CustPN))
    End Sub

    Protected Sub ibtnReqDate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim o As ImageButton = CType(sender, ImageButton)
        Dim t As TextBox = CType(o.Parent.FindControl("txtreqdate"), TextBox)
        CType(Me.ascxCalendar.FindControl("hDesC"), HiddenField).Value = t.ClientID
        Me.UPASCXCAL.Update()
        Me.MPPickCalendar.Show()
    End Sub


End Class