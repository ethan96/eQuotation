Imports System.Web.Script.Serialization
Imports Advantech.Myadvantech.DataAccess
Imports SAPDAL.ePricerDiscount
Imports Advantech.Myadvantech.Business

Public Class QuotationDetail1
    Inherits PageBase
    Dim myQD As New quotationDetail("EQ", "quotationDetail")
    Dim CATELIST As String = "", _View As Integer = 0, _IsUSUser As Boolean = False, _IsAAC As Boolean = False
    Public _IsJPOnline As Boolean = False
    Public _IsAddMultiParts As Boolean = False
    Public _IsTWOnline As Boolean = False, _IsAddBTOSPartNoDirectly As Boolean = False, _IsEUQuote As Boolean = False
    Public _IsCNAonlineUser As Boolean = False, _IsKRAonlineUser As Boolean = False, _IsHQDCUser As Boolean = False
    Public _IsUSAENC As Boolean = False, _IsABRUser As Boolean = False, _IsAAUUser As Boolean = False, _IsUSAOnlineEP As Boolean = False
    Public _IsCAPS As Boolean = False
    Public _IsUsingBreakPoints As Boolean = False
    Public _IsInGPProcess As Boolean = False
    Public Quote2ErpID As String = String.Empty
    Private _BreakPointstr As String = String.Empty
    Private _BreakPoint() As String = Nothing
    Private _BreakPointDT As DataTable = Nothing
    Public ABRDiscount As Decimal = 0
    Public _IsABRNonContribuinte As Boolean = False
    Public _Shipto As String = String.Empty, _CountryCode As String = String.Empty
    Public _IsNoSiebel As Boolean = False
    'Dim CBOMWS As New CBOMWS.MyCBOMDAL
    'Dim CBOMWS As New CBOMWS4002.MyCBOMDAL
    'Protected Function CheckBTO(ByVal BtosName As String, ByVal orgid As String) As Boolean
    '    Dim dt As DataTable = Relics.dbUtil.dbGetDataTable("MY", String.Format(" select TOP 1 PART_NO FROM  SAP_PRODUCT_ORG WHERE PART_NO ='{0}' AND  ORG_ID ='{1}'", BtosName.Trim, orgid.Trim))
    '    If dt.Rows.Count > 0 Then Return True
    '    Return False
    'End Function
    Dim IsHaveInvalid As Boolean = False
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim type As Boolean = True
        Dim msg As String = ""
        If Me.gv1.Rows.Count <= 0 OrElse IsHaveInvalid Then
            type = False
        End If

        'Ryan 20170120 Add "and false" in If statement to bypass Zero ITP validation for AJP 
        ' check items with zero itp
        If _IsJPOnline And False Then
            Dim isZITP As New Dictionary(Of String, Boolean)
            isZITP = CheckITPZero(UID)
            If isZITP.Count > 0 Then
                type = False
                msg = "Item(s): "
                For Each r As KeyValuePair(Of String, Boolean) In isZITP
                    msg &= "'" & r.Key & "' "
                Next
                msg &= "is(are) with zero ITP, please remove them from cart to enable the confirm button."
            End If
        End If
        '/ check items with zero itp

        ForbidConfirm(type, msg)
    End Sub
    Public Function CheckITPZero(ByVal UID As String) As Dictionary(Of String, Boolean)
        Dim o As New Dictionary(Of String, Boolean)
        Dim dt As New EQDS.QuotationDetailDataTable, adp As New EQDSTableAdapters.QuotationDetailTableAdapter
        dt = adp.GetQuoteDetailById(UID)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            For Each r As EQDS.QuotationDetailRow In dt
                If r.itp = 0 Then
                    'If r.itemType <> COMM.Fixer.eItemType.Parent AndAlso Relics.CommonLogic.NoStandardSensitiveITP(r.partNo) = False Then
                    'If r.itemType <> COMM.Fixer.eItemType.Parent AndAlso SAPDAL.CommonLogic.NoStandardSensitiveITP(r.partNo) = False Then
                    If r.itemType <> COMM.Fixer.eItemType.Parent AndAlso
                        Not Advantech.Myadvantech.Business.PartBusinessLogic.IsNonStandardSensitiveITP(r.partNo) Then
                        If Not o.ContainsKey(r.partNo) Then
                            o.Add(r.partNo, True)
                        End If
                    End If
                End If
            Next
        End If
        Return o
    End Function
    Public Sub ForbidConfirm(ByVal type As Boolean, ByVal msg As String)
        Me.btnConfirm.Enabled = type
        Me.lbConfirmMsg.Text = msg
        upbtnConfirm.Update()
    End Sub
    Public Function isOnly1Level(ByVal RootID As String, ByVal Org As String) As Boolean
        Dim F As Boolean = False
        If IsEstoreBom(RootID, Org) Then
            F = True
        End If
        'If (RootID.StartsWith("C-CTOS") Or RootID.StartsWith("SYS-")) And (Not RootID.StartsWith("C-CTOS-UUAAESC")) Then
        If RootID.StartsWith("C-CTOS") Or RootID.StartsWith("SYS-") Then
            F = True
        End If
        Return F
    End Function
    Public Function IsEstoreBom(ByVal BTORootID As String, ByVal Org As String) As Boolean
        If BTORootID.StartsWith("EZ-", StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If

        'Frank 2013/02/19:If Catalog local name is Pre-Configuration for AEU eStore (buy.advantech.eu) Configuration,
        'then it's a AEU eStore bom
        If Org.ToString().StartsWith("EU", StringComparison.OrdinalIgnoreCase) Then
            Dim _sql As New StringBuilder
            _sql.AppendLine(" Select a.Catalog_Org,a.CATALOG_TYPE,b.LOCAL_NAME,a.CATALOG_ID,a.CATALOG_NAME,a.CATALOG_DESC, a.CREATED ")
            _sql.AppendLine(" From CBOM_CATALOG a inner join CBOM_CATALOG_LOCALNAME b on a.CATALOG_TYPE=b.CATALOG_TYPE ")
            _sql.AppendLine(" Where a.Catalog_Org='EU' and a.CATALOG_TYPE like '%Pre-Configuration' and a.CATALOG_NAME ='" & Request("BTOITEM") & "' ")
            Dim _dt As DataTable = tbOPBase.dbGetDataTable("MY", _sql.ToString)
            If Not IsNothing(_dt) AndAlso _dt.Rows.Count > 0 AndAlso
                _dt.Rows(0).Item("LOCAL_NAME").ToString.Equals("Pre-Configuration for AEU eStore (buy.advantech.eu) Configuration",
                StringComparison.InvariantCultureIgnoreCase) Then
                Return True
            End If
        End If
        Dim ObjectEZ_FLAG As Object = tbOPBase.dbExecuteScalar("B2B",
                                                               String.Format("SELECT ISNULL(COUNT(BTONo),0) as Bcount  FROM  ESTORE_BTOS_CATEGORY WHERE  DisplayPartno ='{1}' and StoreID like '%{0}'",
                                                                     Left(Org.ToUpper, 2), BTORootID.Trim))
        If ObjectEZ_FLAG IsNot Nothing AndAlso Integer.TryParse(ObjectEZ_FLAG, 0) AndAlso Integer.Parse(ObjectEZ_FLAG) > 0 Then
            Return True
        End If
        Return False
    End Function


    'Public currencySign As String = ""

    Dim oPartNo As String = "", oQty As Integer = 0, oIsShowListPrice As Integer = 0, oIsShowDiscount As Integer = 0
    Dim oIsShowDueDate As Integer = 0, oIsLumpSumOnly As Integer = 0
    Dim _lQuotationDetail_Ext_ABR As List(Of Advantech.Myadvantech.DataAccess.QuotationDetail_Extension_ABR) = Nothing

    Function GetValFromForm() As Integer
        oPartNo = Util.ReplaceSQLChar(Me.txtPartNo.Text.Trim.ToUpper)
        If Integer.TryParse(Me.txtQty.Text, 1) = False Then Me.txtQty.Text = 1
        oQty = Me.txtQty.Text
        oIsShowListPrice = IIf(Me.cbxListPrice.Checked, 1, 0) : oIsShowDiscount = IIf(Me.cbxDisc.Checked, 1, 0)
        oIsShowDueDate = IIf(Me.cbxDueDate.Checked, 1, 0) : oIsLumpSumOnly = IIf(Me.cbxLumpSum.Checked, 1, 0)
        Return 1
    End Function

    Function SetValToForm(ByVal _partNo As String, ByVal _qty As Integer, ByVal _isShowListPrice As Integer,
                            ByVal _isShowDiscount As Integer, ByVal _isShowDueDate As Integer, ByVal _isLumpSumOnly As Integer) As Integer
        Me.txtPartNo.Text = _partNo : Me.txtQty.Text = _qty : Me.cbxListPrice.Checked = IIf(_isShowListPrice = 1, True, False)
        Me.cbxDisc.Checked = IIf(_isShowDiscount = 1, True, False) : Me.cbxDueDate.Checked = IIf(_isShowDueDate = 1, True, False)
        Me.cbxLumpSum.Checked = IIf(_isLumpSumOnly = 1, True, False)
        Return 1
    End Function

    Protected Sub initGV(ByVal UID As String, Optional ByVal DT As DataTable = Nothing)
        'If DT Is Nothing Then
        Dim orderbystr As String = "line_No"
        'If _IsTWOnline Then
        '    orderbystr = "SequenceNo"
        '    tbOPBase.dbExecuteNoQuery("EQ", String.Format("UPDATE QuotationDetail SET SequenceNo= (case when line_No >=100 then line_No else  line_No+5000 end ) WHERE quoteId ='{0}'", UID))
        'End If
        DT = myQD.GetDT(String.Format("quoteId='{0}'", UID), orderbystr)
        'End If

        If _IsABRUser Then
            Dim _qh As New Advantech.Myadvantech.DataAccess.QuotationDetailHelper()
            _lQuotationDetail_Ext_ABR = _qh.GetQuotationDetail_Extension_ABR(UID)
            Me.TitleQuoteType.Visible = True : Me.tdQuoteType.Visible = True
            Dim _QME As QuotationExtension = eQuotationDAL.GetQuoteMasterExtendionByQuoteID(UID)
            Me.lbQuoteType.Text = _QME.ABRQuoteType
            'Dim qh As New Advantech.Myadvantech.DataAccess.QuotationMasterHelper
            'Dim qm As Advantech.Myadvantech.DataAccess.QuotationMaster = qh.GetQuotationMaster(UID)
            'qm.GetABRTotalBX13()


        End If

        'If Business.isBtoOrder(DT) And Business.getTotalAmount_EW(DT) > 0 Then
        '    Dim R As DataRow = DT.NewRow
        '    With R
        '        .Item("line_No") = myQD.getMaxLineNo(UID, 1) + 1 : .Item("category") = "Extended Warranty"
        '        .Item("PartNo") = Business.getEWItemByMonth(DT.Rows(0).Item("ewFlag"))
        '        .Item("description") = "Extended Warranty" : .Item("ewFlag") = "0" : .Item("listPrice") = Business.getTotalPrice_EW(DT)
        '        .Item("newunitPrice") = R.Item("listPrice") : .Item("qty") = Business.getBtoQty(DT)
        '        .Item("reqDate") = Now.ToShortDateString : .Item("dueDate") = Now.ToShortDateString
        '        .Item("newitp") = R.Item("listPrice") : .Item("otype") = COMM.Fixer.eItemType.Others
        '        .Item("deliveryPlant") = DT.Rows(0).Item("deliveryPlant")
        '    End With

        '    DT.Rows.Add(R)
        'End If

        If String.IsNullOrEmpty(Horg.Value) Then
            If Not IsNothing(MasterRef) Then Horg.Value = MasterRef.org
        End If
        SAPDAL.SAPDAL.GetSAPProductInfo(Horg.Value, DT)
        Me.gv1.DataSource = DT : Me.gv1.DataBind()
    End Sub

    Protected Sub initInterFace(ByVal UID As String)
        If String.IsNullOrEmpty(UID) Then Exit Sub
        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        Me.cbxListPrice.Checked = IIf(dt.isShowListPrice = 1, True, False)
        Me.cbxDisc.Checked = IIf(dt.isShowDiscount = 1, True, False)
        Me.cbxDueDate.Checked = IIf(dt.isShowDueDate = 1, True, False)
        'Me.cbxLumpSum.Checked = IIf(Business.isBtoOrder(dt), True, False)
        Me.lbCompanyName.Text = dt.AccName
        If dt.AccErpId <> "" Then
            Me.lbCompanyId.Text = "(" & dt.AccErpId & ")"
            hyCustomerCreditInfo.NavigateUrl = "~/quote/CreditInfo.aspx?ID=" + dt.AccErpId + "&ORG=" + dt.org
            hyCustomerCreditInfo.Visible = True
            Quote2ErpID = dt.AccErpId
            ViewState("Quote2ErpID") = Quote2ErpID
        End If

        Me.lbRBU.Text = dt.siebelRBU
        Horg.Value = dt.org
        If Horg.Value = "EU10" Then
            Me.rbtnMD.Items(1).Enabled = True : Me.pEX.Visible = True
            _IsEUQuote = True
        End If
        lbSAPOrg.Text = Horg.Value
        Dim curr As String = dt.currency
        lbEXR.Text = Business.get_exchangerate("EUR", curr) : lbEXRB.Text = "EUR to " & curr

        'If Me._IsUSUser OrElse Me._IsTWOnline OrElse Me._IsCNAonlineUser OrElse Me._IsKRAonlineUser OrElse Me._IsHQDCUser Then
        If Me._IsUSUser OrElse Me._IsTWOnline OrElse Me._IsCNAonlineUser Then
            Me.rowTotalMargin.Visible = False
            'Me.spMarg2.Visible = False
            Me.rowMarginInformation.Visible = False
        End If

        'Frank 20170627
        If Me._IsHQDCUser Then
            'Me.spMarg1.Visible = True : Me.spMarg2.Visible = False
            Me.rowTotalMargin.Visible = True : Me.rowMarginInformation.Visible = False
            Me.rowTotalITP.Visible = False
        End If


        'Frank 20170627
        If Me._IsKRAonlineUser Then
            'Me.spMarg1.Visible = True : Me.spMarg2.Visible = False
            Me.rowTotalMargin.Visible = True : Me.rowMarginInformation.Visible = False
            Me.rowTotalITP.Visible = True
        End If


        Dim cl As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, dt.org).GetListAll(COMM.Fixer.eDocType.EQ)

        If Pivot.NewObjPatch.isHasBto(cl) = 0 Then
            Me.cbxLumpSum.Visible = False
            Me.cbxLumpSum.Checked = False
        Else
            If Not Me._IsUSUser Then
                Me.cbxLumpSum.Visible = True
            End If
            Me.cbxLumpSum.Checked = True
            'Frank 2012/09/21 If it's a BTOS quote then execute the RefreshProductInventory functions
            Business.RefreshQuotationInventory(UID)
            'Ryan 20160427 If is EU user and is BTOS quote then block adding items to quote.
            If Me._IsEUQuote Then
                btnAdd.Visible = False
            End If
        End If

        If Role.IsFranchiser() Then
            Me.HisFrCH.Value = "Y"
        End If

        Me.HCurrency.Value = Util.getCurrSign(dt.currency)
        initAutoComCateList(dt.siebelRBU, dt.org)
        initGV(UID)

        'Frank 2012/09/19 init print out format option control
        'Me.QuotationViewOptionUC1.QuoteID = UID

        '===Frank 2012/07/17 Load Sales and Order Note===
        Dim _noteDA As New EQDSTableAdapters.QuotationNoteTableAdapter()
        Dim _dt As DataTable = _noteDA.GetNoteTextBYQuoteId(UID)
        For Each _row As DataRow In _dt.Rows
            Select Case _row.Item("notetype").ToString.ToUpper
                Case "SALESNOTE"
                    Me.txtSalesNote.Text = _row.Item("notetext").ToString
                Case "ORDERNOTE"
                    If Me._IsTWOnline Or Me._IsCNAonlineUser Then
                        If Not IsDBNull(_row.Item("notetext")) Then
                            Me.txtOrderNote.Text = _row.Item("notetext").ToString.Replace("<BR />", Chr(10))
                        End If
                    Else
                        Me.txtOrderNote.Text = _row.Item("notetext").ToString
                    End If
            End Select
        Next
        'Ming add default Sales Note 2012/09/04
        If String.IsNullOrEmpty(Me.txtSalesNote.Text.Trim) Then
            Dim _SoldToID As String = String.Empty
            Dim _EQParentTA As EQDSTableAdapters.EQPARTNERTableAdapter = New EQDSTableAdapters.EQPARTNERTableAdapter
            Dim _dtep As EQDS.EQPARTNERDataTable = _EQParentTA.GetPartnerByQIDAndType(UID, "SOLDTO")
            If _dtep IsNot Nothing AndAlso _dtep.Rows.Count > 0 Then
                _SoldToID = CType(_dtep.Rows(0), EQDS.EQPARTNERRow).ERPID
                If Not String.IsNullOrEmpty(_SoldToID) Then
                    'Me.txtSalesNote.Text = Relics.SAPDAL.getSalesNotebyERPid(_SoldToID)
                    Me.txtSalesNote.Text = SAPDAL.SAPDAL.getSalesNotebyERPid(_SoldToID)
                End If
            End If
        End If
        'end
        'Nada added for default ctos note 201307262
        'Dim dtCnote As DataTable = Relics.SAPDAL.GetCTOSAssemblyInstructionListByERPIdFromMyadvantech(MasterRef.AccErpId, "")
        Dim dtCnote As DataTable = SAPDAL.SAPDAL.GetCTOSAssemblyInstructionListByERPIdFromMyadvantech(MasterRef.AccErpId, "")
        If Not IsNothing(dtCnote) AndAlso dtCnote.Rows.Count = 1 AndAlso (Not Me.txtSalesNote.Text.Contains("[****]")) Then
            Me.txtSalesNote.Text = IIf(String.IsNullOrEmpty(Me.txtSalesNote.Text), "", Me.txtSalesNote.Text + vbLf) + "[****]" + "instruction: " + vbLf + dtCnote.Rows(0).Item("FILEP").ToString
            'Me.txtSalesNote.Text + vbLf + "[****]" + _SystemName + " instruction: " + vbLf + _DOCURL
            'String.Format("Follow CTO file {0}-Electric CTO file latest version", dt.Rows(0).Item("quoteToErpId"))
            Me.UP_QuotationNote.Update()

        End If

        '/Nada added for default ctos note 20130726
        '===Frank 2012/07/17 Load Sales and Order Note  End==


    End Sub
    Sub initAutoComCateList(ByVal RBU As String, ByVal ORG As String)
        Dim ws As New CBOMWS.MyCBOMDAL
        ws.Timeout = -1
        Dim dt As New DataTable
        dt = ws.getBTOParentList(RBU, ORG, "")
        For Each r As DataRow In dt.Rows
            CATELIST = CATELIST & r.Item("catalog_id").ToString.Trim() & ","
        Next
        CATELIST = CATELIST.Trim().Trim(",")
    End Sub
    Public Sub InitDrpParent(ByVal C As IBUS.iCartList)
        Dim PreSel As String = Me.drpParentItem.SelectedValue
        Me.drpParentItem.Items.Clear()
        Me.drpParentItem.Items.Add(New ListItem("Loose items", COMM.Fixer.StartLine))
        If Not IsNothing(C) AndAlso C.Count > 0 Then
            For Each R As IBUS.iCartLine In C
                If COMM.CartFixer.isValidParentLineNo(R.lineNo.Value) Then
                    Dim li As New ListItem(R.partNo.Value & "(" & R.lineNo.Value & ")", R.lineNo.Value)
                    Me.drpParentItem.Items.Add(li)
                    If li.Value = PreSel Then
                        li.Selected = True
                    End If
                End If
            Next
            If Not IsPostBack Or _IsAddBTOSPartNoDirectly Then
                drpParentItem.SelectedIndex = drpParentItem.Items.Count - 1
            End If
        End If
        If Role.IsEUSales() Then
            If drpParentItem.Items.Count > 1 Then
                Dim LooseItem As ListItem = drpParentItem.Items.FindByValue("0")
                If LooseItem IsNot Nothing Then
                    drpParentItem.Items.Remove(LooseItem)
                End If
            End If
        End If
        If _IsTWOnline OrElse _IsCNAonlineUser Then
            If drpParentItem.Items.Count > 1 Then
                Dim ItemsCount As Integer = drpParentItem.Items.Count()
                drpParentItem.SelectedIndex = ItemsCount - 1
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Frank 201503258, set gridview's default button to btnUpdate
        Me.Form.DefaultButton = Me.btnUpdate.UniqueID

        If MasterRef.DocType <> COMM.Fixer.eDocType.EQ Then
            Util.showMessage("Only Quotation is editable.", "back")
        End If
        'Frank 2012/08/01:To prevent a problem if the Request("View") is not a number
        If Integer.TryParse(Request("View"), 0) = False Then
            Me._View = 0
        Else
            Me._View = Request("View")
        End If

        If UID IsNot Nothing Then
            If Role.IsUsaUser() Then _IsUSUser = True
            'If Role.IsUSAACSales Then _IsAAC = True
            If Role.IsUSAACSales() AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2100") Then
                _IsAAC = True
            End If
            If Role.IsUsaAenc() Then _IsUSAENC = True
            If Role.IsCAPS() Then _IsCAPS = True : _IsNoSiebel = True
            If Role.IsJPAonlineSales() Then _IsJPOnline = True : _IsNoSiebel = True
            If Role.IsTWAonlineSales() Then _IsTWOnline = True
            If Role.IsCNAonlineSales() Then _IsCNAonlineUser = True
            If Role.IsKRAonlineSales() Then _IsKRAonlineUser = True
            If Role.IsHQDCSales Then Me._IsHQDCUser = True
            If Role.IsABRSales Then Me._IsABRUser = True
            If Role.IsAAUSales Then Me._IsAAUUser = True
            If Role.IsAonlineUsa Then Me._IsUSAOnlineEP = True

            ''Ryan 20160802 Validate if USAonline quote is in GP Process or not
            'If _IsUSAOnlineEP Then
            '    Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", " select UID from QuoteApproval WHERE QuoteID = '" + UID + "' ")
            '    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            '        _IsInGPProcess = True
            '    End If
            'End If

            If _IsTWOnline OrElse _IsCNAonlineUser OrElse _IsKRAonlineUser _
                OrElse _IsHQDCUser OrElse _IsABRUser OrElse _IsAAUUser OrElse _IsJPOnline Then
                'OrElse _IsUSAOnlineEP Then
                'OrElse _IsUSAENC Then
                _IsAddMultiParts = True
            End If

            'If _IsUSAENC OrElse (COMM.Util.IsTesting AndAlso _IsUSAOnlineEP) Then
            If _IsUSAENC Then
                _IsUsingBreakPoints = True
            End If

            'Ryan 20180110 Show volume discount tab for AENC & AAC
            If _IsUSAENC OrElse _IsAAC Then
                TabPanel3.Visible = True
            End If

            If String.Equals(Horg.Value, "EU10", StringComparison.CurrentCultureIgnoreCase) Then
                _IsEUQuote = True
            End If
            If ViewState("Quote2ErpID") IsNot Nothing Then
                Quote2ErpID = ViewState("Quote2ErpID")
            End If
            'If Not Role.IsTWAonlineSales() Then
            If Not _IsTWOnline Then
                txtPartNo.Attributes.Add("onkeydown", "return OnTxtPersonInfoKeyDown();")
                txtPartNo.Width = "220"
            End If

            If _IsUsingBreakPoints Then
                Dim _dtQuoteDetailMasterBreakPoint As DataTable = tbOPBase.dbGetDataTable("EQ", "Select QuoteID,BreakPoints From QuotationMaster_BreakPoint where quoteid='" & UID & "'")
                If _dtQuoteDetailMasterBreakPoint IsNot Nothing AndAlso _dtQuoteDetailMasterBreakPoint.Rows.Count > 0 Then
                    _BreakPointstr = _dtQuoteDetailMasterBreakPoint.Rows(0).Item("BreakPoints").ToString
                    _BreakPoint = _BreakPointstr.Split(",")
                End If

                If Not String.IsNullOrEmpty(_BreakPointstr) Then
                    Dim _SQL As New StringBuilder
                    _SQL.AppendLine(" Select QuoteID,line_No,partNo,QuantityBreakPoint,SAPQuantityUnitPrice,CustomerQuantityUnitPrice From QuotationDetail_BreakPoints ")
                    _SQL.AppendLine("  where quoteid='" & UID & "' AND QuantityBreakPoint in (" & _BreakPointstr & ") order by line_no,QuantityBreakPoint ")
                    _BreakPointDT = tbOPBase.dbGetDataTable("EQ", _SQL.ToString)
                End If

            End If

            ''Ryan 20160802 Block several objects' visibility while quote is in GP Process.
            'If _IsInGPProcess Then
            '    lbtnConfig.Enabled = False
            '    btnAdd.Enabled = False
            '    lbAddErrMsg.Text = "This quote is under GP process, several functions are disabled."
            'End If

            'Ryan 20160823 
            '_Shipto = Advantech.Myadvantech.Business.UserRoleBusinessLogic.EQgetShiptoIDBySoldtoID(Quote2ErpID, UID)
            '_CountryCode = Advantech.Myadvantech.Business.UserRoleBusinessLogic.getCountryCodeByERPID(_Shipto)

        End If

        'Frank 20180425 取ABR QUOTE'S discount rate
        If _IsABRUser Then
            Dim abrdiscountdt As DataTable = tbOPBase.dbGetDataTable("EQ", "select * from QuotationExtensionABR where quoteId = '" + UID + "'")
            If abrdiscountdt Is Nothing OrElse abrdiscountdt.Rows.Count = 0 Then
                ABRDiscount = 0
            Else
                ABRDiscount = Convert.ToDecimal(abrdiscountdt.Rows(0).Item("discount").ToString)
            End If

            Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(UID)
            If _QMaster IsNot Nothing AndAlso Not String.IsNullOrEmpty(_QMaster.quoteToErpId) Then
                Me.Quote2ErpID = _QMaster.quoteToErpId
                Me._IsABRNonContribuinte = Advantech.Myadvantech.DataAccess.SAPDAL.IsABRNonContribuinte(Me.Quote2ErpID, True)
            End If

        End If

        If Not IsPostBack Then

            HDquoteid.Value = UID
            'If Request("VIEW") = 0 Then

            initInterFace(UID)

            If Me._IsUSUser Then
                '20120726 TC: Per Fei's approval hide Margin/Discount block
                UP_QuotationNote.Visible = True : TabPanel2.Visible = False
                cbxListPrice.Visible = False : cbxDisc.Visible = False : cbxDueDate.Visible = False : cbxLumpSum.Visible = False ': QuotationViewOptionUC1.Visible = True
                Me.tbOptions.Visible = False
                Me.lbAddPlant.Visible = True : Me.dlAddPlant.Visible = True
                If Me._IsUSAENC Then
                    TR_Leadtime_warranty_AENC.Visible = True
                End If
            ElseIf (_IsTWOnline Or _IsCNAonlineUser) Then
                twPriceGrade.Visible = True
                Me.tbOptions.Visible = False : UP_QuotationNote.Visible = True
                cbxLumpSum.Visible = False : cbxListPrice.Visible = False : cbxDisc.Visible = False : cbxDueDate.Visible = False
                tdMinimumPrice.Visible = True
            ElseIf _IsKRAonlineUser Then
                UP_QuotationNote.Visible = True : TabPanel2.Visible = True : Me.rbtnMD.Items(1).Enabled = True
                cbxListPrice.Visible = False : cbxDisc.Visible = False : cbxDueDate.Visible = False : cbxLumpSum.Visible = False ': QuotationViewOptionUC1.Visible = True
                Me.tbOptions.Visible = False
                Me.lbAddPlant.Visible = False : Me.dlAddPlant.Visible = False
            ElseIf _IsHQDCUser Then
                UP_QuotationNote.Visible = True : TabPanel2.Visible = False
                cbxListPrice.Visible = False : cbxDisc.Visible = False : cbxDueDate.Visible = False : cbxLumpSum.Visible = False ': QuotationViewOptionUC1.Visible = True
                Me.tbOptions.Visible = False
                Me.lbAddPlant.Visible = False : Me.dlAddPlant.Visible = False
            ElseIf _IsABRUser Then
                UP_QuotationNote.Visible = True : TabPanel2.Visible = True
                Me.trUnitPriceAdd.Visible = False
                cbxListPrice.Visible = False : cbxDisc.Visible = False : cbxDueDate.Visible = False : cbxLumpSum.Visible = False ': QuotationViewOptionUC1.Visible = True
                Me.tbOptions.Visible = False
                Me.lbAddPlant.Visible = False : Me.dlAddPlant.Visible = False
            ElseIf _IsAAUUser Then
                UP_QuotationNote.Visible = True : TabPanel2.Visible = False
                cbxListPrice.Visible = False : cbxDisc.Visible = False : cbxDueDate.Visible = False : cbxLumpSum.Visible = False ': QuotationViewOptionUC1.Visible = True
                Me.tbOptions.Visible = False
                Me.lbAddPlant.Visible = False : Me.dlAddPlant.Visible = False
            Else
                Me.trUnitPriceAdd.Visible = False
            End If

            If _IsUsingBreakPoints Then
                Me.btnBreakPoints.Visible = True
            End If

            If HisFrCH.Value = "Y" Then
                Me.TabPanel2.Visible = False : Me.imgPartNoAdd.Visible = False : Me.tbOptions.Visible = False
            End If
            If Me._IsJPOnline Then
                cbxDisc.Visible = False : cbxListPrice.Visible = False : rowTotalITP.Visible = True
            End If
            If Not String.IsNullOrEmpty(UID) Then
                Dim _QM_Extension As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
                If _QM_Extension IsNot Nothing Then
                    If _QM_Extension.IsShowTotal IsNot Nothing Then cbShowTotal.Checked = IIf(_QM_Extension.IsShowTotal, True, False)
                    If _QM_Extension.IsShowFreight IsNot Nothing Then cbShowFreight.Checked = IIf(_QM_Extension.IsShowTotal, True, False)

                    'Frank 20160606 I put LeadTime info to column Engineer_Telephone for AENC team
                    If Not String.IsNullOrEmpty(_QM_Extension.Engineer_Telephone) Then
                        Me.TextBoxLeadTime.Text = _QM_Extension.Engineer_Telephone
                    End If
                    'Frank 20160606 I put Warranty info to column Warranty for AENC team
                    If Not String.IsNullOrEmpty(_QM_Extension.Warranty) Then
                        Me.TextBoxWarranty.Text = _QM_Extension.Warranty
                    End If
                End If

            End If

            'Ryan 20170111 For EU quotes, show Standard Margin and PTD Margin label
            If _IsEUQuote Then
                'spStandardMargin.Visible = True
                'spPTDMargin.Visible = True
                rowStandardMargin.Visible = True
                rowPTDMargin.Visible = True
            End If

            ''Ryan 20160818 Get ABR quote discount when page load 
            'If _IsABRUser Then
            '    TabPanel2.HeaderText = "Disc"
            '    txtMarginDisc.Text = "Disc"
            '    'rbtnMD.Items.Remove(rbtnMD.Items.FindByValue("Mar"))
            '    rbtnMD.Items(1).Enabled = True
            '    rbtnMD.Items(1).Text = "Addition"

            '    Dim abrdiscountdt As DataTable = tbOPBase.dbGetDataTable("EQ", "select * from QuotationExtensionABR where quoteId = '" + UID + "'")
            '    If abrdiscountdt Is Nothing OrElse abrdiscountdt.Rows.Count = 0 Then
            '        ABRDiscount = 0
            '        Me.txtDMValue.Text = 0
            '    Else
            '        ABRDiscount = Convert.ToDecimal(abrdiscountdt.Rows(0).Item("discount").ToString)
            '        'Me.txtDMValue.Text = ABRDiscount
            '        If ABRDiscount < 0 Then
            '            rbtnMD.SelectedIndex = 1
            '        Else
            '            rbtnMD.SelectedIndex = 0
            '        End If
            '        Me.txtDMValue.Text = Math.Abs(ABRDiscount)

            '    End If
            'End If
            'Ryan 20160818 Get ABR quote discount when page load 
            If _IsABRUser Then
                TabPanel2.HeaderText = "Disc"
                txtMarginDisc.Text = "Disc"
                rbtnMD.Items(1).Enabled = True
                rbtnMD.Items(1).Text = "Addition"
                If ABRDiscount < 0 Then
                    rbtnMD.SelectedIndex = 1
                Else
                    rbtnMD.SelectedIndex = 0
                End If
                Me.txtDMValue.Text = Math.Abs(ABRDiscount)
            End If

            'Ryan 20161116 Hide siebel org for pure sap equotation (non-siebel)
            If _IsNoSiebel Then
                span1.Visible = False
                lbRBU.Visible = False
            End If

        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        lbAddErrMsg.Text = ""
        Dim pns() As String = Split(Trim(txtPartNo.Text), ";")


        'Frank 20180411 BSMI checking for ATW's quote
        Dim _BSMIMessage As String = String.Empty
        If _IsTWOnline Then
            Dim _dt = Advantech.Myadvantech.DataAccess.SAPDAL.CheckBSMIParts(pns)
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                For Each _row As DataRow In _dt.Rows
                    _BSMIMessage &= _row.Item("ITEM_NUMBER") & " : " & _row.Item("BSMI_Certification") & "</BR>"
                Next
            End If

            Dim inValidParts As New List(Of String)
            If Advantech.Myadvantech.Business.PartBusinessLogic.isATWPartsWithoutZMIP(pns.ToList, MasterRef.org, MasterRef.AccErpId, inValidParts) Then
                lbAddErrMsg.Text = "Condition ZMIP not maintained in below parts, please check again.<br/>"
                For Each s As String In inValidParts
                    lbAddErrMsg.Text += s + "<br/>"
                Next
                Exit Sub
            End If

        End If


        For Each pn In pns
            If pn.ToUpper.StartsWith("AGS-EW") Then
                lbAddErrMsg.Text = "AGS item cannot be added directly." 'tbProdInfo.Visible = False : 
                Exit Sub
            End If
        Next
        GetValFromForm()
        'If _IsTWOnline Or _IsCNAonlineUser Then
        If _IsAddMultiParts Then

            Dim _DisplayPrice As Decimal = 0
            If IsNumeric(Me.txtUnitPriceAdd.Text.Trim) Then _DisplayPrice = Me.txtUnitPriceAdd.Text.Trim()

            'If pns.Length > 1 Then

            'Ming20150304开始批量添加料
            Dim _QuoteDetailDT As DataTable = tbOPBase.dbGetDataTable("EQ", "Select * from QuotationDetail Where QuoteID='-1'")

            'Dim _QuoteDetail_Extension_ABRDT As DataTable = Nothing
            'If _IsABRUser Then
            '    _QuoteDetail_Extension_ABRDT = tbOPBase.dbGetDataTable("EQ", "Select * from QuotationDetail_Extension_ABR Where QuoteID='-1'")
            'End If

            'Dim dt1 As DataTable = tbOPBase.dbGetDataTable("MY", String.Format("Select PART_NO,EXTENDED_DESC From SAP_PRODUCT_EXT_DESC Where PART_NO in ('{0}')", String.Join("','", pns)))
            Dim dt1 As DataTable = tbOPBase.dbGetDataTable("MY", String.Format("Select a.PART_NO,isnull(b.EXTENDED_DESC,a.PRODUCT_DESC) as EXTENDED_DESC From SAP_PRODUCT a left join SAP_PRODUCT_EXT_DESC b on a.PART_NO=b.PART_NO Where a.PART_NO in ('{0}')", String.Join("','", pns)))
            Dim _row As DataRow = Nothing, _ParentLineNumber = Me.drpParentItem.SelectedValue, _ItemType = 0, _ProDesc As String = String.Empty, _IsUpdateEW = False
            Dim EWitem As QuoteItem = Nothing, EWint As Integer = 0
            Dim _LineNo As Integer = 1
            If _ParentLineNumber = 0 Then
                Dim _Quotelist As List(Of QuoteItem) = MyQuoteX.GetQuoteLooseItems(UID)
                If _Quotelist IsNot Nothing AndAlso _Quotelist.Count > 0 Then
                    _LineNo = _Quotelist.LastOrDefault().line_No + 1
                End If
            Else
                Dim QITEM As QuoteItem = MyQuoteX.GetQuoteItem(UID, _ParentLineNumber)
                If QITEM IsNot Nothing AndAlso QITEM.ChildListX.Count > 0 Then
                    _LineNo = QITEM.ChildListX.LastOrDefault().line_No + 1
                    Dim _Quotelist As List(Of QuoteItem) = QITEM.ChildListX
                    EWitem = _Quotelist.FirstOrDefault(Function(p) p.partNo.StartsWith("AGS-EW"))
                    If EWitem IsNot Nothing Then
                        _IsUpdateEW = True
                        _LineNo = EWitem.line_No
                        EWint = QITEM.ewFlag
                        'QITEM.ewFlag = 0
                        'For Each item In _Quotelist
                        '    item.ewFlag = 0
                        '    If item.IsEWpartnoX Then
                        '        MyQuoteX.DeleteQuoteItem(UID, item.line_No)
                        '        _LineNo = item.line_No
                        '        Exit For
                        '    End If
                        'Next
                    End If
                Else
                    _LineNo = CInt(_ParentLineNumber) + 1
                End If

            End If
            'Dim IsHaveBtos As Boolean = False
            'For Each pn In pns
            '    If Business.IsPartInBTOS_CTOSMaterialGroup(pn) Then
            '        IsHaveBtos = True
            '        Exit For
            '    End If
            'Next
            'If IsHaveBtos Then
            '    ReDim Preserve pns(pns.Length)
            '    pns(pns.Length - 1) = "AGS-CTOS-SYS-B"
            'End If
            'Dim _IsATWUser As Boolean = Role.IsTWAonlineSales
            'Dim _IsAUSUser As Boolean = Role.IsUsaUser
            Dim _ListPrice As Decimal, _UnitPrice As Decimal, ITP As Decimal, isHaveErpID = False
            Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(UID)
            If _QMaster Is Nothing Then Exit Sub
            If _QMaster.quoteToErpId IsNot Nothing AndAlso Not String.IsNullOrEmpty(_QMaster.quoteToErpId) Then isHaveErpID = True
            Dim _Order As Order = Nothing

            'Ryan 20180724 New get plant logic
            Dim plant As String = Advantech.Myadvantech.DataAccess.SAPDAL.GetPlantByOrg(_QMaster.org)
            'Dim plant As String = Business.getPlantByOrgID(_QMaster.org)

            'Frank 20151014 USA user will not go througt this part, so I commented out the line
            'If _IsUSUser Then plant = Me.dlAddPlant.SelectedValue

            If isHaveErpID Then
                'Ming 20150305 批量获取price
                Dim PNlist As New List(Of PNInfo)
                For Each pn In pns
                    If Not Business.IsPartInBTOS_CTOSMaterialGroup(pn) Then

                        'PNlist.Add(New PNInfo(pn, oQty, plant))
                        If _IsABRUser Then
                            'Frank 2016 ABR's tax will be infulenced by qty, so I just fixed the qty to 1 to let the API return the
                            'tax by qty 1
                            PNlist.Add(New PNInfo(pn, 1, plant))
                        Else
                            PNlist.Add(New PNInfo(pn, oQty, plant))
                        End If
                    End If
                Next
                If PNlist.Count > 0 Then
                    _Order = MyQuoteX.getMultiPrice(UID, PNlist)
                End If
            End If

            'Dim MaxParentLineNo As Integer = MyQuoteX.getMaxParentLineNo(UID) + 100

            Dim MaxParentLineNo As Integer = MyQuoteX.getBtosParentLineNo(UID)


            Dim _MinimumPrice_SalesTeam As SAPDAL.SAPDAL.MinimumPrice_SalesTeam
            If _IsTWOnline Then
                _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.ATW_AOnline
            End If
            'If _IsHQDCUser Then
            '    _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.Intercon_iA
            'End If
            If Role.IsInterconIA Then
                _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.Intercon_iA
            ElseIf Role.IsInterconEC OrElse Role.IsInterconIS Then
                _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.Intercon_EC
            End If


            For Each pn In pns
                'Ming 20150511 批量添加判断是否带有Btos
                Dim isBtosParent As Boolean = Business.IsPartInBTOS_CTOSMaterialGroup(pn)
                If isBtosParent Then
                    ' _LineNo = MyQuoteX.getBtosParentLineNo(UID)
                    If _LineNo < MaxParentLineNo Then
                        _LineNo = MaxParentLineNo
                    Else
                        _LineNo = ((_LineNo + 100) \ 100) * 100
                    End If
                    _ParentLineNumber = _LineNo
                End If
                _row = _QuoteDetailDT.NewRow()
                _row.Item("PartNo") = pn
                _row.Item("quoteId") = UID
                _row.Item("Line_No") = _LineNo
                _ListPrice = 0 : _UnitPrice = 0 : ITP = 0
                If Not isBtosParent Then
                    If isHaveErpID AndAlso _Order IsNot Nothing Then
                        Dim _part As Part = _Order.LineItems.FirstOrDefault(Function(p) p.PartNumber = pn)
                        If _part IsNot Nothing Then
                            _ListPrice = _part.ListPrice
                            _UnitPrice = _part.UnitPrice

                            If _part.ListPrice > 0 AndAlso _part.UnitPrice > 0 AndAlso _part.MinimumPrice <> -1 Then
                                'Adding or Updating part's minimum price from cache container
                                SAPDAL.SAPDAL.SetMinPriceToCache(_QMaster.org, pn, Me.HCurrency.Value, _part.MinimumPrice, _MinimumPrice_SalesTeam)
                            End If

                        End If
                    End If

                    If Not isHaveErpID OrElse _ListPrice = 0 OrElse _UnitPrice = 0 Then
                        _ListPrice = 0 : _UnitPrice = 0
                        Business.GetPriceV2(pn, UID, Pivot.CurrentProfile.getCurrOrg, _ListPrice, _UnitPrice)
                    End If

                    'Ryan 20170731 Get price from company default currency 
                    If isHaveErpID AndAlso _ListPrice = 0 AndAlso _UnitPrice = 0 Then
                        Dim ReSimulatePrice As Boolean = False

                        Dim CompanyCurrency As Object = tbOPBase.dbExecuteScalar("MY", String.Format("SELECT TOP 1 CURRENCY FROM SAP_DIMCOMPANY WHERE COMPANY_ID = '{0}'", _QMaster.quoteToErpId))
                        If CompanyCurrency IsNot Nothing AndAlso Not String.IsNullOrEmpty(CompanyCurrency) AndAlso Not _QMaster.currency.Equals(CompanyCurrency.ToString, StringComparison.OrdinalIgnoreCase) Then
                            ReSimulatePrice = True

                            'Ryan 20170731 Special Logic for AKR if current currency != KRW 
                            If _IsKRAonlineUser AndAlso ReSimulatePrice Then
                                Dim _SAPDAL As New SAPDAL.SAPDAL
                                Dim dtPrice As New DataTable
                                _SAPDAL.getSAPPriceByTable(pn, _QMaster.org, _QMaster.quoteToErpId, _QMaster.quoteToErpId, CompanyCurrency.ToString, "", dtPrice)
                                If dtPrice.Rows.Count > 0 Then
                                    For Each rPrice As DataRow In dtPrice.Rows
                                        _UnitPrice += rPrice.Item("Netwr")
                                        _ListPrice += rPrice.Item("Kzwi1")
                                    Next
                                End If
                                Dim ExchangeRate As Decimal = CType(Business.get_exchangerate(CompanyCurrency.ToString, _QMaster.currency), Decimal)
                                If ExchangeRate <> 0 Then
                                    _ListPrice = _ListPrice * ExchangeRate
                                    _UnitPrice = _UnitPrice * ExchangeRate
                                Else
                                    'Exchange failed, set price back to zero
                                    _ListPrice = 0 : _UnitPrice = 0
                                End If
                            End If
                        End If
                    End If

                    If _IsUSAOnlineEP Then
                        Dim strErr1 As String = String.Empty
                        Business.isANAPnBelowGPV2(MasterRef.AccErpId, pn, _UnitPrice, ITP, strErr1)
                    Else
                        Business.GetITP(UID, pn, ITP, _UnitPrice)
                    End If

                    'Ryan 20170531 Comment out old AJP ITP logic due to new logic has been activated in Business.GetITP
                    'Ryan 20170221  If is AJP and ITP = 0, means system can't get ITP with ADVAJP, then get ITP from _part.conditions which are set from order simulation
                    'If ITP = 0 AndAlso _IsJPOnline Then
                    '    Dim _part As Part = _Order.LineItems.FirstOrDefault(Function(p) p.PartNumber = pn)
                    '    If _part IsNot Nothing AndAlso _part.Conditions.VPRS > 0 Then
                    '        ITP = _part.Conditions.VPRS
                    '    End If
                    'End If
                    'Ryan 20170322 Get XAJP item's ITP from SAP latest purchased price
                    'If _IsJPOnline AndAlso pn.StartsWith("XAJP", StringComparison.OrdinalIgnoreCase) Then
                    '    Dim decimalvalue As Decimal = 0
                    '    Dim str As String = "select a.netpr/a.peinh*100 as itp from saprdp.ekpo a inner join saprdp.ekbe b on a.ebeln=b.ebeln and a.ebelp=b.ebelp " +
                    '                        " where a.mandt='168' and b.mandt='168' And b.matnr='" + pn + "' and b.werks='JPH1' and b.waers='JPY' And rownum=1 order by b.BUDAT desc"
                    '    Dim obj As Object = OracleProvider.ExecuteScalar("SAP_PRD", str)
                    '    If Not obj Is Nothing AndAlso Decimal.TryParse(obj.ToString, decimalvalue) Then
                    '        ITP = decimalvalue
                    '    End If
                    'End If
                    'End Ryan 20170531 Comment out

                End If

                'ICC 2016/1/14 When KRAonline add AGS-CTOS-SYS-A/B, set list price & unit price. Request by Jessica.Lee.
                If _IsKRAonlineUser = True Then
                    If pn.Equals("AGS-CTOS-SYS-A", StringComparison.OrdinalIgnoreCase) Then
                        _ListPrice = 40000
                        _UnitPrice = 40000
                    ElseIf pn.Equals("AGS-CTOS-SYS-B", StringComparison.OrdinalIgnoreCase) Then
                        _ListPrice = 60000
                        _UnitPrice = 60000
                    End If
                End If

                '20171002 Special Logic for AJP per YC's request. If PTD items' unit price < ITP/0.75, then do update
                If _IsJPOnline AndAlso Business.IsPTD(pn) Then
                    Dim AJPPtradePrice As Decimal = Business.GetAJPPTradePrice(_UnitPrice, ITP)
                    If _UnitPrice < AJPPtradePrice Then
                        _ListPrice = AJPPtradePrice
                        _UnitPrice = AJPPtradePrice
                    End If
                End If

                _row.Item("ListPrice") = _ListPrice
                _row.Item("UnitPrice") = _UnitPrice
                _row.Item("newUnitPrice") = _UnitPrice
                If isBtosParent Then
                    _row.Item("HigherLevel") = 0
                Else
                    _row.Item("HigherLevel") = _ParentLineNumber
                End If
                _row.Item("RECFIGID") = ""
                _row.Item("ItemType") = 0
                If isBtosParent Then
                    _row.Item("ItemType") = 1
                End If
                _row.Item("QTY") = oQty

                'Ryan 20170221 AJP同ATW
                'Frank 20150213 ATW的報價單，Virtual Part No.預設帶出Part No.
                If _IsTWOnline OrElse _IsJPOnline OrElse _IsKRAonlineUser Then _row.Item("VirtualPartNo") = _row.Item("PartNo")

                If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then
                    Dim dr() As DataRow = dt1.Select(String.Format("PART_NO ='{0}'", pn))
                    If dr.Length >= 1 Then
                        _row.Item("Description") = dr(0).Item("EXTENDED_DESC").ToString
                    End If
                Else
                    _row.Item("Description") = ""
                End If

                'Frank commented out below if....end if
                'If _IsUSUser AndAlso _row.Item("ItemType") = 0 Then
                '    _row.Item("Itp") = Business.GetAUSCost(_row.Item("PartNo"), "USH1")
                'Else
                '    _row.Item("Itp") = ITP
                'End If
                _row.Item("Itp") = ITP

                _row.Item("newItp") = ITP
                _row.Item("DeliveryPlant") = plant
                _row.Item("Category") = ""
                _row.Item("rohs") = 0
                _row.Item("satisfyFlag") = 0
                _row.Item("canBeConfirmed") = 1
                _row.Item("custMaterial") = ""
                _row.Item("inventory") = 0
                _row.Item("modelNo") = ""
                _row.Item("ewFlag") = EWint
                _row.Item("reqDate") = Now
                _row.Item("dueDate") = Now
                _row.Item("DisplayQty") = oQty.ToString()
                If pns.Length = 1 AndAlso _DisplayPrice > 0 Then
                    If _IsTWOnline OrElse _IsCNAonlineUser Then
                        _row.Item("DisplayUnitPrice") = _DisplayPrice
                    Else
                        Dim _newunitprice As Decimal = 0
                        If Decimal.TryParse(_DisplayPrice, _newunitprice) Then
                            _row.Item("newUnitPrice") = _newunitprice
                        End If
                    End If
                End If
                _row.Item("DisplayLineNo") = _LineNo

                'If _IsABRUser Then
                '    Dim _abrqderow As DataRow = _QuoteDetail_Extension_ABRDT.NewRow
                '    _abrqderow.Item("quoteid") = UID
                '    _abrqderow.Item("line_No") = _LineNo
                '    _QuoteDetail_Extension_ABRDT.Rows.Add(_abrqderow)
                'End If

                _LineNo += 1
                _QuoteDetailDT.Rows.Add(_row)

            Next

            Dim bk As New System.Data.SqlClient.SqlBulkCopy(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
            bk.DestinationTableName = "QuotationDetail"
            bk.WriteToServer(_QuoteDetailDT)

            'If _IsABRUser Then
            '    bk.DestinationTableName = "QuotationDetail_Extension_ABR"
            '    bk.WriteToServer(_QuoteDetail_Extension_ABRDT)
            'End If

            If _IsUpdateEW Then
                If EWitem IsNot Nothing Then
                    EWitem.line_No = _LineNo + 1
                    MyUtil.Current.CurrentDataContext.SubmitChanges()
                End If
                MyQuoteX.ReSetLineNo(UID)
            End If

            Business.ADDQuotationUpdatelog(UID, Pivot.CurrentProfile.UserId & " added partno " & txtPartNo.Text)

            'Candy7希望加完料號後價格主動清空
            Me.txtUnitPriceAdd.Text = ""

            If _IsCNAonlineUser Then
                Dim _errmsg As String = String.Empty
                'QuoteBusinessLogic.WriteACNQuoteLinesITP(UID, _errmsg)
            End If

            If _IsHQDCUser Then
                Dim _errmsg As String = String.Empty
                QuoteBusinessLogic.WriteInterconQuoteLinesITP(UID, _errmsg)
            End If

            'Ryan 20160819 Recalculate ABR quote price after adding new part.
            If _IsABRUser Then
                Advantech.Myadvantech.Business.QuoteBusinessLogic.UpdateSystemPriceForABRQuote(UID, ABRDiscount)
            End If

            If Not String.IsNullOrEmpty(_BSMIMessage) Then
                lbAddErrMsg.Text = _BSMIMessage
            End If

            'initGV(UID)
            'Ming 20150514 如果使用过price grade，之后添加的产品继续用price grade获取price 
            If Trim(TBpriceGrade.Text).Length = 8 Then
                BTgetDiscountPrice_Click(Nothing, Nothing)
            Else
                initGV(UID)
            End If
            '----Add multi-part--- end
            Exit Sub
        End If

        '\Ming20150304保留单个料号添加的逻辑
        If String.IsNullOrEmpty(Horg.Value) Then
            If Not IsNothing(MasterRef) Then Horg.Value = MasterRef.org
        End If
        Dim addItemType As Integer = COMM.Fixer.eItemType.Others, intParentLineNo As Integer = 0, strCategoryName As String = ""
        Dim ewFlag As Integer = 0, _PlantID As String = Business.getPlantByOrgID(MasterRef.org)
        'Frank 2012/10/08: US Aonline sales can determine delivery plant of part
        If _IsUSUser Then _PlantID = Me.dlAddPlant.SelectedValue


        If Me.drpParentItem.SelectedValue > 0 Then
            Dim aptQD As New EQDSTableAdapters.QuotationDetailTableAdapter
            Dim dtQD As EQDS.QuotationDetailDataTable = aptQD.GetSpecificLinePN(UID, Me.drpParentItem.SelectedValue)
            If dtQD.Rows.Count > 0 Then
                ewFlag = dtQD(0).ewFlag
                oQty = dtQD(0).qty * Me.txtQty.Text
                addItemType = COMM.Fixer.eItemType.Others : strCategoryName = Business.GetCategoryName(oPartNo, Horg.Value) : intParentLineNo = Me.drpParentItem.SelectedValue
            Else
                lbAddErrMsg.Text = "Invalid Parent Line No." 'tbProdInfo.Visible = False : 
                Exit Sub
            End If
        End If

        'Ryan 20161116 Exit adding process for JP01 if quotation has BTOS and is adding loose items.
        If _IsJPOnline Then
            If MyQuoteX.IsHaveBtos(UID) AndAlso Me.drpParentItem.SelectedValue = 0 Then
                lbAddErrMsg.Text = "Item can not be added as loose item while there is BTOS system in quotation."
                Exit Sub
            End If
        End If


        '20130530 TC: Use Ajax to get TWH1 inventory for added item
        'Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "getACLATP", "getACLATP('" + Me.txtPartNo.Text + "');", True)


        If Me.tdAddItemType.Visible = True Then addItemType = CInt(Me.dlAddItemType.SelectedValue)
        Dim lineNo As Integer = 0, strErr As String = ""
        If Me._IsUSUser AndAlso trUnitPriceAdd.Visible = True AndAlso IsNumeric(Me.txtUnitPriceAdd.Text.Trim) Then
            Dim unitPrice As Decimal = 0
            If IsNumeric(Me.txtUnitPriceAdd.Text.Trim) Then unitPrice = Me.txtUnitPriceAdd.Text.Trim()
            Dim decGPPrice As Decimal = -1

            'Frank 2016-03-21 only 
            'If Business.isANAPnBelowGPV2(MasterRef.AccErpId, oPartNo, unitPrice, decGPPrice, strErr) AndAlso Not _IsAAC Then
            If Business.isANAPnBelowGPV2(MasterRef.AccErpId, oPartNo, unitPrice, decGPPrice, strErr) _
                AndAlso Me._IsUSAOnlineEP Then
                'AndAlso Role.IsANAAOnlineTeamsByOfficeCode() Then
                lbAddErrMsg.Text = "Item " + oPartNo + " is below GP Price (" & FormatNumber(decGPPrice, 2).ToString() & ")" 'tbProdInfo.Visible = False : 
                initGV(UID)
                Exit Sub
            Else
                'Dim listPrice As Decimal = 0 ' listPrice = Business.getListPrice(oPartNo, "US01", "USD")
                If Not Business.ADD2QUOTE_V2_1(UID, oPartNo, oQty, ewFlag, addItemType, strCategoryName, 1, 1, Now, intParentLineNo, _PlantID, lineNo, strErr, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ) Then
                    lbAddErrMsg.Text = strErr 'tbProdInfo.Visible = False : 
                    Exit Sub
                End If
                Dim _quoteitem As QuoteItem = MyQuoteX.GetQuoteItem(UID, lineNo)
                _quoteitem.newUnitPrice = unitPrice
                MyUtil.Current.CurrentDataContext.SubmitChanges()
                'Dim aptQD As New EQDSTableAdapters.QuotationDetailTableAdapter
                'aptQD.UpdateItemPriceByLine(listPrice, unitPrice, UID, lineNo)
            End If
        Else

            'Frank 20140304: If there is a BTOS part no to be added, then add it as a new parent item in the quotation_detail table
            If _IsTWOnline OrElse _IsCNAonlineUser OrElse _IsKRAonlineUser OrElse _IsHQDCUser OrElse _IsABRUser OrElse _IsAAUUser Then

                If Business.IsPartInBTOS_CTOSMaterialGroup(oPartNo) Then
                    'Dim pf As IBUS.iProdF = Pivot.FactProd
                    'Dim p As IBUS.iProd = pf.getProdByPartNo(oPartNo, MasterRef.org, _PlantID)
                    'If p.type = COMM.Fixer.eProdType.ItemBTO Then
                    addItemType = COMM.Fixer.eItemType.Parent
                    intParentLineNo = 0 : lineNo = 0
                    _IsAddBTOSPartNoDirectly = True
                End If
            End If
            If Not Business.ADD2QUOTE_V2_1(UID, oPartNo, oQty, ewFlag, addItemType, strCategoryName, 1, 1, Now, intParentLineNo, _PlantID, lineNo, strErr, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ) Then
                lbAddErrMsg.Text = strErr 'tbProdInfo.Visible = False : 
                Exit Sub
            End If
            'ICC 2015/2/16 Add product dependency. This currently for EU quote
            If _IsEUQuote Then
                Dim pDependencyList As New List(Of Advantech.Myadvantech.DataAccess.PRODUCT_DEPENDENCY)
                pDependencyList = Advantech.Myadvantech.Business.QuoteBusinessLogic.getProductDependencyByPartNo(oPartNo)
                If pDependencyList.Count > 0 Then
                    For Each p As Advantech.Myadvantech.DataAccess.PRODUCT_DEPENDENCY In pDependencyList
                        If Not Business.ADD2QUOTE_V2_1(UID, p.DEPENDENCY_PART_NO, oQty, ewFlag, addItemType, strCategoryName, 1, 1, Now, intParentLineNo, _PlantID, lineNo, strErr, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ) Then
                            lbAddErrMsg.Text = strErr 'tbProdInfo.Visible = False : 
                            Exit Sub
                        End If
                    Next
                End If
            End If
        End If
        'If lineNo <> 0 Then
        'Me.InitProdinfo(oPartNo, Horg.Value, _PlantID)
        'End If

        If Me.HisNeedSpADAM.Value = "Y" AndAlso Business.IsSpecialADAM(oPartNo) Then
            myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, lineNo), String.Format("ewFlag='99'"))
        End If
        'Ming 20150309: update Display LineNo, Display Qty and Display Unit Price
        If _IsTWOnline Or _IsCNAonlineUser Then
            myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, lineNo), String.Format("DisplayQty='{0}',DisplayLineNo='{1}'", oQty, lineNo))
            If IsNumeric(Me.txtUnitPriceAdd.Text.Trim) Then
                Dim unitPrice As Decimal = Me.txtUnitPriceAdd.Text.Trim()
                myQD.Update(String.Format("quoteId='{0}' and line_no={1}", UID, lineNo), String.Format("DisplayUnitPrice='{0}'", unitPrice))
            End If
        End If
        Me.txtPartNo.Text = "" : Me.txtQty.Text = 1 : Me.txtUnitPriceAdd.Text = ""
        If strErr <> "" Then
            Me.lbAddErrMsg.Text = "Warning: " & strErr
        End If
        Dim cF As IBUS.iCartF = Pivot.FactCart
        Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org)
        If lineNo > COMM.Fixer.StartLine Then
            Dim ewpl As Integer = 0
            If intParentLineNo > COMM.Fixer.StartLine Then
                lineNo = intParentLineNo
            End If
            Dim Sl As New SortedSet(Of Integer)
            Sl.Add(lineNo)
            cart.updateEW(Sl, Pivot.CurrentProfile.CurrDocReg, COMM.Fixer.eDocType.EQ, MasterRef.currency)

            'Frank 20140711: Hi Ming, below line is a temporary solution to fix a issue that new item cannot show up in the gridview after adding to a system which has extended warranty part
            MyUtil.Current.CurrentDataContext = Nothing

        End If

        'ICC 2015/4/20 Focus part no textbox when USOnline added part no successfully.
        If Me._IsUSUser Then txtPartNo.Focus()

        'Ryan 20180110 Show volume discount info for USAENC
        If Me._IsUSAENC OrElse Me._IsAAC Then
            Me.ascxUSVD.ShowData(oPartNo)
            Me.upUSVD.Update()
        End If

        'Frank 2012/08/24: Writing quotation update log
        Business.ADDQuotationUpdatelog(UID, Pivot.CurrentProfile.UserId & " added partno " & oPartNo)

        'Ming 20150514 如果使用过price grade，之后添加的产品继续用price grade获取price 
        If Trim(TBpriceGrade.Text).Length = 8 Then
            BTgetDiscountPrice_Click(Nothing, Nothing)
        Else
            initGV(UID)
        End If


        '/Ming20150304保留单个料号添加的逻辑
    End Sub

    Protected Sub btnDel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim apt As New EQDSTableAdapters.QuotationDetailTableAdapter
        Dim count1 As Integer = 0, count2 As Integer = 0, _removePartNo As String = ""
        Dim cF As IBUS.iCartF = Pivot.FactCart
        Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, lbSAPOrg.Text)
        Dim lines As New SortedSet(Of Integer)

        For i As Integer = 0 To gv1.Rows.Count - 1
            Dim chk As CheckBox = gv1.Rows(i).FindControl("chkKey"), _partno As HyperLink = gv1.Rows(i).FindControl("hlPartNo")
            If chk.Checked Then
                lines.Add(gv1.DataKeys(gv1.Rows(i).RowIndex).Value)
                _removePartNo += _partno.Text + ","
            End If
        Next
        If lines.Count > 0 Then
            'cart.RemoveAt(lines, COMM.Fixer.eDocType.EQ)
            '2013-10-08 Ming add deleting function  for combo order 
            For Each _lineno As Integer In lines
                MyQuoteX.DeleteQuoteItem(UID, _lineno)

                If _IsUsingBreakPoints Then

                    Dim _sql As New StringBuilder
                    _sql.AppendLine(" delete from QuotationDetail_BreakPoints ")
                    _sql.AppendLine(" where line_no= " & _lineno)
                    _sql.AppendLine(" and quoteid='" & UID & "'")
                    tbOPBase.dbExecuteNoQuery("EQ", _sql.ToString)

                End If

            Next
            MyQuoteX.ReSetLineNo(UID)
            'end
        End If

        'Frank 20180418
        If _IsABRUser Then
            QuoteBusinessLogic.UpdateSystemPriceForABRQuote(UID, ABRDiscount)
        End If


        'Frank 2012/08/24: Writing quotation update log
        Business.ADDQuotationUpdatelog(UID, Pivot.CurrentProfile.UserId & " deleted partno " & _removePartNo.Trim(","))
        Business.RefreshQuotationInventory(UID)
        initGV(UID)

        'ICC 2015/4/20 Set ErrMsg empty and refresh update panel (When a btos item has been deleted, parent item dropdownlist should be updated by update panel)
        lbAddErrMsg.Text = String.Empty
        UPForm.Update()

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        Dim cl As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, dt.org).GetListAll(COMM.Fixer.eDocType.EQ)
        If Pivot.NewObjPatch.isHasBto(cl) = 0 Then
            btnAdd.Visible = True
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        MyQuoteX.ReSetLineNo(UID)
        Business.RefreshQuotationInventory(UID)

        'Ryan 20170906 Save notes field per Elaine's request
        Me.BT_UpdateSalesNote_Click(Nothing, Nothing) : Me.BT_UpdateOrderNote_Click(Nothing, Nothing)

        'Frank 20180418
        If _IsABRUser Then
            QuoteBusinessLogic.UpdateSystemPriceForABRQuote(UID, ABRDiscount)
        End If

        initGV(UID) : Me.gv1.DataBind()
    End Sub

    Protected Sub ibtnSeqDown_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As LinkButton = CType(sender, LinkButton), line_no As Integer = obj.CommandName
        'myQD.exChangeLineNo(UID, line_no, line_no + 1) 
        MyQuoteX.UpOrDownLineNo(UID, line_no, "down")
        initGV(UID)
    End Sub

    Protected Sub ibtnSeqUp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As LinkButton = CType(sender, LinkButton)
        Dim line_no As Integer = obj.CommandName
        'myQD.exChangeLineNo(UID, line_no, line_no - 1)
        MyQuoteX.UpOrDownLineNo(UID, line_no, "up")
        initGV(UID)
    End Sub

    Protected Sub gv_drpEW_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As DropDownList = CType(sender, DropDownList), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value, month As Integer = obj.SelectedValue

        Dim dt As New EQDS.QuotationDetailDataTable
        Dim dta As New EQDSTableAdapters.QuotationDetailTableAdapter
        Dim a As String = Horg.Value
        Dim cF As IBUS.iCartF = Pivot.FactCart
        Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Horg.Value)
        If COMM.CartFixer.isValidParentLineNo(id) Then
            cart.UpdateByStr(String.Format("{0}={1}", cart.Item1.ewFlag.Name, month), String.Format("{0}='{1}' and ({2}={4} or {3}={4})", cart.Item1.key.Name, UID, cart.Item1.parentLineNo.Name, cart.Item1.lineNo.Name, id), COMM.Fixer.eDocType.EQ)
        Else
            cart.UpdateByLine(String.Format("{0}={1}", cart.Item1.ewFlag.Name, month), id, COMM.Fixer.eDocType.EQ)
        End If
        Dim s As New SortedSet(Of Integer)
        s.Add(id)
        cart.updateEW(s, MasterRef.DocReg, COMM.Fixer.eDocType.EQ, MasterRef.currency)

        initGV(UID)
    End Sub
    Dim EWU As IBUS.iEWUtil = Pivot.NewObjEWUtil
    Dim DTList As IBUS.iCartList = Nothing
    Private Sub gv1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv1.DataBinding
        If Not IsPostBack AndAlso Not Me.isRequestValid Then
            Exit Sub
        End If
        DTList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org).GetListAll(COMM.Fixer.eDocType.EQ)
        InitDrpParent(DTList)
    End Sub
    Private Sub gv1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv1.RowCreated
        Dim o As DropDownList = CType(e.Row.FindControl("gv_drpEW"), DropDownList)
        If Not IsNothing(o) Then
            Dim EWTypeL As List(Of IBUS.iEWTypeLine) = EWU.getListByReg(Pivot.CurrentProfile.CurrDocReg, 1)
            If Not IsNothing(EWTypeL) AndAlso EWTypeL.Count > 0 Then
                o.Items.Clear()
                o.Items.Add(New ListItem("Without EW", 0))
                For Each R As IBUS.iEWTypeLine In EWTypeL
                    o.Items.Add(New ListItem(R.Month & "months", R.Month))
                Next
            End If
        End If
    End Sub



    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv1.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(5).Visible = False

            'Rosane ABU sales is not allowed to change unit price
            If _IsABRUser Then
                'e.Row.Cells(11).Visible = False
                'e.Row.Cells(11).Text = "Tax"
            End If


        End If
        If _IsUSUser AndAlso e.Row.RowType <> DataControlRowType.EmptyDataRow Then
            e.Row.Cells(14).Visible = False
            If Role.IsMexicoAonlineSales Then
                e.Row.Cells(5).Visible = True
            End If


            'CType(e.Row.FindControl("lbHDueDate"), Label).Visible = False
            'CType(e.Row.FindControl("lbIDueDate"), Label).Visible = False
        End If


        If e.Row.RowType = DataControlRowType.Header Then
            If Me.HisFrCH.Value = "Y" Then
                CType(e.Row.FindControl("lbUnitPrice"), Label).Text = "Quotation Price" : CType(e.Row.FindControl("lbITP"), Label).Text = "Unit Price"
            ElseIf Role.IsCNAonlineSales Then
                CType(e.Row.FindControl("lbUnitPrice"), Label).Text = "Quotation Price" : CType(e.Row.FindControl("lbITP"), Label).Text = "Unit Price"
            Else
                CType(e.Row.FindControl("lbUnitPrice"), Label).Text = "Unit Price"
                'CType(e.Row.FindControl("lbITP"), Label).Text = "ITP(FOB AESC)"
                If _IsEUQuote Then
                    CType(e.Row.FindControl("lbITP"), Label).Text = "ITP(FOB AESC)"
                Else
                    CType(e.Row.FindControl("lbITP"), Label).Text = "ITP"
                End If
            End If
            If _IsABRUser Then
                CType(e.Row.FindControl("lbUnitPrice"), Label).Text = "Tax"
            End If
            If _IsJPOnline Then
                CType(e.Row.FindControl("lbITP"), Label).Text = "ITP(FOB AJP)"
            End If
            If _IsUSUser Then
                CType(e.Row.FindControl("lbHDueDate"), Label).Text = "Available Date"
                CType(e.Row.FindControl("lbITP"), Label).Text = "Cost"
            End If

            If _IsUsingBreakPoints Then
                If String.IsNullOrEmpty(_BreakPointstr) Then
                    CType(e.Row.FindControl("lbBreakPointScales"), Label).Text &= "N/A"
                Else
                    CType(e.Row.FindControl("lbBreakPointScales"), Label).Text &= _BreakPointstr.Replace(",", "-")
                End If

            End If

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim DBITEM As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim line_no As Integer = gv1.DataKeys(e.Row.RowIndex).Value



            Dim _CurrentQuoteItem As QuoteItem = MyQuoteX.GetQuoteItem(UID, line_no)
            If _CurrentQuoteItem Is Nothing Then
                'If _CurrentQuoteItem.partNo.StartsWith("AGS-CTOS-", StringComparison.CurrentCultureIgnoreCase) Then
                '    e.Row.Cells(0).Text = ""
                'End If
                'Dim cF As IBUS.iCartF = Pivot.FactCart
                Dim c As IBUS.iCart(Of IBUS.iCartLine) = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Me.Horg.Value)
                Dim cl As IBUS.iCartLine = c.Item(ID)

                Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Error by " + Pivot.CurrentProfile.UserId.ToString, "", "line_no: " + line_no.ToString() + vbNewLine + "QuoteID: " + UID, "")
                Exit Sub
            End If
            Dim part_no As String = DBITEM.Item("partNo").ToString
            Dim ListPice As Decimal = CDbl(CType(e.Row.FindControl("lbListPrice"), Label).Text)
            Dim UnitPrice As Decimal = CDbl(CType(e.Row.FindControl("txtUnitPrice"), TextBox).Text)
            Dim _PickImg As HtmlControl = CType(e.Row.FindControl("PickImg"), HtmlControls.HtmlControl)
            Dim itp As Decimal = DBITEM.Item("newITP").ToString
            Dim qty As Decimal = CInt(CType(e.Row.FindControl("txtGVQty"), TextBox).Text)

            Dim Discount As Decimal = 0.0, SubTotal As Decimal = 0.0, ewPrice As Decimal = 0.0
            Dim DrpEW As DropDownList = CType(e.Row.FindControl("gv_drpEW"), DropDownList)
            If Me.HisNeedSpADAM.Value = "Y" AndAlso Business.IsSpecialADAM(part_no) Or DBITEM.Item("ewflag") = 99 Then
                DrpEW.Items.Clear() : DrpEW.Items.Add(New ListItem("without EW", "0")) : DrpEW.Items.Add(New ListItem("36 months", "99"))
            End If
            If DBITEM.Item("ewflag") = 999 Then
                DrpEW.Items.Clear() : DrpEW.Items.Add(New ListItem("without EW", "0")) : DrpEW.Items.Add(New ListItem("3 months", "999"))
            End If
            DrpEW.SelectedValue = DBITEM.Item("ewFlag").ToString


            ewPrice = FormatNumber(Business.getRateByEWItem(Business.getEWItemByMonth(CInt(DrpEW.SelectedValue), MasterRef.DocReg), MasterRef.DocReg) * UnitPrice, 2)

            If ListPice = 0 Then
                'Nada modified for bug
                CType(e.Row.FindControl("lbListPrice"), Label).Text = "TBD" ': e.Row.Cells(11).Text = "TBD"
            Else
                Discount = (ListPice - UnitPrice) / ListPice
                If ListPice < UnitPrice Then
                    Discount = 0
                End If
                e.Row.Cells(12).Text = FormatNumber(Discount * 100, 2) & "%"

                If _IsKRAonlineUser Then
                    Dim _lbListPrice As Label = CType(e.Row.FindControl("lbListPrice"), Label)
                    _lbListPrice.Text = FormatNumber(_lbListPrice.Text, 0, , , TriState.True)
                End If

            End If
            SubTotal = FormatNumber(qty * UnitPrice, 2)
            CType(e.Row.FindControl("lbSubTotal"), Label).Text = SubTotal
            If UnitPrice <> 0 And itp <> 0 Then
                CType(e.Row.FindControl("lbMargin"), Label).Text = FormatNumber((UnitPrice - itp) * 100 / UnitPrice, 2) & "%"
            End If

            If Me._IsUSUser Then
                If CType(e.Row.FindControl("lbIDueDate"), Label).Text.Equals("12/31/9999") Then CType(e.Row.FindControl("lbIDueDate"), Label).Text = "TBD"
                CType(e.Row.FindControl("gv_drpDeliveryPlant"), DropDownList).SelectedValue = DBITEM.Item("deliveryPlant")

                If Role.IsAllowSeeCost(Pivot.CurrentProfile.UserId) Then '2015/2/13 Change function if user has permission to see cost column
                    e.Row.Cells(17).HorizontalAlign = HorizontalAlign.Right
                    If itp > 0 Then
                        e.Row.Cells(17).Text = Me.HCurrency.Value & " " & itp
                    Else
                        e.Row.Cells(17).Text = ""
                    End If
                End If
            End If

            'Frank 2015/04/24: For US, TW and CN AOnline sales, all lines are checked by default.
            If _IsUSUser OrElse _IsTWOnline OrElse _IsCNAonlineUser OrElse _IsKRAonlineUser _
                OrElse _IsHQDCUser OrElse _IsABRUser OrElse _IsAAUUser Then
                CType(e.Row.FindControl("chkKey"), CheckBox).Checked = False
            Else
                CType(e.Row.FindControl("chkKey"), CheckBox).Checked = True
            End If

            If _IsKRAonlineUser OrElse _IsHQDCUser Then
                CType(e.Row.FindControl("lnkView"), ImageButton).Visible = True
            End If

            'Frank 2015/03/05
            'Only ATW Aonline team can control display unit price/qty field
            If Me._IsTWOnline Or _IsCNAonlineUser Then
                CType(e.Row.FindControl("lnkView"), ImageButton).Visible = True
                CType(e.Row.FindControl("lbDisplayUnitPriceSplitSign"), Label).Visible = True
                CType(e.Row.FindControl("txtDisplayUnitPrice"), TextBox).Visible = True
                CType(e.Row.FindControl("lbDisplayQtySplitSign"), Label).Visible = True
                CType(e.Row.FindControl("txtDisplayQty"), TextBox).Visible = True
                CType(e.Row.FindControl("lbDisplayLinenoSplitSign"), Label).Visible = True
                CType(e.Row.FindControl("txtDisplayLineno"), TextBox).Visible = True
            End If

            'If e.Row.RowIndex = gv1.Rows.Count Then
            '    CType(e.Row.FindControl("ibtnSeqDown"), LinkButton).Visible = False
            'End If
            If e.Row.RowIndex = 0 Then
                CType(e.Row.FindControl("ibtnSeqUp"), LinkButton).Visible = False
            End If
            Try
                If _CurrentQuoteItem IsNot Nothing AndAlso _CurrentQuoteItem.IsEWpartnoX Then
                    e.Row.Cells(1).Text = ""
                End If
            Catch ex As Exception
                Dim ccc = 1
                'Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Error PartNO: " + _CurrentQuoteItem.partNo, "", "line_no: " + line_no.ToString() + vbNewLine + "QuoteID: " + UID, "")
            End Try
            If DBITEM.Item("itemType") = COMM.Fixer.eItemType.Parent Then

                e.Row.Cells(1).Text = "" : e.Row.Cells(3).Text = "" 'e.Row.Cells(5).Text = "" ' : e.Row.Cells(6).Text = ""

                'Frank 2012/09/04:cells(5) visible control====
                If (Not _IsTWOnline) And (Not _IsCNAonlineUser) And (Not _IsHQDCUser) And (Not _IsKRAonlineUser) Then
                    CType(e.Row.FindControl("txtDescription"), TextBox).Visible = False
                End If
                If _IsUSUser Then
                    CType(e.Row.FindControl("BT_PickCTOAssemblyInstructionDoc"), Button).Visible = True
                End If
                'cells(5) visible control end====

                e.Row.Cells(8).Text = ""  'e.Row.Cells(8).Text = ""

                'e.Row.Cells(10).Text = ""
                'e.Row.Cells(12).Text = "" : e.Row.Cells(13).Text = ""
                'e.Row.Cells(14).Text = ""
                'e.Row.Cells(15).Text = ""
                e.Row.Cells(17).Text = ""
                If Not Me._IsUSUser Then
                    CType(e.Row.FindControl("lbMargin"), Label).Text = Business.getMargin(UID) & "%"
                End If
                Dim _lbSubTotal As Label = CType(e.Row.FindControl("lbSubTotal"), Label)
                Dim _txtUnitPrice As TextBox = CType(e.Row.FindControl("txtUnitPrice"), TextBox)
                Dim _lbListPrice As Label = CType(e.Row.FindControl("lbListPrice"), Label)
                'e.Row.Cells(10).Text = FormatNumber(Business.getTotalListPrice(CType(gv1.DataSource, DataTable)), 2)
                Dim DTL As IBUS.iCartList = DTList.Merge2NewCartList(DTList.Where(Function(X) X.parentLineNo.Value = DBITEM.Item("line_No")))
                Dim _SubTotal As Decimal = 0, _ListPriceTotalAmount As Decimal = 0

                If Not IsNothing(DTL) AndAlso DTL.Count > 0 Then
                    _SubTotal = DTL.getTotalAmount()
                    _ListPriceTotalAmount = DTL.getListPriceTotalAmount()
                End If

                If _IsABRUser Then
                    _SubTotal = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetABRQuoteItemSubTotalWithTax(UID, line_no)
                End If

                'Frank 2015/02/09
                '產生給客戶看的報價單內的組裝單要可以輸入一個總價
                If _IsTWOnline Or _IsCNAonlineUser Then
                    '_txtUnitPrice.Enabled = True
                    _txtUnitPrice.Enabled = False
                    'If _CurrentQuoteItem.newUnitPrice > 0 Then
                    '    'Frank 2015/02/13用MyQuoteX抓出的資料好像不會立即更新qty textchaged事件中的sql update，所以先不要用_CurrentQuoteItem.qty
                    '    '_SubTotal = _CurrentQuoteItem.newUnitPrice * _CurrentQuoteItem.qty
                    '    _SubTotal = _CurrentQuoteItem.newUnitPrice * qty
                    'End If
                Else
                    _txtUnitPrice.Enabled = False
                End If



                _lbSubTotal.Text = _SubTotal
                _txtUnitPrice.Text = FormatNumber(_SubTotal / qty, 2)


                _lbListPrice.Text = FormatNumber(_ListPriceTotalAmount / qty, 2)
                If (_IsJPOnline OrElse _IsKRAonlineUser) AndAlso Decimal.TryParse(_lbListPrice.Text, 0) Then
                    _lbListPrice.Text = FormatNumber(_lbListPrice.Text, 0, , , TriState.True)
                End If
                If IsDBNull(DBITEM.Item("RECFIGID")) OrElse String.IsNullOrEmpty(DBITEM.Item("RECFIGID")) Then
                    CType(e.Row.FindControl("hlReconfigure"), HyperLink).Visible = False
                End If

                'Ryan 20160427 If Part is defined in CBOM_WithoutEW, set EW drp visible to false.
                If Advantech.Myadvantech.Business.PartBusinessLogic.IsNoEWParts(part_no, True) Then
                    If DrpEW IsNot Nothing Then DrpEW.Enabled = False
                End If
            Else
                'Frank 2013/10/25
                'If not BTOS parent item then hide reconfigure link
                CType(e.Row.FindControl("hlReconfigure"), HyperLink).Visible = False
            End If

            'Frank 2013/04/22
            '======Format price as xxx,xxx and no decimal for AJP
            If _IsJPOnline OrElse _IsKRAonlineUser Then
                CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(CType(e.Row.FindControl("lbSubTotal"), Label).Text, 0, , , TriState.True)
                CType(e.Row.FindControl("txtUnitPrice"), TextBox).Text = FormatNumber(CType(e.Row.FindControl("txtUnitPrice"), TextBox).Text, 0, , , TriState.True)
                'DBITEM.Item("listprice")
                'If DBITEM.Item("listprice") IsNot Nothing AndAlso e.Row.FindControl("lbListPrice") IsNot Nothing Then
                '    CType(e.Row.FindControl("lbListPrice"), Label).Text = FormatNumber(DBITEM.Item("listprice"), 0, , , TriState.True)
                'End If

                If e.Row.FindControl("txtItp") IsNot Nothing Then
                    CType(e.Row.FindControl("txtItp"), TextBox).Text = FormatNumber(CType(e.Row.FindControl("txtItp"), TextBox).Text, 0, , , TriState.True)
                End If
            End If
            '=====Format price End================================

            'If DBITEM.Item("partNo").ToString.ToLower.Contains("ags-ew") Then
            'e.Row.Cells(0).Text = ""
            If part_no.ToLower.StartsWith("ags-ew") OrElse part_no.ToLower.StartsWith("ags-ctos-") Then 'AGS-CTOS-
                ' e.Row.Cells(0).Text = ""
                If DBITEM.Item("partNo").ToString.ToLower.Contains("ags-ew") Then e.Row.Cells(0).Text = ""
                CType(e.Row.FindControl("txtUnitPrice"), TextBox).Enabled = False
                CType(e.Row.FindControl("txtGVQty"), TextBox).Enabled = False
                If Not IsNothing(e.Row.FindControl("btnSpecialItp")) Then
                    'Frank and AEU Irene 20160907 : postpone to phase 2
                    'Ryan 20160907 Per Ozdal's request and Ruud has confirmed, exclude EU from disable btnITP logic.
                    'If Not _IsEUQuote Then
                    CType(e.Row.FindControl("btnSpecialItp"), Button).Enabled = False
                    'End If
                End If
                CType(e.Row.FindControl("gv_drpEW"), DropDownList).Visible = False ': CType(e.Row.FindControl("txtGVQty"), TextBox).Enabled = False
                If part_no.ToLower.StartsWith("ags-ctos-") AndAlso _IsTWOnline Then
                    CType(e.Row.FindControl("txtGVQty"), TextBox).Enabled = True
                End If

                If part_no.ToUpper.StartsWith("AGS-CTOS-SYS-") AndAlso _IsJPOnline Then
                    CType(e.Row.FindControl("txtUnitPrice"), TextBox).Enabled = True
                End If
            End If
            If _IsEUQuote AndAlso part_no.ToLower.StartsWith("ags-ctos-", StringComparison.CurrentCultureIgnoreCase) Then
                e.Row.Cells(0).Text = ""
            End If
            'If myQD.isBtoChildItem(UID, line_no) = 1 Then
            If DBITEM.Item("HigherLevel") > COMM.CartFixer.StartLine Then
                CType(e.Row.FindControl("gv_drpEW"), DropDownList).Visible = False ': CType(e.Row.FindControl("txtGVQty"), TextBox).Enabled = False
            End If

            'Frank 20140715: I really don't understand why "If True Then"..... , so I comment out below 3 lines
            'If True Then
            '    CType(e.Row.FindControl("txtGVQty"), TextBox).Enabled = True
            'End If
            Dim ORG As String = MasterRef.org

            'Frank 20150515: Do not need to display error message for non-orderable part for both ATW and ACN Team
            If Not _IsTWOnline AndAlso Not _IsCNAonlineUser AndAlso Not _IsHQDCUser AndAlso Not _IsKRAonlineUser Then
                If DBITEM.Item("itemType") <> COMM.Fixer.eItemType.Parent And (Not DBITEM.Item("partNo").ToString.ToLower.StartsWith(COMM.Fixer.AGSEWPrefix.ToLower)) Then
                    Dim strStatusCode As String = "", strStatusDesc As String = ""
                    If DBITEM.Item("isPhaseout") Then
                        CType(e.Row.FindControl("txtDescription"), TextBox).Text = String.Format("Status:{0} ({1})", DBITEM.Item("StatusCode"), DBITEM.Item("StatusDesc"))
                        CType(e.Row.FindControl("txtDescription"), TextBox).ForeColor = Drawing.Color.Red
                        IsHaveInvalid = True
                    End If

                    ''20160701 Julia.Wong : Line item unit price & ITP is missing, please block it from eQ
                    ''Ryan 20161206 Set MEMO- & Option- items as exception
                    'If _IsHQDCUser AndAlso (DBITEM.Item("UnitPrice") = 0 OrElse DBITEM.Item("ITP") = 0) AndAlso (Not part_no.ToLower.StartsWith("ags-ctos-", StringComparison.CurrentCultureIgnoreCase)) _
                    '    AndAlso (Not part_no.ToLower.StartsWith("MEMO", StringComparison.CurrentCultureIgnoreCase)) _
                    '    AndAlso (Not part_no.ToLower.StartsWith("Option", StringComparison.CurrentCultureIgnoreCase)) _
                    '    AndAlso (Not part_no.ToLower.StartsWith("NRE", StringComparison.CurrentCultureIgnoreCase)) Then
                    '    CType(e.Row.FindControl("txtDescription"), TextBox).Text = "Zero Price" 'String.Format("Status:{0} ({1})", DBITEM.Item("StatusCode"), DBITEM.Item("StatusDesc"))
                    '    CType(e.Row.FindControl("txtDescription"), TextBox).ForeColor = Drawing.Color.Red
                    '    IsHaveInvalid = True
                    'End If

                End If
            End If


            If _IsHQDCUser Then
                If DBITEM.Item("itemType") <> COMM.Fixer.eItemType.Parent And (Not DBITEM.Item("partNo").ToString.ToLower.StartsWith(COMM.Fixer.AGSEWPrefix.ToLower)) Then


                    Dim strStatusCode As String = "", strStatusDesc As String = ""
                    If DBITEM.Item("StatusCode") = "I" Then
                        CType(e.Row.FindControl("txtDescription"), TextBox).Text = String.Format("Status:{0} ({1})", DBITEM.Item("StatusCode"), DBITEM.Item("StatusDesc"))
                        CType(e.Row.FindControl("txtDescription"), TextBox).ForeColor = Drawing.Color.Red
                        IsHaveInvalid = True
                    End If


                    If (DBITEM.Item("UnitPrice") = 0 OrElse DBITEM.Item("ITP") = 0) _
                        AndAlso (Not part_no.ToLower.StartsWith("ags-ctos-", StringComparison.CurrentCultureIgnoreCase)) _
                        AndAlso (Not part_no.ToLower.StartsWith("MEMO", StringComparison.CurrentCultureIgnoreCase)) _
                        AndAlso (Not part_no.ToLower.StartsWith("Option", StringComparison.CurrentCultureIgnoreCase)) _
                        AndAlso (Not part_no.ToLower.Equals("FREIGHT_FEE", StringComparison.CurrentCultureIgnoreCase)) _
                        AndAlso (Not part_no.ToLower.StartsWith("NRE", StringComparison.CurrentCultureIgnoreCase)) Then

                        Dim _reason As String = String.Empty
                        If DBITEM.Item("UnitPrice") = 0 Then
                            _reason = "Unit Price"
                        End If
                        If DBITEM.Item("ITP") = 0 Then
                            If String.IsNullOrEmpty(_reason) Then
                                _reason = "ITP"
                            Else
                                _reason = _reason & ",ITP"
                            End If

                        End If

                        CType(e.Row.FindControl("txtDescription"), TextBox).Text = "Zero Price(" & _reason & ")"

                        CType(e.Row.FindControl("txtDescription"), TextBox).ForeColor = Drawing.Color.Red
                        IsHaveInvalid = True

                    End If

                End If
            End If



            If Me.HisFrCH.Value = "Y" OrElse _IsJPOnline Then
                If Not IsNothing(e.Row.FindControl("btnSpecialItp")) Then
                    CType(e.Row.FindControl("btnSpecialItp"), Button).Visible = False
                End If
            End If
            Dim o As TextBox = CType(e.Row.FindControl("txtDescription"), TextBox)
            If Not DBITEM.Item("partNo").ToString.ToUpper.StartsWith("OPTION") And Not DBITEM.Item("partNo").ToString.ToUpper = "MEMO" And Not DBITEM.Item("partNo").ToString.ToUpper = "SPECIAL" Then
                If o IsNot Nothing Then
                    o.ReadOnly = True : o.BackColor = System.Drawing.ColorTranslator.FromHtml("#ebebeb")
                End If
            Else
                Dim ib As ImageButton = CType(e.Row.FindControl("lnkView"), ImageButton)
                If ib IsNot Nothing Then ib.Visible = True
                If o IsNot Nothing Then o.BackColor = System.Drawing.ColorTranslator.FromHtml("#ebebeb")
            End If
            'ICC 2016/3/1 Mark this code
            'If _IsTWOnline Or _IsJPOnline Or _IsCNAonlineUser Or _IsKRAonlineUser Then
            '    If o IsNot Nothing Then
            '        o.ReadOnly = False
            '    End If
            'End If
            If _CurrentQuoteItem.IsEWpartnoX OrElse (_CurrentQuoteItem.ItemTypeX = QuoteItemType.BtosParent AndAlso Not _IsAddMultiParts) Then
                _PickImg.Visible = False
            End If
            'Ming20141110      Disable the price edit function for all IMG-XXXXX part numbers  ,	Disable adding an extended warranty option for below product lines
            If _IsEUQuote Then
                If Not SAPDAL.CommonLogic.isAllowedChangePrice(_CurrentQuoteItem.partNo, "EU10") Then
                    Dim TB As TextBox = CType(e.Row.FindControl("txtUnitPrice"), TextBox)
                    If TB IsNot Nothing Then TB.Enabled = False
                    'Frank and AEU Irene 20160907 : postpone to phase 2
                    'Ozdal 20160907  unblock special ITP button so when sales add SPR no with special service price
                    Dim btSpecITP As Button = CType(e.Row.FindControl("btnSpecialItp"), Button)
                    If btSpecITP IsNot Nothing Then btSpecITP.Enabled = False
                End If
                If _CurrentQuoteItem.ItemTypeX = QuoteItemType.Part AndAlso (Not SAPDAL.CommonLogic.isAllowedAddEW(_CurrentQuoteItem.partNo, "", "EU10")) Then
                    DrpEW.Visible = False
                End If
            End If
            If _IsHQDCUser Then
                If _CurrentQuoteItem.sprNo IsNot Nothing AndAlso Not String.IsNullOrEmpty(_CurrentQuoteItem.sprNo) Then
                    CType(e.Row.FindControl("txtUnitPrice"), TextBox).Enabled = False
                End If
            End If

            If _IsUsingBreakPoints Then
                If Not String.IsNullOrEmpty(Me._BreakPointstr) AndAlso
                    Me._BreakPointDT IsNot Nothing AndAlso
                    Me._BreakPointDT.Rows.Count > 0 Then

                    Dim _foundrows() As DataRow = Me._BreakPointDT.Select("line_No=" & line_no)
                    Dim _substr As String = String.Empty
                    For Each _founditem As DataRow In _foundrows
                        _substr &= _founditem.Item("CustomerQuantityUnitPrice") & "-"
                    Next
                    CType(e.Row.FindControl("lbBreakPointsPrice"), Label).Text = _substr.TrimEnd("-")
                End If
            End If

            'Ryan 20160802 Block several objects' visibility while quote is in GP Process.
            If _IsInGPProcess Then
                _PickImg.Visible = False
                CType(e.Row.FindControl("txtUnitPrice"), TextBox).Enabled = False
            End If

            If _IsABRUser Then
                If DBITEM.Item("itemType") = COMM.Fixer.eItemType.Parent OrElse line_no < 100 Then

                    Dim QD_Ext_ABR As Advantech.Myadvantech.DataAccess.QuotationDetail_Extension_ABR = _lQuotationDetail_Ext_ABR.Where(Function(p) p.line_No = line_no).FirstOrDefault()

                    If QD_Ext_ABR IsNot Nothing Then
                        e.Row.Cells(11).Text = "ICMS: " & MasterRef.currency & " " & FormatNumber(QD_Ext_ABR.BX13, 2).ToString()
                        e.Row.Cells(11).Text &= "<br/>" & "ICMS (Orig): " & MasterRef.currency & " " & FormatNumber(QD_Ext_ABR.BX94, 2).ToString()
                        e.Row.Cells(11).Text &= "<br/>" & "ICMS (Dest): " & MasterRef.currency & " " & FormatNumber(QD_Ext_ABR.BX95, 2).ToString()
                        e.Row.Cells(11).Text &= "<br/>" & "ICMS (Spec): " & MasterRef.currency & " " & FormatNumber(QD_Ext_ABR.BX96, 2).ToString()
                        e.Row.Cells(11).Text &= "<br/>" & "IPI: " & MasterRef.currency & " " & FormatNumber(QD_Ext_ABR.BX23, 2).ToString()
                        e.Row.Cells(11).Text &= "<br/>" & "PIS: " & MasterRef.currency & " " & FormatNumber(QD_Ext_ABR.BX82, 2).ToString()
                        e.Row.Cells(11).Text &= "<br/>" & "COFINS: " & MasterRef.currency & " " & FormatNumber(QD_Ext_ABR.BX72, 2).ToString()
                        Dim _st As Decimal = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetABRQuoteItemSubTotalWithTax(UID, line_no)
                        CType(e.Row.FindControl("lbSubTotal"), Label).Text = _st.ToString("N2")
                    End If
                Else
                    e.Row.Cells(11).Text = ""
                End If
            End If

        End If
        If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(22).Visible = False
            'If Business.isBtoOrder(CType(gv1.DataSource, DataTable)) = 0 Then
            '    e.Row.Cells(3).Visible = False
            'End If
            'If Business.isBtoOrder(CType(gv1.DataSource, DataTable)) = 1 Then
            '    e.Row.Cells(7).Visible = False
            'End If
            If _IsTWOnline Or _IsCNAonlineUser Then
                e.Row.Cells(3).Visible = False
            End If
            e.Row.Cells(8).Visible = False : e.Row.Cells(9).Visible = False
            If Me.HisFrCH.Value = "Y" Then
                e.Row.Cells(19).Visible = False
            End If

            'Discount by qty scales
            If _IsUsingBreakPoints Then
                e.Row.Cells(22).Visible = True
            End If

            If Me._IsUSUser Then
                If Role.IsAllowSeeCost(Pivot.CurrentProfile.UserId) Then '2015/2/13 Change function if user has permission to see cost column 
                    e.Row.Cells(17).Visible = True
                Else
                    e.Row.Cells(17).Visible = False
                End If

                e.Row.Cells(18).Visible = False : e.Row.Cells(19).Visible = False
            Else
                e.Row.Cells(21).Visible = False
            End If

            If Me._IsKRAonlineUser Then
                'Frank 20170623
                'Release virtual part number for AKR
                e.Row.Cells(5).Visible = True

                e.Row.Cells(14).Visible = False
                e.Row.Cells(17).Visible = False : e.Row.Cells(18).Visible = False
                e.Row.Cells(19).Visible = False : e.Row.Cells(21).Visible = False
            End If

            If Me._IsHQDCUser Then
                e.Row.Cells(14).Visible = False
                'e.Row.Cells(17).Visible = True
                e.Row.Cells(18).Visible = True
                e.Row.Cells(19).Visible = False : e.Row.Cells(21).Visible = False
            End If

            If _IsABRUser OrElse _IsAAUUser Then
                e.Row.Cells(7).Visible = False

                e.Row.Cells(14).Visible = False
                e.Row.Cells(17).Visible = False : e.Row.Cells(18).Visible = False
                e.Row.Cells(19).Visible = False : e.Row.Cells(21).Visible = False
            End If


            If Me._IsTWOnline Or _IsCNAonlineUser Then
                e.Row.Cells(5).Visible = True : e.Row.Cells(14).Visible = False
                e.Row.Cells(17).Visible = False : e.Row.Cells(18).Visible = False : e.Row.Cells(19).Visible = False
            End If

            'If Role.IsEUSales() OrElse Me._IsTWOnline Or _IsCNAonlineUser Then
            If _IsEUQuote OrElse _IsHQDCUser Then

                'Frank 2013/07/15: (Emil)Disable ITP visibility, but still keep GP approval flow.
                If e.Row.RowType = DataControlRowType.DataRow Then
                    If String.IsNullOrEmpty(CType(e.Row.FindControl("lbSPRNO"), Label).Text) Then
                        Dim _lbItpSign As Label = CType(e.Row.FindControl("lbItpSign"), Label)
                        If _lbItpSign IsNot Nothing Then _lbItpSign.Visible = False

                        Dim _txtItp As TextBox = CType(e.Row.FindControl("txtItp"), TextBox)
                        If _txtItp IsNot Nothing Then _txtItp.Visible = False

                    End If

                End If

            End If

            If _IsEUQuote Then
                'Frank 2013/08/05: Offer virtual part number function to AEU user.
                e.Row.Cells(5).Visible = True

                'Frank 2013/07/30: (Emil)Please also hide the “Margin” column, if we keep it, sales can easily reverse to get the ITP.
                e.Row.Cells(18).Visible = False

            End If


            If _IsJPOnline Then
                e.Row.Cells(3).Visible = False
                'Frank 2013/09/06: Offer virtual part number function to AJP user.
                e.Row.Cells(5).Visible = True
            End If
        End If
    End Sub

    Public Function TransferLocalTime(ByVal _datetime As DateTime) As String

        'If CDate(Eval("duedate")).ToString("yyyy/MM/dd") = "1900/01/01" Then Return "TBD"
        If _datetime.ToString(Pivot.CurrentProfile.DatePresentationFormat) = CDate("1900/01/01").ToString(Pivot.CurrentProfile.DatePresentationFormat) Then Return "TBD"
        'Frank:20131003
        If _datetime.ToString(Pivot.CurrentProfile.DatePresentationFormat) = CDate("9999/12/31").ToString(Pivot.CurrentProfile.DatePresentationFormat) Then Return "TBD"

        If Me._IsUSUser Then Return Util.GetLocalTime("AUS", _datetime).ToString(Pivot.CurrentProfile.DatePresentationFormat)

        Return _datetime.ToString(Pivot.CurrentProfile.DatePresentationFormat)

    End Function

    Protected Sub gv_drpDeliveryPlant_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As DropDownList = CType(sender, DropDownList), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim _lineno As Integer = Me.gv1.DataKeys(row.RowIndex).Value, _NewDeliveryPlant As String = obj.SelectedValue

        Dim dta As New EQDSTableAdapters.QuotationDetailTableAdapter
        dta.UpdateDeliveryPlantByLine(_NewDeliveryPlant, UID, _lineno)
        'Business.RefreshPartInventory(UID, _lineno, COMM.Fixer.eDocType.EQ)
        MyQuoteX.RefreshPartInventory(UID, _lineno)
        initGV(UID)
    End Sub
    Protected Sub txtDisplayLineno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me._IsTWOnline Or _IsCNAonlineUser Then
            Dim obj As TextBox = CType(sender, TextBox), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
            Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value, DisplayQty As String = obj.Text

            Dim cF As IBUS.iCartF = Pivot.FactCart
            Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Horg.Value)
            cart.UpdateByLine(String.Format("{0}=N'{1}'", "DisplayLineno", DisplayQty), id, COMM.Fixer.eDocType.EQ)
        End If
    End Sub
    Protected Sub txtDisplayQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me._IsTWOnline Or _IsCNAonlineUser Then
            Dim obj As TextBox = CType(sender, TextBox), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
            Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value, DisplayQty As String = obj.Text

            Dim cF As IBUS.iCartF = Pivot.FactCart
            Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Horg.Value)
            cart.UpdateByLine(String.Format("{0}='{1}'", "DisplayQty", DisplayQty), id, COMM.Fixer.eDocType.EQ)

            'Frank 2012/08/24: Writing quotation update log
            Business.ADDQuotationUpdatelog(UID, Pivot.CurrentProfile.UserId & " updated display qty to " & DisplayQty & " for line " & id)

        End If
    End Sub

    Protected Sub txtDisplayUnitPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me._IsTWOnline Or _IsCNAonlineUser Then
            Dim obj As TextBox = CType(sender, TextBox), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
            Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value, DisplayUnitPrice As String = obj.Text

            Dim cF As IBUS.iCartF = Pivot.FactCart
            Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Horg.Value)
            cart.UpdateByLine(String.Format("{0}='{1}'", "DisplayUnitPrice", DisplayUnitPrice), id, COMM.Fixer.eDocType.EQ)

            'Frank 2012/08/24: Writing quotation update log
            Business.ADDQuotationUpdatelog(UID, Pivot.CurrentProfile.UserId & " updated display unit price to " & DisplayUnitPrice & " for line " & id)

        End If
    End Sub

    Protected Sub txtUnitPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As TextBox = CType(sender, TextBox), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value, UnitPrice As String = obj.Text, _doublevalue As Double
        'Frank 2012/08/22 if new unit price is invalid double ,then do not update 
        If Not Double.TryParse(UnitPrice, _doublevalue) Then Exit Sub
        'Ryan 20170321 Allow AJP users update price as 0
        If _doublevalue = 0 AndAlso Not _IsJPOnline Then Exit Sub
        Dim cF As IBUS.iCartF = Pivot.FactCart
        Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Horg.Value)

        If Not COMM.CartFixer.isValidParentLineNo(id) Then
            '20120912 TC: When updating unit price, it should also pass US GP control
            If Me._IsUSUser Then
                Dim aptQD As New EQDSTableAdapters.QuotationDetailTableAdapter, decGPPercentage As Decimal = -1
                Dim dtQM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
                If Not IsNothing(dtQM) Then
                    Dim dtQMLine As EQDS.QuotationDetailDataTable = aptQD.GetSpecificLinePN(dtQM.Key, id)
                    If dtQMLine.Count = 1 Then
                        If String.Equals(dtQM.AccErpId, "UEPP5001") Then dtQM.DIVISION = "20"
                        Dim getPriceWarning As String = ""
                        Dim sListPrice As String = CType(row.FindControl("lbListPrice"), Label).Text
                        Dim ListPice As Decimal = 0
                        If sListPrice = "TBD" Then
                            ListPice = 0
                        Else
                            Decimal.TryParse(CType(row.FindControl("lbListPrice"), Label).Text, ListPice)
                        End If

                        'If Not _IsAAC AndAlso Not (UnitPrice >= ListPice Or Not Business.isANAPnBelowGPV2(MasterRef.AccErpId, dtQMLine(0).partNo, UnitPrice, decGPPercentage, getPriceWarning)) Then
                        'If (Not COMM.Util.IsTesting) AndAlso (Not _IsUSAOnlineEP AndAlso Role.Is2ANAOnlineTeamsByOfficeCode()) _
                        'If (Not _IsUSAOnlineEP AndAlso Role.Is2ANAOnlineTeamsByOfficeCode()) _
                        'If Role.IsANAAOnlineTeamsByOfficeCode() _
                        If Me._IsUSAOnlineEP _
                            AndAlso Not (UnitPrice >= ListPice Or Not Business.isANAPnBelowGPV2(MasterRef.AccErpId, dtQMLine(0).partNo, UnitPrice, decGPPercentage, getPriceWarning)) Then
                            'lbAddErrMsg.Text = "Item " + dtQMLine(0).partNo + " Lin is below GP (" & FormatNumber((decGPPercentage * 100), 0).ToString() & "%)"
                            lbAddErrMsg.Text = String.Format("Item {0} Line {1} is below GP Price({2})", dtQMLine(0).partNo, dtQMLine(0).line_No, FormatNumber(IIf(decGPPercentage > ListPice, ListPice, decGPPercentage), 2))
                            UPForm.Update() 'ICC 2015/4/17 Becasue error message won't show in page (AsyncPostBackTrigger), so update updatepanel manually
                            Exit Sub
                        Else
                            lbAddErrMsg.Text = ""
                            UPForm.Update() 'ICC 2015/4/17 Becasue error message won't show in page (AsyncPostBackTrigger), so update updatepanel manually
                        End If
                    End If

                End If

            End If
            cart.UpdateByLine(String.Format("{0}='{1}'", cart.Item1.newunitPrice.Name, UnitPrice), id, COMM.Fixer.eDocType.EQ)
            ' myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, id), String.Format("newunitPrice='{0}'", UnitPrice))
            Dim sl As New SortedSet(Of Integer)
            sl.Add(id)
            cart.updateEW(sl, Pivot.CurrentProfile.CurrDocReg, COMM.Fixer.eDocType.EQ, MasterRef.currency)


            'Frank 2012/08/24: Writing quotation update log
            Business.ADDQuotationUpdatelog(UID, Pivot.CurrentProfile.UserId & " updated unit price to " & UnitPrice & " for line " & id)

        Else
            ''Frank 2015/02/09
            ''產生給客戶看的報價單內的組裝單要可以輸入一個總價
            'If Me._IsTWOnline Then
            '    Dim _ParentItem As QuoteItem = MyQuoteX.GetQuoteItem(UID, id)
            '    Dim _NewUnitPrice As Decimal = 0
            '    If Decimal.TryParse(UnitPrice, _NewUnitPrice) Then
            '        If _NewUnitPrice = _ParentItem.UnitPriceX Then
            '            _ParentItem.newUnitPrice = 0
            '        Else
            '            _ParentItem.newUnitPrice = _NewUnitPrice
            '        End If
            '        MyUtil.Current.CurrentDataContext.SubmitChanges()
            '    End If
            'End If
        End If
    End Sub
    'ICC 2016/3/1 Mark this code
    'Protected Sub txtDescription_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim obj As TextBox = CType(sender, TextBox), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
    '    Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value
    '    Dim D As String = Util.ReplaceSQLChar(obj.Text)

    '    'Frank 2015-03-20 ATW is allowed to change description of BTOS parent item
    '    If _IsTWOnline Or _IsCNAonlineUser Then
    '        myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, id), String.Format("description=N'{0}'", D))
    '    Else
    '        If id <> 100 Then
    '            myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, id), String.Format("description=N'{0}'", D))
    '        End If
    '    End If

    'End Sub
    Protected Sub txtcustMaterial_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As TextBox = CType(sender, TextBox), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value
        Dim D As String = Util.ReplaceSQLChar(obj.Text)
        ' If id <> 100 Then
        myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, id), String.Format("VirtualPartNo=N'{0}'", D))
        ' End If
    End Sub
    Protected Sub txtGVQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim obj As TextBox = CType(sender, TextBox), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value
        Dim Qty As String = obj.Text, _intvalue As Integer = 0

        Dim cF As IBUS.iCartF = Pivot.FactCart
        Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Horg.Value)

        If Not Integer.TryParse(Qty, _intvalue) Then Exit Sub
        If _intvalue < 1 Then Exit Sub

        Dim myQDA As New EQDSTableAdapters.QuotationDetailTableAdapter
        If COMM.CartFixer.isValidParentLineNo(id) Then
            Dim PQty As Integer = 0
            Dim myQD As EQDS.QuotationDetailDataTable
            myQD = myQDA.GetSpecificLinePN(UID, id)
            Dim myQDRow As EQDS.QuotationDetailRow = myQD(0)
            PQty = myQDRow.qty

            If PQty <> 0 Then
                cart.UpdateByStr(String.Format("{2}=convert(decimal(10,2),{2})/{0} * {1}", PQty, Qty, cart.Item1.Qty.Name), String.Format("{2}='{0}' and ({3}={1} or {4}={1})", UID, id, cart.Item1.key.Name, cart.Item1.parentLineNo.Name, cart.Item1.lineNo.Name), COMM.Fixer.eDocType.EQ)
            End If
            '/ReCalDueDateForEachLine/'
        ElseIf _IsUSUser And COMM.CartFixer.getParentLineNoFromLineNo(id) > COMM.Fixer.StartLine Then
            Dim myQD As EQDS.QuotationDetailDataTable
            myQD = myQDA.GetSpecificLinePN(UID, id)
            myQDA.UpdateQtyByLine(Qty, UID, id)
            ReCalDue(UID, id)
            If Business.IsPTD(myQD(0).partNo) Then
            Else
                Util.showMessage("Please be informed that the configuration system may not be assembled if you change the non-PTrade component's quantity.")
            End If
            cart.UpdateByLine(String.Format("{0}={1}", cart.Item1.Qty.Name, Qty), id, COMM.Fixer.eDocType.EQ)
            ReCalDue(UID, id)

        Else
            cart.UpdateByLine(String.Format("{0}={1}", cart.Item1.Qty.Name, Qty), id, COMM.Fixer.eDocType.EQ)
            ReCalDue(UID, id)

        End If
        Dim SL As New SortedSet(Of Integer)
        SL.Add(id)
        cart.updateEW(SL, Pivot.CurrentProfile.CurrDocReg, COMM.Fixer.eDocType.EQ, MasterRef.currency)

        'Frank 2012/08/24: Writing quotation update log
        Business.ADDQuotationUpdatelog(UID, Pivot.CurrentProfile.UserId & " updated qty to " & Qty & " for line " & id)
    End Sub

    Sub ReCalDue(ByVal quoteId As String, ByVal line_no As String)
        Dim dt As DataTable = myQD.GetDT(String.Format("quoteId='{0}' and Line_no='{1}'", quoteId, line_no), "") 'CType(Page.Items("D"), DataTable).Select(String.Format("line_no={0}", line_no))
        If dt.Rows.Count = 1 Then
            Dim part_no As String = dt(0).Item("partNo"), plant As String = dt(0).Item("deliveryPlant")
            Dim qty As String = dt(0).Item("qty"), req_date As String = dt(0).Item("reqDate")
            Dim duedate As String = "", inventory As Integer = 0, satisflag As Integer = 0, qtyCanbeConfirmed As Integer = 0
            SAPTools.getInventoryAndATPTable(part_no, plant, qty, duedate, inventory, New DataTable, req_date, satisflag, qtyCanbeConfirmed)
            myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", quoteId, line_no), String.Format("duedate='{0}',inventory='{1}',SatisfyFlag='{2}',CanbeConfirmed='{3}'", duedate, inventory, satisflag, qtyCanbeConfirmed))
        End If
    End Sub

    Protected Sub txtreqdate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As TextBox = CType(sender, TextBox)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value
        If Date.TryParse(obj.Text, Now) Then
            Dim req_date As Date = CDate(obj.Text)
            myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, id), String.Format("reqdate='{0}'", req_date))
            ReCalDue(UID, id)
        End If
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If GetValFromForm() = 1 Then

            'If (COMM.Util.IsTesting()) Then
            'Ryan 20160316 
            'If (Role.IsAonlineUsa() OrElse Role.IsMexicoAonlineSales()) AndAlso CType(sender, Button).ID <> "NCNR_Confirm_Button" Then
            If Me._IsUSAOnlineEP AndAlso CType(sender, Button).ID <> "NCNR_Confirm_Button" Then
                If Advantech.Myadvantech.Business.OrderBusinessLogic.IsRiskOrder(UID, RiskOrderInputType.Quote) Then
                    NCNR_UpdatePanel.Update()
                    Me.NCNR_Popup.Show()
                    Exit Sub
                End If
            End If
            'End If

            Dim myQD As New EQDSTableAdapters.QuotationDetailTableAdapter, myQPartnerApt As New EQDSTableAdapters.EQPARTNERTableAdapter
            Dim QDDT As New EQDS.QuotationDetailDataTable, QPartners As New EQDS.EQPARTNERDataTable
            Dim QMDT As IBUS.iDocHeaderLine = MasterRef
            Dim M As IBUS.iDocHeader = Pivot.NewObjDocHeader

            'If QMDT Is Nothing OrElse QMDT.Count = 0 Then Exit Sub
            If QMDT Is Nothing OrElse IsNothing(QMDT) Then
                Response.Redirect("~/home.aspx")
            End If
            'Frank:If quoteToErpId=UEPP5001 then go to quotationMaster.aspx when user clicks the confirm button
            If QMDT.AccErpId.Equals("UEPP5001", StringComparison.InvariantCultureIgnoreCase) Then
                Util.showMessage("Pre-config quotation must [<a href='QuotationMaster.aspx?UID=" & UID & "'> Click Here</a>] to re-pick a correct account. ")
                Exit Sub
                'Response.Redirect("QuotationMaster.aspx?UID=" & UID)
            End If

            QDDT = myQD.GetQuoteDetailById(QMDT.Key)

            Dim detail As New List(Of struct_GP_Detail) ', decQuoteTotal As Decimal = 0
            For Each x As EQDS.QuotationDetailRow In QDDT
                Dim detailLine As New struct_GP_Detail
                detailLine.lineNo = x.line_No : detailLine.PartNo = x.partNo : detailLine.Price = x.newUnitPrice
                detailLine.QTY = x.qty : detailLine.Itp = x.newItp : detail.Add(detailLine)
                'decQuoteTotal += x.unitPrice * x.qty
            Next
            Dim strShiptoId As String = ""
            QPartners = myQPartnerApt.GetPartnerByQIDAndType(QMDT.Key, "S")
            If QPartners.Count = 0 Then
                strShiptoId = QMDT.AccErpId
            Else
                strShiptoId = QPartners(0).ERPID
            End If
            Dim decSubTotal As Decimal = 0
            If String.IsNullOrEmpty(strShiptoId) = False Then
                decSubTotal = Business.GetTaxableAmount(QMDT.Key, strShiptoId)
            End If
            '20120723 TC: Get Tax% and calculate Tax for ship-to is US quote
            'If QMDT(0).quoteId.StartsWith("AUSQ", StringComparison.CurrentCultureIgnoreCase) Then
            'If Role.IsUsaUser() Then
            'Dim _ME As Quote_Master_Extension = Nothing
            If _IsUSUser Then

                Dim _decTaxPercentage As Decimal = 0

                If QMDT.isExempt = 0 Then
                    'Dim _txtTempZipCode As String = Relics.SAPDAL.getUSZipcodeByShipToID(strShiptoId)
                    'If Not String.IsNullOrEmpty(_txtTempZipCode) Then
                    '    _decTaxPercentage = Relics.SAPDAL.getSalesTaxByZIP(_txtTempZipCode)
                    'End If

                    Dim _txtTempZipCode As String = SAPDAL.SAPDAL.getUSZipcodeByShipToID(strShiptoId)
                    If Not String.IsNullOrEmpty(_txtTempZipCode) Then
                        _decTaxPercentage = SAPDAL.SAPDAL.getSalesTaxByZIP(_txtTempZipCode)
                    End If


                    'Dim strShiptoId As String = ""
                    'QPartners = myQPartnerApt.GetPartnerByQIDAndType(QMDT(0).quoteId, "S")
                    'If QPartners.Count = 0 Then
                    '    strShiptoId = QMDT(0).quoteToErpId
                    'Else
                    '    strShiptoId = QPartners(0).ERPID
                    'End If
                    'If String.IsNullOrEmpty(strShiptoId) = False Then
                    '    Dim txtTempZipCode As String = ""
                    '    If SAPTools.getUSZipcodeByERPID(strShiptoId, txtTempZipCode) AndAlso String.IsNullOrEmpty(txtTempZipCode) = False Then
                    '        'Dim ws As New AgsWS.USTaxService, decTaxPercentage As Decimal = 0
                    '        'If ws.getSalesTaxByZIP(txtTempZipCode, decTaxPercentage) Then
                    '        '    If decTaxPercentage >= 0 Then
                    '        '        Dim decTax = FormatNumber(decQuoteTotal * decTaxPercentage, 2)
                    '        '        myQM.UpdateTax(decTax, QMDT(0).quoteId)
                    '        '    End If
                    '        'End If

                    '        decTaxPercentage = SAPTools.getSalesTaxByZIP(txtTempZipCode)

                    '    End If
                    'End If
                    'Else
                    '    myQM.UpdateTax(0, QMDT(0).quoteId)
                End If


                If _decTaxPercentage > 0 Then
                    'Dim decTax = FormatNumber(decSubTotal * _decTaxPercentage, 2)
                    M.Update(QMDT.Key, String.Format("tax='{0}'", _decTaxPercentage), COMM.Fixer.eDocType.EQ)
                Else
                    M.Update(QMDT.Key, String.Format("tax='{0}'", 0), COMM.Fixer.eDocType.EQ)
                End If



                'If _IsUSAENC Then

                '    _ME = MyQuoteX.GetMasterExtension(UID)

                '    If _ME IsNot Nothing Then
                '        _ME.Engineer_Telephone = Me.TextBoxLeadTime.Text
                '        _ME.Warranty = Me.TextBoxWarranty.Text
                '        MyQuoteX.LogQuoteMasterExtension(_ME)
                '    Else
                '        Dim _Master_Extension As New Quote_Master_Extension
                '        _Master_Extension.QuoteID = UID
                '        _Master_Extension.EmailGreeting = ""
                '        _Master_Extension.SpecialTandC = ""
                '        _Master_Extension.SignatureRowID = ""
                '        _Master_Extension.Engineer = ""
                '        _Master_Extension.LastUpdatedBy = Pivot.CurrentProfile.UserId
                '        _Master_Extension.LastUpdated = Now
                '        _Master_Extension.Engineer_Telephone = Me.TextBoxLeadTime.Text
                '        _Master_Extension.Warranty = Me.TextBoxWarranty.Text
                '        _Master_Extension.ApprovalFlowType = eQApprovalFlowType.Normal
                '        MyQuoteX.LogQuoteMasterExtension(_Master_Extension)
                '    End If


                'End If

            End If

            Me.BT_UpdateSalesNote_Click(Nothing, Nothing) : Me.BT_UpdateOrderNote_Click(Nothing, Nothing)
            'Dim itpType As Decimal = Relics.SAPDAL.itpType.EU
            'If Role.IsJPAonlineSales() Then
            '    itpType = Relics.SAPDAL.itpType.JP
            'End If

            Dim itpType As Decimal = SAPDAL.SAPDAL.itpType.EU
            If Me._IsJPOnline Then
                itpType = SAPDAL.SAPDAL.itpType.JP
            End If
            Dim _IsSpecialProductForAEU As Boolean = False
            Dim _IsNeedGP As Boolean = False
            If _IsEUQuote Then
                _IsSpecialProductForAEU = Business.checkSpecialProductForAEU(QMDT)
                '_IsNeedGP = Business.isNeedGPControl(UID, QMDT.org) AndAlso (GPControl.getLevel(QMDT.AccRowId, QMDT.AccErpId, detail, itpType, QMDT.AccOfficeName, QMDT.AccGroupName) > 0 OrElse (QMDT.org = "EU10" AndAlso GPControl.isDLGR(detail)))
                Dim office As String = QMDT.AccOfficeName
                Dim Group As String = QMDT.AccGroupName

                Dim _SalesCode As String = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetSalesCodeByQuoteID(UID)
                If Advantech.Myadvantech.Business.UserRoleBusinessLogic.IsBBIrelandBySalesID(_SalesCode) Then
                    Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetSalesOfficeAndGroupCodeBySalesCode(_SalesCode, office, Group)
                End If
                _IsNeedGP = Business.isNeedGPControl(UID, QMDT.org) AndAlso (GPControl.getLevel(QMDT.AccRowId, QMDT.AccErpId, detail, itpType, office, Group) > 0 OrElse (QMDT.org = "EU10" AndAlso GPControl.isDLGR(detail)))
            End If
            Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(QMDT.Key)
            If _QMaster Is Nothing Then Exit Sub
            Dim _ME As QuotationExtension = _QMaster.QuoteExtensionX
            _ME.ApprovalFlowType = eQApprovalFlowType.Normal
            If _IsNeedGP Then _ME.ApprovalFlowType = eQApprovalFlowType.GP
            If _IsSpecialProductForAEU Then _ME.ApprovalFlowType = eQApprovalFlowType.ThirtyDaysExpiration
            If _IsNeedGP AndAlso _IsSpecialProductForAEU Then _ME.ApprovalFlowType = eQApprovalFlowType.GPandExpiration

            If _IsUSAENC Then
                _ME.Engineer_Telephone = Me.TextBoxLeadTime.Text
                _ME.Warranty = Me.TextBoxWarranty.Text
            End If


            _ME.Update()
            ApprovalFlowType1.Visible = False : ApprovalFlowType2.Visible = False
            'If (New Integer() {1, 2, 12}).Contains(_ME.ApprovalFlowType) Then
            If Not _ME.ApprovalFlowType = eQApprovalFlowType.Normal Then
                If _ME.ApprovalFlowType = 1 OrElse _ME.ApprovalFlowType = 12 Then Me.ApprovalFlowType1.Visible = True
                If _ME.ApprovalFlowType = 2 OrElse _ME.ApprovalFlowType = 12 Then Me.ApprovalFlowType2.Visible = True
                UPConfirm.Update()
                Me.MPConfirm.Show()
            Else

                Dim GPitems As New List(Of Advantech.Myadvantech.DataAccess.QuotationDetail)()

                If (_IsKRAonlineUser AndAlso QuoteBusinessLogic.IsNeedGPforARK(UID)) OrElse
                    (_IsHQDCUser AndAlso QuoteBusinessLogic.IsNeedGPforIntercon(UID, 0, GPitems)) OrElse
                    (_IsABRUser AndAlso QuoteBusinessLogic.IsNeedGPforABR(UID, 0, GPitems)) OrElse
                    (_IsUSAENC AndAlso QuoteBusinessLogic.IsNeedGPforAENC(UID)) OrElse
                    ((Role.IsAonlineUsaIag OrElse Role.IsAonlineUsaISystem) AndAlso QuoteBusinessLogic.IsNeedGPforANAIIoTAOnline(UID, 0, GPitems)) Then
                    If GPitems.Count > 0 Then

                        If _IsHQDCUser OrElse _IsKRAonlineUser OrElse _IsABRUser Then
                            gvbelowgpitems.Visible = True
                        End If

                        gvbelowgpitems.DataSource = GPitems
                        gvbelowgpitems.DataBind()
                    End If
                    If _IsHQDCUser Then
                        Dim _QM_Extension As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
                        If _QM_Extension IsNot Nothing Then
                            Me.txtNotifyEmail.Text = _QM_Extension.Engineer
                        End If
                        TRNotifyCC.Visible = True
                    End If
                    ApprovalFlowType1.Visible = True : UPConfirm.Update()
                    Me.MPConfirm.Show()
                    Exit Sub
                End If

                'M.Update(UID, String.Format("isLumpSumOnly='{0}',isShowListPrice='{1}',isShowDiscount='{2}',isShowDueDate='{3}'", oIsLumpSumOnly, oIsShowListPrice, oIsShowDiscount, oIsShowDueDate), COMM.Fixer.eDocType.EQ)
                If _IsKRAonlineUser Then
                Else
                    M.Update(UID, String.Format("isLumpSumOnly='{0}',isShowListPrice='{1}',isShowDiscount='{2}',isShowDueDate='{3}'", oIsLumpSumOnly, oIsShowListPrice, oIsShowDiscount, oIsShowDueDate), COMM.Fixer.eDocType.EQ)
                End If

                goNext(UID)
            End If
            'If _IsSpecialProductForAEU Then
            '    Me.MPConfirm.Show()
            'ElseIf Business.isNeedGPControl(UID, QMDT.org) And _
            '     (GPControl.getLevel(QMDT.AccRowId, QMDT.AccErpId, detail, itpType, QMDT.AccOfficeName, QMDT.AccGroupName) > 0 OrElse (QMDT.org = "EU10" AndAlso GPControl.isDLGR(detail))) Then
            '    Me.MPConfirm.Show()
            'Else
            '    M.Update(UID, String.Format("isLumpSumOnly='{0}',isShowListPrice='{1}',isShowDiscount='{2}',isShowDueDate='{3}'", oIsLumpSumOnly, oIsShowListPrice, oIsShowDiscount, oIsShowDueDate), COMM.Fixer.eDocType.EQ)
            '    goNext(UID)
            'End If

        End If
    End Sub
    Protected Sub goNext(ByVal UID As String)
        updatedueDate()
        updateLastUpdatedDate()

        Dim _QM_Extension As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
        If _QM_Extension IsNot Nothing Then
            _QM_Extension.IsShowTotal = cbShowTotal.Checked
            _QM_Extension.IsShowFreight = cbShowFreight.Checked

            'Frank 20160714
            'I put CC of GP approval result mail to QM_Extension.Engineer field
            If _IsHQDCUser Then
                _QM_Extension.Engineer = Me.txtNotifyEmail.Text
            End If

        Else
            Dim QM_Extension As New Quote_Master_Extension
            QM_Extension.QuoteID = UID
            QM_Extension.IsShowTotal = cbShowTotal.Checked
            QM_Extension.IsShowFreight = cbShowFreight.Checked
            MyUtil.Current.CurrentDataContext.Quote_Master_Extensions.InsertOnSubmit(QM_Extension)
        End If
        MyUtil.Current.CurrentDataContext.SubmitChanges()

        If Me._IsJPOnline Then
            Response.Redirect(String.Format("~/Quote/Quotation2SiebelAJP.aspx?UID=" & UID))
        Else
            Response.Redirect(String.Format("~/Quote/Quotation2Siebel.aspx?UID=" & UID))
        End If
    End Sub
    Sub updatedueDate()
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("eq", String.Format("select max(duedate) as duedate from quotationDetail where quoteid='{0}'", UID))
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim MDue As String = ""
            If Not IsDBNull(dt.Rows(0).Item("duedate")) AndAlso dt.Rows(0).Item("duedate") IsNot Nothing Then
                MDue = dt.Rows(0).Item("duedate")
            End If
            Pivot.NewObjDocHeader.Update(UID, String.Format("deliveryDate='{0}'", MDue), COMM.Fixer.eDocType.EQ)
        End If
    End Sub

    Sub updateLastUpdatedDate()
        Dim QM As Quote_Master = MyQuoteX.GetQuoteMaster(UID)
        If QM IsNot Nothing Then
            QM.LastUpdatedDate = DateTime.Now
            MyUtil.Current.CurrentDataContext.SubmitChanges()
        End If
    End Sub
    'Protected Sub ibtnPickPartNo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    '    If Not IsNothing(MasterRef) Then
    '        Dim org As String = MasterRef.Org
    '        Dim partNo As String = Util.ReplaceSQLChar(Me.txtPartNo.Text.ToUpper.Trim())
    '        CType(Me.ascxPickProduct.FindControl("drpOrg"), DropDownList).SelectedValue = org
    '        CType(Me.ascxPickProduct.FindControl("txtName"), TextBox).Text = partNo
    '        Me.ascxPickProduct.ShowData(org, partNo, "") : Me.UPPickProduct.Update() : Me.MPPickProduct.Show()
    '    End If
    'End Sub
    <Services.WebMethod(EnableSession:=True)>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function UpdatePartNO(ByVal Quoteid As String, ByVal Lineno As String, ByVal Partno As String) As String
        Dim hash As New Hashtable(), serializer As New JavaScriptSerializer
        Dim strErr As String = String.Empty
        Try
            Dim QM As Quote_Master = MyUtil.Current.CurrentDataContext.Quote_Masters.Where(Function(p) p.quoteId = Quoteid).FirstOrDefault()
            Dim Qitem As QuoteItem = MyQuoteX.GetQuoteItem(Quoteid, Lineno)
            Dim QEWitem As QuoteItem = MyQuoteX.GetQuoteItem(Quoteid, Lineno + 1)
            Dim strStatusCode As String = "", strStatusDesc As String = "", decATP As Decimal = 0
            If Business.isInvalidPhaseOutV2(Partno, QM.org, strStatusCode, strStatusDesc, decATP) Then
                If String.IsNullOrEmpty(strStatusCode) AndAlso String.IsNullOrEmpty(strStatusDesc) Then
                    strErr = Partno & " cannot be found"
                End If
                strErr = String.Format("Product status of {0} is {1} ({2}),PLM Notice:{3}", Partno, strStatusCode, strStatusDesc, Business.getPLMNote(Partno, QM.org))
                Select Case strStatusCode
                    Case "O", "S"
                        strErr += " and has no inventory"
                    Case "I", "M1", "O1", "S1", "S2", "S5", "T", "V"
                    Case Else
                End Select
                hash("status") = 0
                hash("message") = strErr
                Return serializer.Serialize(hash)
            End If
            If QM IsNot Nothing AndAlso Qitem IsNot Nothing Then
                Dim unitPrice As Decimal = 0, listPrice As Decimal = 0, itp As Decimal = 0
                Dim dtPrice As New DataTable
                'Ming 20150603 RecyclingFee
                Qitem.RecyclingFee = 0
                Dim ShipTo As String = Business.GetShipToByQuoteID(Quoteid)
                Business.GetPriceBiz(QM.quoteToErpId, ShipTo, QM.org, QM.currency, "", QM.siebelRBU, Partno, QM.createdBy, dtPrice, QM.DocReg, strErr)
                If Not IsNothing(dtPrice) AndAlso dtPrice.Rows.Count > 0 Then
                    unitPrice = dtPrice.Rows(0).Item("Netwr")
                    listPrice = dtPrice.Rows(0).Item("Kzwi1")
                    If String.Equals(QM.org, "US01", StringComparison.InvariantCultureIgnoreCase) Then
                        Dim _RecycleFee As Decimal = 0
                        If Not IsDBNull(dtPrice.Rows(0).Item("RECYCLE_FEE")) AndAlso Decimal.TryParse(dtPrice.Rows(0).Item("RECYCLE_FEE").ToString(), _RecycleFee) Then
                            Qitem.RecyclingFee = _RecycleFee
                        End If
                    End If
                End If
                Dim strCategoryName As String = String.Empty
                If Qitem.ItemTypeX = QuoteItemType.BtosPart Then
                    strCategoryName = Business.GetCategoryName(Partno, QM.org)
                End If
                Qitem.partNo = Partno
                Qitem.VirtualPartNo = Partno
                Qitem.unitPrice = unitPrice
                Qitem.newUnitPrice = unitPrice
                Qitem.listPrice = listPrice
                Qitem.category = strCategoryName
                Dim partnodesc As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 PRODUCT_DESC    from  dbo.SAP_PRODUCT where PART_NO='{0}'", Partno))
                If partnodesc IsNot Nothing Then
                    Qitem.description = partnodesc.ToString.Trim
                End If
                If Qitem.ItemTypeX = QuoteItemType.Part Then
                    Qitem.ewFlag = 0
                End If
                MyUtil.Current.CurrentDataContext.SubmitChanges()
                If Qitem.ItemTypeX = QuoteItemType.Part Then
                    If QEWitem IsNot Nothing AndAlso QEWitem.IsEWpartnoX Then
                        MyQuoteX.DeleteQuoteItem(Quoteid, QEWitem.line_No)
                        MyQuoteX.ReSetLineNo(Quoteid)
                    End If
                End If
                If Qitem.ItemTypeX = QuoteItemType.BtosPart Then
                    Dim QBtosEWitem As QuoteItem = MyUtil.Current.CurrentDataContext.QuoteItems.Where(Function(p) p.HigherLevel = Qitem.HigherLevel AndAlso p.quoteId = Qitem.quoteId).OrderByDescending(Function(p) p.line_No).FirstOrDefault()
                    If QBtosEWitem IsNot Nothing AndAlso QBtosEWitem.IsEWpartnoX() Then
                        Dim QBtositem As QuoteItem = MyUtil.Current.CurrentDataContext.QuoteItems.Where(Function(p) p.line_No = Qitem.HigherLevel AndAlso p.quoteId = Qitem.quoteId).FirstOrDefault()
                        If QBtositem IsNot Nothing Then
                            Dim totalprice As Decimal = QBtositem.ChildWarrantAbleSubUnitPriceX * QBtosEWitem.EW_RateX
                            QBtosEWitem.listPrice = totalprice
                            QBtosEWitem.unitPrice = totalprice
                            QBtosEWitem.newUnitPrice = totalprice
                            MyUtil.Current.CurrentDataContext.SubmitChanges()
                        End If
                    End If
                End If
                hash("status") = 1
                hash("message") = "Succes"

                'Ryan 20160203 Add to fix bug "ITP not updating while changing part no".
                If Role.IsHQDCSales Then
                    Dim _errmsg As String = String.Empty
                    QuoteBusinessLogic.WriteInterconQuoteLinesITP(Quoteid, _errmsg)
                ElseIf Role.IsAonlineUsa Then
                    'Ryan 20160729 Add to fix bug "USAONLINE ITP not updating while changing part no".
                    Dim newItp As Decimal = 0

                    If Role.IsAonlineUsaIag OrElse Role.IsAonlineUsaISystem Then
                        newItp = Business.GetAUSCost(Partno, Qitem.deliveryPlant, Quoteid)
                    Else
                        Business.isANAPnBelowGPV2(QM.quoteToErpId, Partno, unitPrice, newItp, "")
                    End If

                    Qitem.itp = newItp
                    Qitem.newItp = newItp
                    MyUtil.Current.CurrentDataContext.SubmitChanges()
                Else
                    'Ryan 20170918 Get ITP for else regions 
                    Dim newItp As Decimal = 0
                    Business.GetITP(Quoteid, Partno, newItp, unitPrice)
                    Qitem.itp = newItp
                    Qitem.newItp = newItp
                    MyUtil.Current.CurrentDataContext.SubmitChanges()
                End If
            End If
        Catch ex As Exception
            hash("status") = 0
            hash("message") = ex.ToString()
        End Try
        Return serializer.Serialize(hash)
    End Function
    Public Sub PickProductEnd(ByVal str As Object)
        oPartNo = str(0).ToString
        SetValToForm(oPartNo, Me.txtQty.Text,
                     IIf(Me.cbxListPrice.Checked, 1, 0),
                     IIf(Me.cbxDisc.Checked, 1, 0),
                     IIf(Me.cbxDueDate.Checked, 1, 0),
                     IIf(Me.cbxLumpSum.Checked, 1, 0))
        Me.UPForm.Update()
        Me.MPPickProduct.Hide()
    End Sub

    'Public Sub InitProdinfo(ByVal partNo As String, ByVal org As String, ByVal PartDeliveryPlant As String)
    '    tbProdInfo.Visible = True
    '    Me.lbPartNo.Text = partNo : Me.lbProdStatus.Text = Business.getProductStatus(partNo, org)
    '    'Dim DT As New DataTable
    '    'DT = Business.getATPdetail(partNo, org)
    '    'gvAddedPNInventory.DataSource = DT : gvAddedPNInventory.DataBind() : Me.lbPLMNOTE.Text = Business.getPLMNote(partNo, org)

    '    'Dim prod_input As New Relics.SAPDALDS.ProductInDataTable, _sapdal As New Relics.SAPDAL
    '    Dim prod_input As New SAPDAL.SAPDALDS.ProductInDataTable, _sapdal As New SAPDAL.SAPDAL
    '    'Dim MainDeliveryPlant As String = "USH1", _errormsg As String = String.Empty
    '    Dim MainDeliveryPlant As String = Business.getPlantByOrgID(org), _errormsg As String = String.Empty
    '    'Dim inventory_out As New Relics.SAPDALDS.QueryInventory_OutputDataTable
    '    Dim inventory_out As New SAPDAL.SAPDALDS.QueryInventory_OutputDataTable
    '    prod_input.AddProductInRow(partNo, 0, PartDeliveryPlant)
    '    _sapdal.QueryInventory_V2(prod_input, MainDeliveryPlant, Now, inventory_out, _errormsg)
    '    gvAddedPNInventory.DataSource = inventory_out : gvAddedPNInventory.DataBind() : Me.lbPLMNOTE.Text = Business.getPLMNote(partNo, org)
    '    Me.lbABCDindicator.Text = Business.getPartIndicator(partNo, PartDeliveryPlant) : Me.lbMOQ.Text = Business.getMOQ(partNo, org)
    'End Sub

    Protected Sub lbtnConfig_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If _IsTWOnline OrElse _IsHQDCUser Then
            Response.Redirect(String.Format("~/quote/CustomBTOS_ATW.aspx?UID={0}&ORGID=TW01", UID))
        ElseIf _IsKRAonlineUser Then
            Response.Redirect(String.Format("~/quote/CustomBTOS_ATW.aspx?UID={0}&ORGID=KR01", UID))
        ElseIf _IsCNAonlineUser Then
            'The direction url need to be changed when syncing the ACN bom to Myadvantech function is ready
            Response.Redirect(String.Format("~/quote/CustomBTOS_ACN.aspx?UID={0}", UID))
        Else
            Response.Redirect(String.Format("~/quote/Catalog.aspx?UID={0}", UID))
        End If

    End Sub



    Protected Sub btnSpecialItp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value
        Me.hLine.Value = id



        Dim _txtITPobj As TextBox = CType(row.FindControl("txtItp"), TextBox)
        Dim _lbSPRNO As Label = CType(row.FindControl("lbSPRNO"), Label)

        Dim LineNO As Integer = 0
        If Integer.TryParse(Me.hLine.Value, 0) Then
            LineNO = Integer.Parse(Me.hLine.Value)
        End If


        If _IsEUQuote Then
            lbITPMsg.Visible = True
            lbITPMsg.Text = ""
            Me.txtSITP.Text = ""
            Me.txtSPRNO.Text = ""

            Dim item As Advantech.Myadvantech.DataAccess.QuotationDetail = QuoteBusinessLogic.GetQuotationDetail(UID).SingleOrDefault(Function(P) P.line_No = LineNO)
            lbITPTitle.Text = "ITP (FOB ACL):"
            If Business.IsPTD(item.partNo) Then
                lbITPTitle.Text = "AESC ITP:"
            End If
            If item.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase) OrElse
                item.partNo.StartsWith("IMG-", StringComparison.InvariantCultureIgnoreCase) OrElse
                item.partNo.StartsWith("IMG ", StringComparison.InvariantCultureIgnoreCase) Then
                lbITPTitle.Text = "Special Service Price:"
            End If

            'Ryan 20161122 Disable txtSITP due to SPR verification is enabled.
            If Not Business.IsPTD(item.partNo) Then
                txtSITP.Enabled = False
            Else
                lbITPMsg.Text = "For P-Trade items, please input both special ITP and SPR No. fields."
                txtSITP.Enabled = True
            End If
        End If


        If _IsHQDCUser Then
            txtSITP.Enabled = False : txtSITP.Text = ""
            Dim item As Advantech.Myadvantech.DataAccess.QuotationDetail = QuoteBusinessLogic.GetQuotationDetail(UID).SingleOrDefault(Function(P) P.line_No = id)
            If item IsNot Nothing Then
                If Not String.IsNullOrEmpty(item.sprNo) Then Me.txtSITP.Text = Math.Round(item.newItp.Value, 2)
                Me.txtSPRNO.Text = item.sprNo
            End If
        Else
            If _txtITPobj IsNot Nothing AndAlso Not String.IsNullOrEmpty(_txtITPobj.Text.Trim) _
                       AndAlso _lbSPRNO IsNot Nothing AndAlso Not String.IsNullOrEmpty(_lbSPRNO.Text.Trim) Then
                Dim SITP As Decimal = 0.0
                Decimal.TryParse(_txtITPobj.Text.Trim, SITP)
                If SITP > 0 Then
                    SITP = SITP / 1.045
                End If
                Me.txtSITP.Text = Math.Round(SITP, 2)
                Me.txtSPRNO.Text = _lbSPRNO.Text.Trim
            End If
        End If
        Me.UPSITP.Update() : Me.MPSITP.Show()
    End Sub

    Protected Sub btnSITP_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim SPRNO As String = Me.txtSPRNO.Text.Replace("'", "''")
        Dim ITP As String = Me.txtSITP.Text.Trim.Replace(",", "")

        Dim LineNO As Integer = 0
        If Integer.TryParse(Me.hLine.Value, 0) Then
            LineNO = Integer.Parse(Me.hLine.Value)
        End If

        Dim item As Advantech.Myadvantech.DataAccess.QuotationDetail = QuoteBusinessLogic.GetQuotationDetail(UID).SingleOrDefault(Function(P) P.line_No = LineNO)
        If item Is Nothing Then
            Exit Sub
        End If

        If String.IsNullOrEmpty(SPRNO) AndAlso String.IsNullOrEmpty(ITP) Then
            myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, Me.hLine.Value), "newItp=itp,SPRNO=''")

        ElseIf _IsHQDCUser OrElse (_IsEUQuote AndAlso Not Business.IsPTD(item.partNo)) Then
            'Ming 20151105  set ITP  for intercon
            If String.IsNullOrEmpty(SPRNO) Then
                'myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, Me.hLine.Value), String.Format("Itp='{0}',SPRNO='{1}',newItp='{2}', newUnitPrice=unitPrice", 0, "", 0))
                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, Me.hLine.Value), String.Format("SPRNO='{0}',newItp=itp, newUnitPrice=unitPrice", ""))
                Response.Redirect("~/quote/quotationDetail.aspx?VIEW=0&UID=" & UID, True)
            End If
            'Dim LineNO As Integer = 0
            'If Integer.TryParse(Me.hLine.Value, 0) Then
            '    LineNO = Integer.Parse(Me.hLine.Value)
            'End If
            Dim QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(UID)
            If item IsNot Nothing Then
                Dim sb As New StringBuilder
                sb.AppendFormat(" select A.Request_No,A.RBU_NAME,A.CUS_ID,B.SpecialOffered ,B.Model,A.P_From, A.P_End,isnull(B.Request_SP,0) as Request_SP,A.STATUS, isnull(A.CUS_CURRENCY,'') AS CUS_CURRENCY ")
                sb.AppendFormat("  from  [SPR_record] A right join  SPR_Model B on  A.RECORD_ID =B.Record_ID  ")
                sb.AppendFormat("  where A.Request_No='{0}'", SPRNO)
                Dim dt As DataTable = tbOPBase.dbGetDataTable("EZ", sb.ToString())
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Dim dr As DataRow() = dt.Select("CUS_ID='" + QMaster.quoteToErpId + "'")
                    If dr.Length = 0 Then
                        Util.AjaxShowMsg(Me.UPSITP, "The SPR is not applied for the quote-to account """ + QMaster.quoteToErpId + """", "itpmsg")
                        Exit Sub
                    End If
                    dr = dt.Select("Model='" + item.partNo + "'")
                    If dr.Length = 0 Then
                        Util.AjaxShowMsg(Me.UPSITP, "The part no. of this SPR is not the same as the one of the current quote line.", "itpmsg")
                        Exit Sub
                    End If
                    Dim dritem As DataRow = dr(0)
                    If IsDBNull(dritem.Item("SpecialOffered")) Then
                        Util.AjaxShowMsg(Me.UPSITP, "Please input the SPR No. which is applied for the special ITP.", "itpmsg")
                        Exit Sub
                    End If
                    If Not IsDBNull(dritem.Item("STATUS")) Then
                        If Not String.Equals("APPROVED", dritem.Item("STATUS")) Then
                            Util.AjaxShowMsg(Me.UPSITP, "The SPR has not yet been approved", "itpmsg")
                            Exit Sub
                        End If
                    End If
                    If Not IsDBNull(dritem.Item("P_From")) AndAlso Not IsDBNull(dritem.Item("P_End")) Then
                        Dim P_From As DateTime = Now, P_End As DateTime = Now
                        If DateTime.TryParse(dritem.Item("P_From"), P_From) AndAlso DateTime.TryParse(dritem.Item("P_End"), P_End) Then
                            If P_From.Date <= QMaster.expiredDate.Value AndAlso QMaster.expiredDate.Value <= P_End.Date Then
                                'Means SPR expired date and Quotation expired date is valid
                            Else
                                If DateTime.Now <= P_End.Date AndAlso P_From.Date <= QMaster.expiredDate.Value AndAlso QMaster.expiredDate.Value > P_End.Date Then
                                    QMaster.expiredDate = P_End.Date
                                    eQuotationContext.Current.SaveChanges()
                                Else
                                    Util.AjaxShowMsg(Me.UPSITP, "The SPR is expired.", "itpmsg")
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                    Dim SITP As Decimal = 0.0, Request_SP As Decimal = 0.0
                    If Decimal.TryParse(dritem.Item("SpecialOffered"), SITP) AndAlso Decimal.TryParse(dritem.Item("Request_SP"), Request_SP) Then
                        If SITP > 0 AndAlso Request_SP > 0 Then
                            'Ryan 20161129 If is EU quotes and non Ptrade, need 4.5% mark up.
                            If _IsEUQuote Then

                                'Check RBU currency to check if ITP needs currecy exchange
                                If Not String.IsNullOrEmpty(dritem.Item("RBU_NAME").ToString) Then
                                    Dim RBU_Currency As String = tbOPBase.dbExecuteScalar("ePricerV2", "select Currency from RBUmapping where RBU_NAME = '" + dritem.Item("RBU_NAME").ToString + "'")
                                    If Not RBU_Currency Is Nothing AndAlso Not String.IsNullOrEmpty(RBU_Currency) Then
                                        ' If SPR rbu currency is not equals to QMaster currency, needs to do currency exchanging
                                        If Not RBU_Currency.ToUpper.Equals(QMaster.currency.ToUpper) Then
                                            Dim exchange_rate As String = Business.get_exchangerate(RBU_Currency, QMaster.currency)
                                            If Decimal.TryParse(exchange_rate, 0) Then
                                                SITP = SITP * Convert.ToDecimal(exchange_rate)
                                            End If
                                        End If
                                    End If
                                End If

                                'Check if request_sp (selling price) needs currency exchange (ITP has checked!! no need to do twice)
                                If Not QMaster.currency.ToUpper.Equals(dritem.Item("CUS_CURRENCY").ToString.ToUpper) Then
                                    Dim exchange_rate As String = Business.get_exchangerate(dritem.Item("CUS_CURRENCY").ToString.ToUpper, QMaster.currency)
                                    If Decimal.TryParse(exchange_rate, 0) Then
                                        Request_SP = Request_SP * Convert.ToDecimal(exchange_rate)
                                    End If
                                End If

                                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, Me.hLine.Value), String.Format("newItp='{0}',SPRNO='{1}',newunitprice='{2}'", SITP * 1.045, SPRNO, Request_SP))
                            Else
                                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, Me.hLine.Value), String.Format("newItp='{0}',SPRNO='{1}',newunitprice='{2}'", SITP, SPRNO, Request_SP))
                            End If
                        End If
                    End If
                Else
                    Util.AjaxShowMsg(Me.UPSITP, "SPR No. is invalid", "itpmsg")
                    Exit Sub
                End If
            End If
        Else
            Dim SITP As Decimal = 0.0
            If Not IsNumeric(ITP) Then
                Util.AjaxShowMsg(Me.UPSITP, "ITP must be numeric", "itpmsg")
                Exit Sub
                ' Me.MPSITP.Hide() : Exit Sub
            End If
            If String.IsNullOrEmpty(SPRNO) Then
                Util.AjaxShowMsg(Me.UPSITP, "SPR cannot be empty", "itpmsg")
                Exit Sub
                ' Me.MPSITP.Hide() : Exit Sub
            End If
            SITP = Decimal.Parse(ITP)

            'Frank IPTx1.045是指AESC賣給各RBU需要把成本墊高，不然AESC沒賺到錢
            'Ruud Proost 20151113:The 4.5% is needed to cover real expense to be paid which is financially recognized as COGS. 
            'Ryan 20161102 Per Ruud's request, remove PTrade item's 4.5% mark up.
            'Ryan 20161129 Only EU quotes' Ptrade items will apply below rule, non-Ptrade will use SPR verification.
            If _IsEUQuote Then
                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, Me.hLine.Value), String.Format("newItp='{0}',SPRNO='{1}'", SITP, SPRNO))
            Else
                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, Me.hLine.Value), String.Format("newItp='{0}',SPRNO='{1}'", SITP * 1.045, SPRNO))
            End If
        End If

        Response.Redirect("~/quote/quotationDetail.aspx?VIEW=0&UID=" & UID)
    End Sub

    Protected Sub btnRealConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If GetValFromForm() = 1 Then
            'Pivot.NewObjDocHeader.Update(UID, String.Format("isShowListPrice='{0}',isShowDiscount='{1}',isShowDueDate='{2}',isLumpSumOnly='{3}',RelatedInfo=N'{4}'", oIsShowListPrice, oIsShowDiscount, oIsShowDueDate, oIsLumpSumOnly, Util.ReplaceSQLChar(Me.txtRelatedInfo.Text.Trim)), COMM.Fixer.eDocType.EQ)
            If _IsKRAonlineUser Then
                Pivot.NewObjDocHeader.Update(UID, String.Format("RelatedInfo=N'{0}'", Util.ReplaceSQLChar(Me.txtRelatedInfo.Text.Trim)), COMM.Fixer.eDocType.EQ)
            Else
                Pivot.NewObjDocHeader.Update(UID, String.Format("isShowListPrice='{0}',isShowDiscount='{1}',isShowDueDate='{2}',isLumpSumOnly='{3}',RelatedInfo=N'{4}'", oIsShowListPrice, oIsShowDiscount, oIsShowDueDate, oIsLumpSumOnly, Util.ReplaceSQLChar(Me.txtRelatedInfo.Text.Trim)), COMM.Fixer.eDocType.EQ)
            End If

            'If _IsHQDCUser Then

            '    Dim _QM_Extension As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
            '    If _QM_Extension IsNot Nothing Then
            '        _QM_Extension.Engineer = Me.txtNotifyEmail.Text
            '        MyUtil.Current.CurrentDataContext.SubmitChanges()
            '    End If
            'End If


            goNext(UID)
        End If

    End Sub


    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'Ryan 20161018 Add for ABR reset process
        If _IsABRUser Then

            tbOPBase.dbExecuteNoQuery("EQ", "delete from QuotationExtensionABR where quoteid = '" + UID + "'")
            tbOPBase.dbExecuteNoQuery("EQ", "insert into QuotationExtensionABR values ('" + UID + "', '0')")
            Advantech.Myadvantech.Business.QuoteBusinessLogic.UpdateSystemPriceForABRQuote(UID, 0)
            Me.txtDMValue.Text = 0

            initGV(UID) : Me.gv1.DataBind()
        Else
            btnResetOriginalPrice_Click(sender, e)
        End If

        'myQD.Update(String.Format("quoteId='{0}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "'", UID), String.Format("newunitPrice=unitPrice"))
        'initGV(UID)
    End Sub

    Protected Sub btnConfirmMD_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim count As Integer = 0
        Dim _ListPrice As Double = 0
        Dim QuoteMaster As Quote_Master = MyQuoteX.GetQuoteMaster(UID)

        If _IsABRUser Then

            'If Me.rbtnMD.SelectedValue = "Dic" Then
            '    If IsNumeric(Me.txtDMValue.Text) Then
            '        tbOPBase.dbExecuteNoQuery("EQ", "delete from QuotationExtensionABR where quoteid = '" + UID + "'")
            '        tbOPBase.dbExecuteNoQuery("EQ", "insert into QuotationExtensionABR values ('" + UID + "', '" + Me.txtDMValue.Text + "')")

            '        Advantech.Myadvantech.Business.QuoteBusinessLogic.UpdateSystemPriceForABRQuote(UID, -Convert.ToDecimal(Me.txtDMValue.Text))
            '    Else
            '        lbAddErrMsg.Text = "Discount input can only be numbers."
            '    End If
            'End If

            If IsNumeric(Me.txtDMValue.Text) Then

                Dim _DMVal As Decimal = 0
                Decimal.TryParse(Me.txtDMValue.Text, _DMVal)


                If Me.rbtnMD.SelectedValue = "Dic" Then
                    _DMVal = _DMVal * -1
                End If

                tbOPBase.dbExecuteNoQuery("EQ", "delete from QuotationExtensionABR where quoteid = '" + UID + "'")
                'tbOPBase.dbExecuteNoQuery("EQ", "insert into QuotationExtensionABR values ('" + UID + "', '" + Me.txtDMValue.Text + "')")
                tbOPBase.dbExecuteNoQuery("EQ", "insert into QuotationExtensionABR values ('" & UID & "', '" & _DMVal & "')")

                'Advantech.Myadvantech.Business.QuoteBusinessLogic.UpdateSystemPriceForABRQuote(UID, -Convert.ToDecimal(Me.txtDMValue.Text))
                Advantech.Myadvantech.Business.QuoteBusinessLogic.UpdateSystemPriceForABRQuote(UID, _DMVal)
            Else
                lbAddErrMsg.Text = "Discount/Addition input can only be numbers."
            End If



        Else
            For i As Integer = 0 To gv1.Rows.Count - 1
                Dim chk As CheckBox = gv1.Rows(i).FindControl("chkKey")
                'Frank 2013/11/11: Get part's list price
                Dim lblistprice As Label = gv1.Rows(i).FindControl("lbListPrice")

                '_ListPrice = lblistprice.Text
                If Not Double.TryParse(lblistprice.Text, _ListPrice) Then
                    _ListPrice = 0
                End If

                If chk.Checked Then
                    Dim LineNo As Integer = gv1.DataKeys(gv1.Rows(i).RowIndex).Value

                    '======Ryan 20160506 Exclude items which are defined in GPDetailValidation======
                    Dim stra = "Select * from QuotationDetail where quoteid = '" & UID & "' and line_no = '" & LineNo & "'"
                    Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", stra)
                    Dim detail As New List(Of struct_GP_Detail)
                    If (dt.Rows.Count > 0 AndAlso dt IsNot Nothing) Then
                        For Each x As DataRow In dt.Rows
                            Dim detailLine As New struct_GP_Detail
                            detailLine.lineNo = x.Item("line_no")
                            detailLine.PartNo = x.Item("partno")
                            detailLine.Price = x.Item("newunitprice")
                            detailLine.QTY = x.Item("qty")
                            detailLine.Itp = x.Item("newitp")
                            detail.Add(detailLine)
                        Next
                        detail = GPControl.GPDetailValidation(detail)
                        If detail.Count = 0 Then
                            Continue For
                        End If
                    End If
                    '======================End========================

                    Dim QuoteItem As QuoteItem = MyQuoteX.GetQuoteItem(UID, LineNo)
                    If IsNumeric(Me.txtDMValue.Text) AndAlso Not Advantech.Myadvantech.Business.PartBusinessLogic.IsServicePart(QuoteItem.partNo, QuoteMaster.org) Then
                        Dim d As Decimal = Me.txtDMValue.Text / 100

                        'Ryan 20180122 Comment below out due to SQL update can't not meet new requirement, use entity instead.
                        If Me.rbtnMD.SelectedValue = "Dic" Then
                            'Frank 2013/11/11 : new unit price cannot less than 0
                            If (_ListPrice - _ListPrice * d) >= 0 Then
                                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}' and itemType<> '" & COMM.Fixer.eItemType.Parent & "' and partno not like '96%' and partno not like 'IMG%' and partno not like 'AGS%'", UID, LineNo), String.Format("newunitPrice = round(listPrice - listPrice * {0} , 2)", d))
                            Else
                                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}' and itemType<> '" & COMM.Fixer.eItemType.Parent & "' and partno not like '96%' and partno not like 'IMG%' and partno not like 'AGS%'", UID, LineNo), "newunitPrice = 0")
                            End If
                        Else
                            If d < 1 Then
                                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}' and itemType<> '" & COMM.Fixer.eItemType.Parent & "' and partno not like '96%' and partno not like 'IMG%' and partno not like 'AGS%'", UID, LineNo), String.Format("newunitPrice = round(newItp/(1 - {0}) , 2)", d))
                            Else
                                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}' and itemType<> '" & COMM.Fixer.eItemType.Parent & "' and partno not like '96%' and partno not like 'IMG%' and partno not like 'AGS%'", UID, LineNo), String.Format("newunitPrice = round(newItp * {0} , 2)", (d + 1)))
                            End If
                        End If
                    End If
                End If
            Next
        End If



        initGV(UID)
    End Sub

    Protected Sub imgXls_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Dim dt As New DataTable
        'dt = myQD.GetDT(String.Format("quoteId='{0}'", UID), "line_no")
        'Dim dtData As DataTable = dt.Copy()
        'dtData.Columns.Add("Extended Warranty", GetType(String))
        'dtData.Columns("Extended Warranty").SetOrdinal(5) : dtData.AcceptChanges()
        'For Each r As GridViewRow In gv1.Rows
        '    If r.RowType = DataControlRowType.DataRow Then
        '        Dim E_DropDownList As DropDownList = CType(r.Cells(6).FindControl("gv_drpEW"), DropDownList)
        '        ' Dim E_TextBox As TextBox = CType(r.Cells(6).FindControl("gv_lbEW"), TextBox)
        '        If E_DropDownList IsNot Nothing Then
        '            If E_DropDownList.SelectedValue <> "0" Then
        '                Dim dtDatars() As DataRow = dtData.Select(String.Format("line_No='{0}'", Me.gv1.DataKeys(r.RowIndex).Value))
        '                If dtDatars.Length > 0 Then
        '                    dtDatars(0).BeginEdit() : dtDatars(0)("Extended Warranty") = E_DropDownList.SelectedItem.Text
        '                    dtDatars(0).EndEdit() : dtData.AcceptChanges()
        '                End If
        '            End If
        '        End If
        '    End If
        'Next

        'Frank 20150811
        MyQuoteX.ExportQuoteToExcel(UID)
        'Dim _IQueryable = From p As QuoteItem In MyUtil.Current.CurrentDataContext.QuoteItems Where p.quoteId = UID
        '               Select New With {
        '                   .Line_No = p.line_No,
        '                   .PartNo = p.partNo,
        '                   .QTY = p.qty,
        '                   .ListPrice = p.ListPriceX,
        '                   .UnitPrice = p.UnitPriceX,
        '                   .Discount = CalculateDiscount(p.ListPriceX, p.UnitPriceX),
        '                   .Description = p.description,
        '                   .DeliveryPlant = p.deliveryPlant,
        '                   .HigherLevel = p.HigherLevel,
        '                   .ReqDate = p.reqDate,
        '                   .DueDate = p.dueDate}
        'Dim _list = _IQueryable.ToList()
        'Util.List2ExcelDownload(_list, "quoteDetail")
    End Sub


    Protected Sub upGV1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not IsPostBack AndAlso Not Me.isRequestValid Then
            Exit Sub
        End If
        ' Me.upbtnConfirm.Update()
        'Me.lbtotal.Text = FormatNumber(Business.getTotalAmount(CType(gv1.DataSource, DataTable)), 2)
        Dim L As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org).GetListAll(COMM.Fixer.eDocType.EQ)
        If IsNothing(L) Then
            Me.lbtotal.Text = "0.00"
        ElseIf (Me._IsTWOnline Or Me._IsCNAonlineUser) Then
            MyUtil.Current.CurrentDataContext = Nothing
            'Me.lbtotal.Text = MyQuoteX.GetATWTotalAmount(UID).ToString()
            Me.lbtotal.Text = Convert.ToInt32(L.getTotalAmount) & " / " & MyQuoteX.GetATWTotalAmount(UID).ToString()
        Else
            Me.lbtotal.Text = L.getTotalAmount
        End If

        If Not Me._IsUSUser Then
            Me.lbTotalMargin.Text = Business.getMargin(UID)
        End If
        If _IsEUQuote Then
            Me.lbStandardMargin.Text = Business.getStandardPartsMargin(UID)
            Me.lbPTDMargin.Text = Business.getPTDItemsMargin(UID)
        End If
        If Me._IsJPOnline Then
            Me.lbTotalMargin.Text = Business.getAllItemsMargin(UID)
            Me.lbTotalITP.Text = Business.getAllItemsITP(UID)
        End If

        'Frank 20170703 Tiger requests the total margin information
        If Me._IsHQDCUser Then
            Me.lbTotalMargin.Text = Business.getAllItemsMargin(UID)
            'Me.lbTotalITP.Text = Business.getAllItemsITP(UID)
        End If

        If Me._IsABRUser Then

            Me.rowTotalMargin.Visible = False : Me.rowTotalITP.Visible = False : Me.rowStandardMargin.Visible = False
            Me.rowPTDMargin.Visible = False : Me.rowMarginInformation.Visible = False
            Me.rowTotalICMS.Visible = True : Me.rowTotalICMSOrig.Visible = True : Me.rowTotalICMSDest.Visible = True : Me.rowTotalICMSSpec.Visible = True
            Me.rowTotalIPI.Visible = True : Me.rowTotalPIS.Visible = True
            Me.rowTotalCONGIS.Visible = True : Me.rowSUBTotal.Visible = True

            Me.rowTotalHeader.Text = "Total (TAX):"

            Dim qh As New Advantech.Myadvantech.DataAccess.QuotationMasterHelper
            Dim qm As Advantech.Myadvantech.DataAccess.QuotationMaster = qh.GetQuotationMaster(UID)
            Me.lbTotalICMS.Text = qm.GetABRTotalBX13().ToString("N2")
            Me.lbTotalICMSOrig.Text = qm.GetABRTotalBX94().ToString("N2")
            Me.lbTotalICMSDest.Text = qm.GetABRTotalBX95().ToString("N2")
            Me.lbTotalICMSSpec.Text = qm.GetABRTotalBX96().ToString("N2")
            Me.lbTotalIPI.Text = qm.GetABRTotalBX23().ToString("N2")
            Me.lbTotalPIS.Text = qm.GetABRTotalBX82().ToString("N2")
            Me.lbTotalCOFINS.Text = qm.GetABRTotalBX72().ToString("N2")
            Dim _quotetotalwithouttax = 0
            If Not IsNothing(L) Then
                _quotetotalwithouttax = L.getTotalAmount
            End If

            'Me.lbABRQuoteSubTotal.Text = FormatNumber(L.getTotalAmount, 0, , , TriState.True)
            Me.lbABRQuoteSubTotal.Text = _quotetotalwithouttax.ToString("N2")
            Me.lbtotal.Text = (_quotetotalwithouttax + qm.GetABRAllTaxTotalAmount).ToString("N2")
        End If

        'Frank 20170628 Nadia does not want to show decimal
        If Me._IsKRAonlineUser Then
            If IsNothing(L) Then
                Me.lbtotal.Text = 0
            Else
                Me.lbtotal.Text = FormatNumber(L.getTotalAmount, 0, , , TriState.True)
            End If


            'Me.lbTotalMargin.Text = Business.getAllItemsMargin(UID)
            Me.lbTotalMargin.Text = QuoteBusinessLogic.GetTotalMarginByQuotationDetails(UID).ToString("P")
            Me.lbTotalITP.Text = FormatNumber(Business.getAllItemsITP(UID), 0, , , TriState.True)
        End If

    End Sub


    Protected Sub txtRelatedInfo_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(dt) Then
            Me.txtRelatedInfo.Text = dt.relatedInfo
        End If
    End Sub

    Protected Sub imgPartNo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value

        Dim DTD As DataTable
        DTD = myQD.GetDT(String.Format("quoteID='{0}' AND Line_no='{1}'", UID, id), "")
        If DTD.Rows.Count > 0 And Not IsNothing(MasterRef) Then
            'InitProdinfo(DTD(0).Item("PARTNO"), MasterRef.org, DTD(0).Item("deliveryPlant"))
        End If
        'Me.UPpartInfo.Update()
    End Sub

    Protected Sub imgPartNoAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        If Not IsNothing(MasterRef) Then
            'InitProdinfo(Business.Format2SAPItem(Me.txtPartNo.Text.Trim), MasterRef.org, Me.dlAddPlant.SelectedValue)
        End If
        'Me.UPpartInfo.Update()
    End Sub
    <Services.WebMethod()>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function CheckIfReconfigurable(ByVal BTO As String, ByVal UID As String) As String
        Dim apt As New SqlClient.SqlDataAdapter(
            " select top 1 a.RECFIGID AS ROW_ID " +
            " from QuotationDetail a " +
            " where a.QuoteID=@CID and a.Line_No=@RID " +
            " order by a.Line_No desc ",
            ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        With apt.SelectCommand.Parameters
            .AddWithValue("CID", UID)
            .AddWithValue("RID", BTO)
        End With
        Dim reconfigDt As New DataTable
        apt.Fill(reconfigDt) : apt.SelectCommand.Connection.Close()
        If reconfigDt.Rows.Count = 1 Then
            Return reconfigDt.Rows(0).Item("ROW_ID")
        End If
        Return ""
    End Function
    Protected Sub btnConfigConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect(String.Format("~/quote/Catalog.aspx?UID={0}", UID))
    End Sub

    Protected Sub ibtnMaster_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Response.Redirect("~/quote/quotationMaster.aspx?UID=" & UID)
    End Sub

    Protected Sub ibtnDetail_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Response.Redirect("~/quote/quotationDetail.aspx?UID=" & UID)
    End Sub

    Protected Sub ibtnPreview_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Response.Redirect("~/quote/quotation2Siebel.aspx?UID=" & UID)
    End Sub

    Private Sub SaveNote(ByVal _Quoteid As String, ByVal _NoteType As String, ByVal _notetext As String)
        Dim _QuotNoteDA As New EQDSTableAdapters.QuotationNoteTableAdapter
        _QuotNoteDA.DeleteQuotationNote(_Quoteid, _NoteType) : _QuotNoteDA.InsertQuotationNote(_Quoteid, _NoteType, _notetext)
    End Sub

    Protected Sub BT_PickCTOAssemblyInstructionDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '20130814 Per disscution of Jay & TC, Nada restored back assembly pick logic
        Dim obj As Button = CType(sender, Button), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim _systemName As String = CType(row.FindControl("hlPartNo"), HyperLink).Text
        If UID Is Nothing Then Exit Sub
        If MasterRef Is Nothing Then Exit Sub
        pupupMPCTOSAssemblyDoc(_systemName)
    End Sub

    Private Sub pupupMPCTOSAssemblyDoc(ByVal _SystemName As String)
        Dim _SoldToID As String = String.Empty, _EQParentTA As EQDSTableAdapters.EQPARTNERTableAdapter = New EQDSTableAdapters.EQPARTNERTableAdapter
        Dim _dt As EQDS.EQPARTNERDataTable = _EQParentTA.GetPartnerByQIDAndType(UID, "SOLDTO")
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            _SoldToID = CType(_dt.Rows(0), EQDS.EQPARTNERRow).ERPID
            CType(Me.ascxPickCTOSAssDoc1.FindControl("h_rowid"), HiddenField).Value = _SoldToID
            CType(Me.ascxPickCTOSAssDoc1.FindControl("h_system_name"), HiddenField).Value = _SystemName
            CType(Me.ascxPickCTOSAssDoc1.FindControl("txtERPID"), TextBox).Text = _SoldToID
            Me.ascxPickCTOSAssDoc1.ShowData(_SoldToID, "") : Me.UPPickCTOSDoc.Update() : Me.MPCTOSAssDoc.Show()
        End If
    End Sub

    Public Sub PickCTOSAssemblyInstructionDocEnd(ByVal DOCURL As Object, ByVal SystemName As Object)
        Dim _DOCURL As String = DOCURL
        Dim _SystemName As String = SystemName
        If Not String.IsNullOrEmpty(_DOCURL) Then
            If Not String.IsNullOrEmpty(Me.txtSalesNote.Text) Then
                Me.txtSalesNote.Text = Me.txtSalesNote.Text + Environment.NewLine
            End If
            Me.txtSalesNote.Text = IIf(String.IsNullOrEmpty(Me.txtSalesNote.Text), "", Me.txtSalesNote.Text + vbLf) + "[****]" + _SystemName + " instruction: " + vbLf + _DOCURL
            'Me.txtSalesNote.Text = _SystemName & " instruction " & Me.txtSalesNote.Text + _DOCURL
            Me.UP_QuotationNote.Update()
        End If
        Me.MPCTOSAssDoc.Hide()
    End Sub


    Protected Sub BT_UpdateSalesNote_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim _NoteText As String = Me.txtSalesNote.Text, _Quoteid As String = UID, _NoteType As String = "SalesNote"
        SaveNote(_Quoteid, _NoteType, _NoteText) : Me.LB_SalesNote.Text = "Saved."
    End Sub

    Protected Sub imgPartNo_Click1(sender As Object, e As ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim _partno As String = CType(row.FindControl("hlPartNo"), HyperLink).Text
        If Me._IsUSAENC OrElse Me._IsAAC Then
            Me.ascxUSVD.ShowData(_partno)
            Me.upUSVD.Update()
        End If
    End Sub

    Protected Sub BT_UpdateOrderNote_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim _NoteText As String = Me.txtOrderNote.Text, _Quoteid As String = UID, _NoteType As String = "OrderNote"
        SaveNote(_Quoteid, _NoteType, _NoteText) : Me.LB_OrderNote.Text = "Saved."
    End Sub



    Protected Sub txtCategory_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As TextBox = CType(sender, TextBox), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Me.gv1.DataKeys(row.RowIndex).Value
        Dim D As String = Util.ReplaceSQLChar(obj.Text)
        If id <> 100 Then
            myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", UID, id), String.Format("category='{0}'", D))
        End If
    End Sub
    Protected Function GetAvailableQty(ByVal objQty As Object) As String
        If objQty IsNot Nothing AndAlso Not String.IsNullOrEmpty(objQty.ToString) Then
            If String.Equals(objQty.ToString, "0") Then
                Return "TBD"
            Else
                Return objQty.ToString.Trim
            End If
        End If
        Return "TBD"
    End Function


    '20130530 TC: Add Page Method GetACLATP to let client side using jQuery Ajax to get ACL inventory and show it right beside product related info block
    <Services.WebMethod()>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function GetACLATP(ByVal strPartNo As String) As String
        'Dim prod_input As New Relics.SAPDALDS.ProductInDataTable, _sapdal As New Relics.SAPDAL
        Dim prod_input As New SAPDAL.SAPDALDS.ProductInDataTable, _sapdal As New SAPDAL.SAPDAL
        Dim MainDeliveryPlant As String = "TWH1", _errormsg As String = String.Empty
        'Dim inventory_out As New Relics.SAPDALDS.QueryInventory_OutputDataTable
        Dim inventory_out As New SAPDAL.SAPDALDS.QueryInventory_OutputDataTable
        prod_input.AddProductInRow(strPartNo, 0, MainDeliveryPlant)
        If _sapdal.QueryInventory_V2(prod_input, MainDeliveryPlant, Now, inventory_out, _errormsg) Then
            Dim atpInfoObj As New ATPTotalInfo
            atpInfoObj.PartNo = strPartNo
            atpInfoObj.ATPRecords = New List(Of ATPRecord)
            'For Each invRow As Relics.SAPDALDS.QueryInventory_OutputRow In inventory_out
            For Each invRow As SAPDAL.SAPDALDS.QueryInventory_OutputRow In inventory_out
                Dim atpRec As New ATPRecord
                atpRec.Qty = invRow.STOCK : atpRec.AvailableDate = invRow.STOCK_DATE.ToString(Pivot.CurrentProfile.DatePresentationFormat)
                atpInfoObj.ATPRecords.Add(atpRec)
            Next
            Dim serializer = New Script.Serialization.JavaScriptSerializer()
            Dim json As String = serializer.Serialize(atpInfoObj)
            Return json
        End If
        Return ""
    End Function

    Class ATPTotalInfo
        Private _strPN As String, _ATPRecords As List(Of ATPRecord)
        Public Property PartNo As String
            Get
                Return _strPN
            End Get
            Set(ByVal value As String)
                _strPN = value
            End Set
        End Property

        Public Property ATPRecords As List(Of ATPRecord)
            Get
                Return _ATPRecords
            End Get
            Set(ByVal value As List(Of ATPRecord))
                _ATPRecords = value
            End Set
        End Property

    End Class

    Class ATPRecord
        Private _intQty As Integer, _dtDate As String
        Public Property Qty As Integer
            Get
                Return _intQty
            End Get
            Set(ByVal value As Integer)
                _intQty = value
            End Set
        End Property
        Public Property AvailableDate As String
            Get
                Return _dtDate
            End Get
            Set(ByVal value As String)
                _dtDate = value
            End Set
        End Property
    End Class

    Private Sub btnApplyMinimumPrice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplyMinimumPrice.Click

        Dim tmpMinPrice As Double = 0, tmpErrMsg As String = String.Empty
        Dim _QuoteDetail As List(Of QuoteItem) = MyQuoteX.GetQuoteList(UID)
        Dim EWLineNos As New SortedSet(Of Integer)

        Dim _MinimumPrice_SalesTeam As SAPDAL.SAPDAL.MinimumPrice_SalesTeam
        If _IsTWOnline Then
            _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.ATW_AOnline
        End If
        If Role.IsInterconIA Then
            _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.Intercon_iA
        ElseIf Role.IsInterconEC OrElse Role.IsInterconIS Then
            _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.Intercon_EC
        End If


        For Each _item As QuoteItem In _QuoteDetail
            If _item.ewFlag > 0 Then
                Select Case _item.ItemTypeX
                    Case QuoteItemType.BtosParent, QuoteItemType.Part
                        EWLineNos.Add(_item.line_No)
                End Select
            End If
            If _item.ItemTypeX = QuoteItemType.BtosParent Then Continue For
            If _item.partNo.ToUpper.StartsWith("AGS-") Then Continue For

            tmpMinPrice = SAPDAL.SAPDAL.GetMinPrice("TW01", _item.partNo, MasterRef.currency, _MinimumPrice_SalesTeam, tmpErrMsg, Me.HCurrency.Value)
            If tmpMinPrice = -1 Then Continue For
            _item.newUnitPrice = tmpMinPrice
        Next
        MyUtil.Current.CurrentDataContext.SubmitChanges()

        Dim cF As IBUS.iCartF = Pivot.FactCart
        Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org)
        cart.updateEW(EWLineNos, MasterRef.DocReg, COMM.Fixer.eDocType.EQ, MasterRef.currency)

        initGV(UID) : Me.gv1.DataBind()

    End Sub

    Private Sub btnResetOriginalPrice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetOriginalPrice.Click

        Dim _QuoteDetail As List(Of QuoteItem) = MyQuoteX.GetQuoteList(UID)
        Dim EWLineNos As New SortedSet(Of Integer)
        For Each _item As QuoteItem In _QuoteDetail
            If _item.ewFlag > 0 Then
                Select Case _item.ItemTypeX
                    Case QuoteItemType.BtosParent, QuoteItemType.Part
                        EWLineNos.Add(_item.line_No)
                End Select
            End If
            If _item.ItemTypeX = QuoteItemType.BtosParent Then Continue For
            If _item.partNo.ToUpper.StartsWith("AGS-") Then Continue For

            _item.newUnitPrice = _item.unitPrice
        Next
        MyUtil.Current.CurrentDataContext.SubmitChanges()

        Dim cF As IBUS.iCartF = Pivot.FactCart
        Dim cart As IBUS.iCart(Of IBUS.iCartLine) = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org)
        cart.updateEW(EWLineNos, MasterRef.DocReg, COMM.Fixer.eDocType.EQ, MasterRef.currency)

        initGV(UID) : Me.gv1.DataBind()

    End Sub

    Protected Sub BTgetDiscountPrice_Click(sender As Object, e As EventArgs) Handles BTgetDiscountPrice.Click
        If Trim(TBpriceGrade.Text).Length <> 8 Then Exit Sub
        Dim Products As New List(Of ProductPrice)
        Dim Qitems As List(Of Advantech.Myadvantech.DataAccess.QuotationDetail) = QuoteBusinessLogic.GetQuotationDetail(UID)
        For Each item As Advantech.Myadvantech.DataAccess.QuotationDetail In Qitems
            If Not item.IsEWpartnoX AndAlso item.ItemTypeX <> Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent Then
                Products.Add(New ProductPrice(Trim(item.partNo)))
            End If
        Next
        If Products IsNot Nothing Then
            Dim Pricegrade As String = Trim(TBpriceGrade.Text), PricingRBU As String = "ATW", PricingCurrency As String = "NTD", SalesOrg As String = "TW01"
            GetProductDiscountByEPricerPriceGrade(Pricegrade, PricingRBU, PricingCurrency, SalesOrg, Products)
            Dim tmpErrMsg As String = String.Empty

            Dim _MinimumPrice_SalesTeam As SAPDAL.SAPDAL.MinimumPrice_SalesTeam
            If _IsTWOnline Then
                _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.ATW_AOnline
            End If
            If Role.IsInterconIA Then
                _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.Intercon_iA
            ElseIf Role.IsInterconEC OrElse Role.IsInterconIS Then
                _MinimumPrice_SalesTeam = SAPDAL.SAPDAL.MinimumPrice_SalesTeam.Intercon_EC
            End If


            For Each pg In Products
                If pg.IsPricingOK Then
                    Dim items As List(Of Advantech.Myadvantech.DataAccess.QuotationDetail) = Qitems.Where(Function(p) p.partNo = pg.PartNo).ToList()
                    If items.Count > 0 Then

                        'Frank 20150918 Get part's minimum price
                        tmpErrMsg = String.Empty
                        Dim tmpMinPrice As Double = SAPDAL.SAPDAL.GetMinPrice("TW01", pg.PartNo, MasterRef.currency, _MinimumPrice_SalesTeam, tmpErrMsg, Me.HCurrency.Value)

                        For Each curritem In items
                            If pg.DiscountPrice > 0 Then
                                curritem.newUnitPrice = pg.DiscountPrice

                                'Frank 20150918 if discount price changed by price grade is less then part's minimum price
                                'Set minimum price to it new unit price
                                If curritem.newUnitPrice < tmpMinPrice AndAlso tmpMinPrice <> -1 Then
                                    curritem.newUnitPrice = tmpMinPrice
                                End If

                            End If
                        Next
                    End If
                End If
            Next
            For Each item As Advantech.Myadvantech.DataAccess.QuotationDetail In Qitems
                If item.ItemTypeX = Advantech.Myadvantech.DataAccess.QuoteItemType.BtosParent Then
                    item.newUnitPrice = 0
                End If
            Next
            eQuotationContext.Current.SaveChanges()
            initGV(UID) : Me.gv1.DataBind()
        End If
    End Sub
    <Services.WebMethod(EnableSession:=True)>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function GetPriceGrades(ByVal prefixText As String, ByVal count As Integer) As String()
        Return SAPDAL.ePricerDiscount.GetPriceGrades(prefixText, count)
    End Function
    <Services.WebMethod()>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function GetOrderHistory(PartNo As String, ErpID As String) As String
        Dim OrderRecords As New List(Of OrderHistoryRecord)
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(" select top 200 a.CURRENCY, a.LINE_NO, a.ORDER_DATE, a.ORDER_QTY, a.PART_NO, a.SO_NO, a.UNIT_PRICE   ")
            .AppendLine(" from SAP_ORDER_HISTORY_ATW2YEAR a  ")
            .AppendLine(" where a.COMPANY_ID='" + ErpID.Replace("'", "''") + "'  ")
            If Not String.IsNullOrEmpty(PartNo) Then .AppendLine(" and a.part_no like '%" + Replace(Replace(Trim(PartNo), "'", "''"), "*", "%") + "%'  ")
            .AppendLine(" order by a.ORDER_DATE desc ")
        End With
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", sb.ToString())
        For Each r As DataRow In dt.Rows
            Dim OrderHistoryRecord1 As New OrderHistoryRecord()
            With OrderHistoryRecord1
                .SO_NO = r.Item("SO_NO") : .ORDER_DATE = CDate(r.Item("ORDER_DATE")).ToString("yyyy/MM/dd")
                .LINE_NO = r.Item("LINE_NO") : .PART_NO = r.Item("PART_NO") : .ORDER_QTY = r.Item("ORDER_QTY")
                '.UNIT_PRICE = Util.FormatMoney(r.Item("UNIT_PRICE"), r.Item("CURRENCY"))

                'Frank UNIT_PRICE in table actually stored total amount.
                Dim _total As Decimal = 0
                Decimal.TryParse(r.Item("UNIT_PRICE"), _total)

                Dim _qty As Integer = 0
                Integer.TryParse(r.Item("ORDER_QTY"), _qty)
                If _qty = 0 Then _qty = 1

                .UNIT_PRICE = Util.FormatMoney(FormatNumber(_total / _qty, 2), r.Item("CURRENCY"))
                .AMOUNT = Util.FormatMoney(r.Item("UNIT_PRICE"), r.Item("CURRENCY"))
            End With
            OrderRecords.Add(OrderHistoryRecord1)
        Next
        Dim serializer As New Script.Serialization.JavaScriptSerializer()
        Return serializer.Serialize(OrderRecords)
    End Function

    <Services.WebMethod()>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function UpdateDescription(ByVal ID As String, ByVal description As String) As String
        'ICC 2016/2/26 For ATW to update description
        If Not String.IsNullOrEmpty(ID) Then
            Dim result As Boolean = Advantech.Myadvantech.Business.QuoteBusinessLogic.UpdateQuotationDescription(ID, description)
            If result = True Then Return (New JavaScriptSerializer).Serialize("Success")
        End If
        Return (New JavaScriptSerializer).Serialize("Error")
    End Function

    <Serializable()>
    Public Class OrderHistoryRecord
        Public Property SO_NO As String : Public Property ORDER_DATE As String : Public Property LINE_NO As String : Public Property PART_NO As String
        Public Property ORDER_QTY As Integer : Public Property UNIT_PRICE As String : Public Property AMOUNT As String
    End Class

    Protected Sub btnBreakPoints_Click(sender As Object, e As EventArgs) Handles btnBreakPoints.Click

    End Sub

    <Services.WebMethod()>
    <Web.Script.Services.ScriptMethod()>
    Public Shared Function CheckBTOSExist(ByVal UID As String) As String
        If Not String.IsNullOrEmpty(UID) Then
            Return (New JavaScriptSerializer).Serialize(MyQuoteX.IsHaveBtos(UID))
        End If
        Return (New JavaScriptSerializer).Serialize(False)
    End Function
End Class
