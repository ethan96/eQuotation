Imports HtmlAgilityPack
Imports AjaxControlToolkit.HTMLEditor
Imports Advantech.Myadvantech.DataAccess
Imports Advantech.Myadvantech.Business
Imports System.Threading

Public Class Quotation2Siebel
    Inherits PageBase

    Public quoteId As String = String.Empty, _ServerPath As String = ""
    Public _IsAUSUser As Boolean = False, _IsAACUser As Boolean = False
    Public _IsAJPUser As Boolean = False, _IsAEUUser As Boolean = False
    Public _IsATWUser As Boolean = False, _IsCNAonline As Boolean = False
    Public _IsKRAonline As Boolean = False, _IsHQDCUser As Boolean = False
    Public _IsAENCUser As Boolean = False, _IsABRUser As Boolean = False
    Public _IsAAUUser As Boolean = False, _IsANAAOnlineEP As Boolean = False
    Public _IsCAPS As Boolean = False

    'Public Function GetLoginUserIsANAUser() As Boolean
    '    If ViewState("IsANAUser") Is Nothing Then

    '        ViewState("IsANAUser") = Role.IsUsaUser()
    '    End If
    '    Return ViewState("IsANAUser")
    'End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If MasterRef.DocType <> COMM.Fixer.eDocType.EQ Then
            Util.showMessage("Only Quotation is editable.", "back")
        End If
        If IsNothing(Pivot.CurrentProfile) Then
            Session.Abandon() : Util.ClearCookie_Login("ADEQCOOK")
            Response.Redirect(String.Format("~/login.aspx?RURL={0}", HttpContext.Current.Server.UrlEncode(Request.RawUrl)))
        End If
        If Request("UID") IsNot Nothing Then
            If Role.IsUsaUser() Then _IsAUSUser = True
            If Role.IsAonlineUsa Then _IsANAAOnlineEP = True
            'If Role.IsUSAACSales() Then _IsAACUser = True
            If Role.IsUSAACSales() AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2100") Then
                _IsAACUser = True
            End If
            If Role.IsUsaAenc Then _IsAENCUser = True
            If Role.IsCAPS Then _IsCAPS = True
            If Role.IsJPAonlineSales() Then _IsAJPUser = True
            If Role.IsEUSales() Then _IsAEUUser = True
            If Role.IsTWAonlineSales Then _IsATWUser = True
            If Role.IsCNAonlineSales Then _IsCNAonline = True
            If Role.IsKRAonlineSales Then _IsKRAonline = True
            If Role.IsHQDCSales Then _IsHQDCUser = True
            If Role.IsABRSales Then _IsABRUser = True
            If Role.IsAAUSales Then _IsAAUUser = True
        End If
        'Me._IsAUSUser = GetLoginUserIsANAUser()

        Me._ServerPath = Util.GetRuntimeSiteUrl
        Me.quoteId = Request("UID")
        hdQuoteId.Value = Me.quoteId.ToString.Trim

        If Not IsPostBack AndAlso Request("UID") IsNot Nothing Then

            Dim QuoteTb As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(hdQuoteId.Value, COMM.Fixer.eDocType.EQ)

            'Frank 2012/07/30:If salesRowId of QuotationMaster is empty or no recodere in eqParnter then redirect to QuotationMaster.aspx
            '===Checking-start===
            If IsNothing(QuoteTb) Then
                Response.Redirect("~/home.aspx")
            End If
            If String.IsNullOrEmpty(QuoteTb.salesRowId) Then
                Response.Redirect("QuotationMaster.aspx?UID=" & hdQuoteId.Value)
            End If
            Dim aptEQPartner As New EQDSTableAdapters.EQPARTNERTableAdapter
            Dim QuotePartner As EQDS.EQPARTNERDataTable = aptEQPartner.GetPartnersByQuoteId(hdQuoteId.Value)
            If QuotePartner Is Nothing OrElse QuotePartner.Rows.Count = 0 Then
                Response.Redirect("QuotationMaster.aspx?UID=" & hdQuoteId.Value)
            End If
            '===Checking-end===

            'TC 2012/07/26:US AOnline user get Internal version by default, EU sales get external version by default
            If Me._IsAUSUser Then
                Me.RadioButtonList_PriviewMode.SelectedIndex = 0
            Else
                Me.RadioButtonList_PriviewMode.SelectedIndex = 1
            End If

            'Frank 2015/03/04: Forward checkbox does not need to show up on page for ATW user
            If _IsATWUser OrElse _IsCNAonline OrElse _IsKRAonline OrElse _IsHQDCUser Then
                Me.cbForward.Visible = False
            End If


            'Esther 20160412 Opportunity:  Remove this section since the AENC FSE’s will not use Siebel
            'Ryan 20160728 US AENC team menu visibility
            If _IsAENCUser OrElse _IsCAPS OrElse _IsAJPUser Then
                Me.tbopty.Visible = False
                Me.cbForward.Visible = False
                Me.RadioButtonList_PriviewMode.SelectedValue = "false"
                Me.RadioButtonList_PriviewMode.Items.RemoveAt(0)
                Me.btnGoSiebel.Text = "Confirm"

                If _IsAJPUser OrElse _IsCAPS Then Me.trFreight.Visible = False
                If _IsAJPUser Then drpAJPOffice.Visible = True
            End If

            'Frank 20140811: AEU's freight must be auto calculated before executing the function "getPageStr"
            Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(Me.quoteId)
            TBFreight.Text = _QM.freight
            Dim _QME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(Me.quoteId)

            If _IsKRAonline Then
                Me.view_type_option.Visible = False
                Me.AKR_Pricing_option.Visible = True

                Dim _selectval As Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod = Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod.ListAndNegoPrice
                [Enum].TryParse(Of Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod)(_QM.isShowListPrice.ToString, _selectval)
                '_QM.isShowListPrice = _selectval.ToString()

                Select Case _selectval
                    Case AKRQuotingPriceMethod.ListAndNegoPrice
                        Me.RadioButtonList_ListPriceOnly.SelectedIndex = 0
                    Case AKRQuotingPriceMethod.ListPriceOnly
                        Me.RadioButtonList_ListPriceOnly.SelectedIndex = 2
                    Case AKRQuotingPriceMethod.NegoPriceOnly
                        Me.RadioButtonList_ListPriceOnly.SelectedIndex = 1
                End Select


                'If _QM.isShowListPrice = 1 Then
                '    Me.RadioButtonList_ListPriceOnly.SelectedIndex = 1
                'End If
            End If


            If _QME IsNot Nothing AndAlso _IsAJPUser Then
                If Not String.IsNullOrEmpty(_QME.JPCustomerOffice) Then
                    If Not Me.drpAJPOffice.Items.FindByValue(_QME.JPCustomerOffice) Is Nothing Then
                        Me.drpAJPOffice.Items.FindByValue(_QME.JPCustomerOffice).Selected = True
                    Else
                        Me.drpAJPOffice.Items.FindByValue("1").Selected = True
                    End If
                Else
                    Me.drpAJPOffice.Items.FindByValue("1").Selected = True
                End If
            End If

            If _QME IsNot Nothing AndAlso (_IsAEUUser OrElse _IsAUSUser) Then
                Dim _IsNeedAutoCalculateFreight As Boolean = False
                If _QME.IsShowFreight IsNot Nothing AndAlso Not IsDBNull(_QME.IsShowFreight) AndAlso (Boolean.Parse(_QME.IsShowFreight) OrElse _QM.freight = 0) Then
                    _IsNeedAutoCalculateFreight = True ': ddlFreight.Visible = True
                End If
                If _IsAUSUser Then
                    trAUSFreight.Visible = True : _IsNeedAutoCalculateFreight = True
                End If

                'Frank 20141231 :Cathee has confirmed that freight function can be released to production site
                'If Not COMM.Util.IsTesting() Then
                'trAUSFreight.Visible = False
                'End If

                If _IsAEUUser Then
                    trAEUFreight.Visible = True

                    '===Ryan 20171005 Comment out AEU copy quotation opty logic.===
                    'Ryan 20161229 Get copy purpose to set opty block visibility
                    'If Not _QME.CopyPurpose Is Nothing AndAlso _QME.CopyPurpose.HasValue Then
                    '    If _QME.CopyPurpose = 1 Then
                    '        ibtn_PickOpty.Visible = False
                    '    ElseIf _QME.CopyPurpose = 2 Then
                    '        ButtonNewOpty.Visible = False
                    '    End If
                    'End If
                    '===End 20171005 Comment out===
                End If

                '20150116 暂时取消PageLoad时候加载运费
                If _IsAEUUser AndAlso Decimal.TryParse(TBFreight.Text.Trim, 0) AndAlso Decimal.Parse(TBFreight.Text.Trim) = 0 Then
                    TBFreight.Text = "TBD"
                End If
                '运费功能恢复后，上面这段逻辑可以去掉
                _IsNeedAutoCalculateFreight = False
                If _IsNeedAutoCalculateFreight AndAlso _IsAEUUser Then
                    Dim ShipRatedt As DataTable = Util.getNewShipRate(Me.quoteId)
                    If ShipRatedt IsNot Nothing AndAlso ShipRatedt.Rows.Count > 0 Then
                        ' Dim _minFreight As Decimal = Decimal.Parse(ShipRatedt.Rows(0)("freight"))
                        For Each dr As DataRow In ShipRatedt.Rows
                            ' dr("TextStr") = String.Format("{0}  ({1})", dr("name"), dr("freight"))
                            If dr("name") IsNot Nothing AndAlso String.Equals(dr("name"), "TNT Economy", StringComparison.CurrentCultureIgnoreCase) Then
                                If dr("freight") IsNot Nothing AndAlso Decimal.TryParse(dr("freight"), 0) Then
                                    TBFreight.Text = dr("freight")
                                    If _IsAEUUser AndAlso _QM IsNot Nothing Then
                                        _QM.freight = Decimal.Parse(dr("freight"))
                                        MyUtil.Current.CurrentDataContext.SubmitChanges()
                                    End If
                                    Exit For
                                End If
                            End If
                        Next
                    Else
                        labnodata.Text = "System is busy, please retry the ""Refresh"" button to get freight"
                    End If
                    If Decimal.TryParse(TBFreight.Text.Trim, 0) AndAlso Decimal.Parse(TBFreight.Text.Trim) = 0 Then
                        TBFreight.Text = "TBD"
                    End If
                End If
            End If

            getPageStr(hdQuoteId.Value, Me.RadioButtonList_PriviewMode.SelectedValue) : INITOPTY()
            If QuoteTb.attentionEmail IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(QuoteTb.attentionEmail) Then
                txtForwardTo.Text = QuoteTb.attentionEmail
            End If


            '20140212
            If _IsATWUser Or _IsCNAonline Then
                labopty_owner_email.Visible = True : TBopty_owner_email.Visible = True
                TBopty_owner_email.Text = QuoteTb.CreatedBy
            End If


            'Frank 2016-11-10 AJP
            '' 20130401 set default opty
            'If _IsAJPUser Then
            '    'Nada revised to convenient maintenance (removed dbac part of quoteDetail)...........................................................................................
            '    'Dim QM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(hdQuoteId.Value, COMM.Fixer.eDocType.EQ)
            '    Dim QDT As IBUS.iCartList = Pivot.FactCart().getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, MasterRef.org).GetListAll(COMM.Fixer.eDocType.EQ)
            '    If Not IsNothing(QDT) AndAlso QDT.Count > 0 Then
            '        Dim QD As IBUS.iCartLine = QDT.FirstOrDefault
            '        Dim OPTYT As List(Of IBUS.iOptyQuoteLine) = Pivot.NewObjOptyQuote.GetListByQuoteID(UID)
            '        If Not IsNothing(OPTYT) AndAlso OPTYT.Count > 0 Then
            '            'If QM IsNot Nothing Then
            '            If _QM IsNot Nothing Then
            '                'Dim optyname As String = QM.AccName + " " + QD.partNo.Value + " " + QD.Qty.Value.ToString
            '                Dim optyname As String = _QM.quoteToName + " " + QD.partNo.Value + " " + QD.Qty.Value.ToString
            '                Call PickOptyEnd("new ID", optyname, "25% Proposing/Quoting")
            '                DDLOptyStage.Visible = True
            '            End If
            '            labopty_owner_email.Visible = True : TBopty_owner_email.Visible = True
            '            'TBopty_owner_email.Text = SiebelTools.Get_PRIMARY_SALES_EMAIL_ByAccountROWID(QM.AccRowId)
            '            TBopty_owner_email.Text = SiebelTools.Get_PRIMARY_SALES_EMAIL_ByAccountROWID(_QM.quoteToRowId)
            '        End If
            '    End If
            '    '/Nada revised to convenient maintenance (removed dbac part of quoteDetail)...........................................................................................
            'End If
            If _IsAEUUser Then
                LitAEUfreight.Visible = True
            End If

            'Ryan 20160727 Add due invoive validation for USAonline users
            If _IsANAAOnlineEP Then
                Dim open_dt As DataTable = Advantech.Myadvantech.Business.OrderBusinessLogic.GetOpenInvoiceStatus(_QM.quoteToErpId)
                Dim due_list As List(Of DataRow) = (From d As DataRow In open_dt.Rows Where d("AR_STATUS").Equals("Partial Overdue") Or d("AR_STATUS").Equals("Overdue") Select d).ToList()
                If due_list.Count > 0 Then
                    lb_dueinvoice.Text = "This company has due invoices and need your attention."
                End If
            End If

        End If
    End Sub

    Sub INITOPTY()

        Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
        Dim dt As EQDS.optyQuoteDataTable = myOptyQuote.GetOptyQuoteByOuoteID(hdQuoteId.Value)

        If dt.Rows.Count > 0 Then
            Me.txtOptyName.Text = dt(0).optyName : Me.txtOptyRowID.Text = dt(0).optyId
            Me.DDLOptyStage.SelectedValue = dt(0).optyStage
            If dt(0).optyId.Equals("new ID", StringComparison.InvariantCultureIgnoreCase) Then Me.cbx_NewOpty.Checked = True
        End If
    End Sub
    Sub getPageStr(ByVal UID As String, Optional ByVal IsInternalUser As Boolean = True)
        Me.NoToolBarEditor.InitialCleanUp = False

        Dim DT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        If _IsAJPUser Then
            Me.NoToolBarEditor.Content = Business.getPageStrInternal(UID, DT.DocReg, IsInternalUser)
        ElseIf _IsAEUUser Then
            Me.NoToolBarEditor.Content = Util.GetAllStringForAEU(UID)
        Else
            Me.NoToolBarEditor.Content = _
                "<style type=""text/css"">.dummy{}body{font-family: Arial,宋体;font-size: 12px;margin: 0px;}table{width: 100%;}td{border: solid 1px #EEEEEE;}p{line-height: 20px;}</style>" _
                & Business.getPageStrInternal(UID, DT.DocReg, IsInternalUser)
            'Ryan 20160412 Remove vertical text "Send all POs...".
            If _IsAACUser Then
                Me.NoToolBarEditor.Content = Me.NoToolBarEditor.Content.Replace("class=""verticaltext""", "class=""verticaltext"" style=""display:none;""")
            End If
        End If
        NoToolBarEditor.ActiveMode = ActiveModeType.Preview
        If _IsAJPUser Then
            NoToolBarEditor.ActiveMode = ActiveModeType.Design
            tdpdf.Visible = True
        End If
    End Sub

    Sub pupupMPOPTY(ByVal _tabid As Integer)

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(hdQuoteId.Value, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(dt) Then
            CType(Me.ascxPickopty1.FindControl("h_rowid"), HiddenField).Value = dt.AccRowId
            'Ming20141128如果已關聯了一個opty，當再次按下pick按鈕時，Project Name不用帶入textbox，然後查出此account 所有的opty。不然怕使用者誤以為只有這一個opty可以用
            CType(ascxPickopty1.FindControl("HFoptyID"), HiddenField).Value = txtOptyRowID.Text
            Me.ascxPickopty1.ShowData(dt.AccRowId) : Me.ascxPickopty1.SetTabSelectedIndex(_tabid)

            '===Ryan 20171005 Comment out AEU copy quotation opty logic.===
            'Ryan 20161229 If is AEU user, need to hide ascx tab according to its copy purpose
            'If _IsAEUUser Then
            '    Dim _QME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(Me.quoteId)
            '    If Not _QME Is Nothing AndAlso _QME.CopyPurpose.HasValue Then
            '        If _QME.CopyPurpose = 1 Then
            '            ' CopyPurpose = 1 means user wants to create new quotation from copy, needs to create new opty, hide pick tab in ascx
            '            Me.ascxPickopty1.DisableTabsbyIndex(0)
            '        ElseIf _QME.CopyPurpose = 2 Then
            '            ' CopyPurpose = 2 means user wants to revise existing quotation from copy, needs to pick existing opty, hide new opty tab 
            '            Me.ascxPickopty1.DisableTabsbyIndex(1)
            '        Else
            '            ' CopyPurpose = 0 means new quotation, no need to do anything
            '        End If
            '    End If
            'End If
            '===End 20171005 Comment out===

            Me.UPPickOpty.Update() : Me.MPPickOpty.Show()
        End If
    End Sub

    Protected Sub ibtn_PickOpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        pupupMPOPTY(0)
    End Sub

    Protected Sub ButtonNewOpty_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        pupupMPOPTY(1)
    End Sub

    Public Sub PickOptyEnd(ByVal optyid As Object, ByVal optyName As Object, ByVal optyStage As Object)
        Dim Id As String = optyid, Name As String = optyName, Stage As String = optyStage

        Me.txtOptyRowID.Text = Id : Me.txtOptyName.Text = Name : Me.UP_Opty.Update() : Me.MPPickOpty.Hide()
        If Not String.IsNullOrEmpty(optyStage) Then
            Me.DDLOptyStage.SelectedValue = optyStage
        End If
    End Sub

    Protected Sub cbx_NewOpty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If CType(sender, CheckBox).Checked Then
            Me.txtOptyRowID.Text = "new ID"
        Else
            Me.txtOptyRowID.Text = ""
        End If
        Me.txtOptyName.Text = "" : Me.UP_Opty.Update()
    End Sub

    Private Sub GoNext()
        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim optyId As String = Me.txtOptyRowID.Text.Trim.Replace("'", "''"), optyName As String = Me.txtOptyName.Text.Trim, opty_owner_email As String = TBopty_owner_email.Text.Trim
        'If _IsAJPUser Then
        '    Laberror.Text = ""
        '    Dim sb As New StringBuilder
        '    sb.AppendFormat(" select TOP 1 ROW_ID from siebel_contact where EMAIL_ADDRESS='{0}' and employee_flag='Y' ", opty_owner_email)
        '    Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", sb.ToString)
        '    If dt.Rows.Count = 0 Then
        '        Laberror.Text = "The sales email you input is invalid or does not exist in Siebel."
        '        Exit Sub
        '    End If
        'End If
        Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
        myOptyQuote.DeleteOptyByQuoteID(hdQuoteId.Value)
        If Not String.IsNullOrEmpty(optyId) Then

            'Nada revised............................................

            Dim optyQuote1 As IBUS.iOptyQuoteLine = Pivot.NewLineOptyQuote
            optyQuote1.optyId = optyId
            optyQuote1.optyName = optyName
            optyQuote1.Opty_Owner_Email = opty_owner_email
            optyQuote1.quoteId = hdQuoteId.Value
            optyQuote1.optyStage = DDLOptyStage.SelectedValue
            Dim OptyQuoteO As IBUS.iOptyQuote = Pivot.NewObjOptyQuote
            OptyQuoteO.DeleteByQuoteID(quoteId)
            OptyQuoteO.Add(optyQuote1)
            '/Nada revised............................................
        End If

        Dim QMDT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(hdQuoteId.Value, COMM.Fixer.eDocType.EQ), QDDT As New DataTable
        QDDT = myQD.GetDT(String.Format("quoteId='{0}'", hdQuoteId.Value), "line_No")
        Dim detail As New List(Of struct_GP_Detail)
        For Each x As DataRow In QDDT.Rows
            Dim detailLine As New struct_GP_Detail
            detailLine.lineNo = x.Item("line_no") : detailLine.PartNo = x.Item("partno") : detailLine.Price = x.Item("newunitprice")
            detailLine.QTY = x.Item("qty") : detailLine.Itp = x.Item("newitp") : detail.Add(detailLine)
        Next
        Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(QMDT.Key)
        If _QMaster Is Nothing Then Exit Sub
        Dim _ME As QuotationExtension = _QMaster.QuoteExtensionX
        If _ME Is Nothing Then Exit Sub
        Dim msg As String = String.Empty
        'If Not Business.isNeedGPControl(hdQuoteId.Value, QMDT.org) Then
        '1: GP  2: 30天 12: gp+30天
        If _ME.ApprovalFlowType = eQApprovalFlowType.Normal Then
            Dim myQuoteApproval As New quotation_approval("EQ", "quotation_approval")
            myQuoteApproval.Delete(String.Format("quote_Id='{0}'", hdQuoteId.Value))
            'Chiara:I am not receiving anymore the finished eQuotations by email authomatically
            If _IsAEUUser Then Business.send_Quotation_Approval(hdQuoteId.Value, "", COMM.Fixer.eDocType.EQ)

            Business.transQuote2Siebel(hdQuoteId.Value)
        Else
            Dim itemType As SAPDAL.SAPDAL.itpType = SAPDAL.SAPDAL.itpType.EU
            If _IsAJPUser Then itemType = SAPDAL.SAPDAL.itpType.JP
            If _ME.ApprovalFlowTypeX = eQApprovalFlowType.ThirtyDaysExpiration Then
                QuoteBusinessLogic.InitQuotationApprovalExpirationFlow(_QMaster.quoteId, msg)
                Business.send_Quotation_Approval_Expiration(hdQuoteId.Value, "", COMM.Fixer.eDocType.EQ)
            Else
                GPControl.InitApprovalFlow(hdQuoteId.Value, QMDT.AccRowId, QMDT.AccErpId, detail, itemType, QMDT.AccOfficeName, QMDT.AccGroupName)
                If GPControl.isApproved(hdQuoteId.Value) Then
                    'Chiara:I am not receiving anymore the finished eQuotations by email authomatically
                    If _IsAEUUser Then Business.send_Quotation_Approval(hdQuoteId.Value, "", COMM.Fixer.eDocType.EQ)
                    Business.transQuote2Siebel(hdQuoteId.Value)
                Else
                    Business.send_Quotation_Approval(hdQuoteId.Value, "", COMM.Fixer.eDocType.EQ)
                End If
            End If
        End If
        '20151016 Ming AKR GP
        Dim isNewGPFlow As Boolean = False
        If _IsKRAonline Then
            If QuoteBusinessLogic.IsNeedGPforARK(UID) Then
                isNewGPFlow = True
                GPcontrolAPI.GPcontrolBiz.CallFlow(UID, Util.GetRuntimeSiteUrl())
            End If
        End If
        If _IsABRUser Then
            Dim GPitems As New List(Of Advantech.Myadvantech.DataAccess.QuotationDetail)()
            If QuoteBusinessLogic.IsNeedGPforABR(UID, 0, GPitems) Then
                isNewGPFlow = True
                GPcontrolAPI.GPcontrolBiz.CallFlow(UID, Util.GetRuntimeSiteUrl())
            End If
        End If
        If _IsHQDCUser Then
            Dim GPitems As New List(Of Advantech.Myadvantech.DataAccess.QuotationDetail)()
            If QuoteBusinessLogic.IsNeedGPforIntercon(UID, 0, GPitems) Then
                isNewGPFlow = True
                GPcontrolAPI.GPcontrolBiz.CallFlow(UID, Util.GetRuntimeSiteUrl())
            End If
        End If
        If _IsAENCUser Then
            If QuoteBusinessLogic.IsNeedGPforAENC(UID) Then
                isNewGPFlow = True
                GPcontrolAPI.GPcontrolBiz.CallFlow(UID, Util.GetRuntimeSiteUrl())
            End If
        End If
        If Role.IsAonlineUsaIag Or Role.IsAonlineUsaISystem Then
            Dim GPitems As New List(Of Advantech.Myadvantech.DataAccess.QuotationDetail)()
            If QuoteBusinessLogic.IsNeedGPforANAIIoTAOnline(UID, 0, GPitems) Then
                isNewGPFlow = True
                GPcontrolAPI.GPcontrolBiz.CallFlow(UID, Util.GetRuntimeSiteUrl())
            End If
        End If


        SavePI2DB(hdQuoteId.Value, Business.getPageStrInternal(hdQuoteId.Value, QMDT.DocReg, False))
        Pivot.NewObjDocHeader.Update(hdQuoteId.Value, String.Format("quoteDate=getDate()"), COMM.Fixer.eDocType.EQ)

        '20120720 Rudy: Forward Quote to customer
        If cbForward.Checked Then

            Dim _errmsg As String = String.Empty, _IsSuccessforward As Boolean = True, _IsShowPageNum As Boolean = False
            If Me._IsAJPUser Then _IsShowPageNum = True
            _IsSuccessforward = Business.ForwardeQuotation(Me.quoteId, Me.forwardEquotationUI.ContentType _
                                                           , Me.forwardEquotationUI.RecipientEmail _
                                                           , Pivot.CurrentProfile.UserId _
                                                           , Pivot.CurrentProfile.UserId _
                                                           , "myadvantech@advantech.com" _
                                                           , Me.forwardEquotationUI.Subject _
                                                           , Me.forwardEquotationUI.EmailGreeting _
                                                           , Nothing _
                                                           , _IsShowPageNum, _errmsg)
        End If
        If isNewGPFlow Then
            For i = 0 To 9
                System.Threading.Thread.Sleep(1000)
                If QuoteBusinessLogic.checkWFData(UID) Then
                    Exit For
                End If
            Next
        End If

        'Ryan 20180420 Update quotation last update date
        updateLastUpdatedDate()

        'Frank 2012/07/26
        Response.Redirect("~/quote/quoteByAccountOwner.aspx")
    End Sub

    Private Sub updateLastUpdatedDate()
        Dim QM As Quote_Master = MyQuoteX.GetQuoteMaster(UID)
        If QM IsNot Nothing Then
            QM.LastUpdatedDate = DateTime.Now
            MyUtil.Current.CurrentDataContext.SubmitChanges()
        End If
    End Sub


    Protected Sub btnRealConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If HF_isgoingGP.Value = "1" Then
            Pivot.NewObjDocHeader.Update(UID, String.Format("RelatedInfo=N'{0}'", Util.ReplaceSQLChar(Me.GPreason.Text.Trim)), COMM.Fixer.eDocType.EQ)
            Dim url As String = Util.GetRuntimeSiteUrl()
            GPcontrolAPI.GPcontrolBiz.CallFlow(UID, url)
            For i = 0 To 9
                System.Threading.Thread.Sleep(1000)
                If QuoteBusinessLogic.checkWFData(UID) Then
                    Exit For
                End If
            Next
            'Dim starter As ThreadStart = Sub() GPcontrolAPI.GPcontrolBiz.CallFlow(UID, url)
            'Dim t As New Thread(starter)
            't.Start()
            't.Join()
        End If
        Me.GoNext()
    End Sub

    Protected Sub btnGoSiebel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'Ryan 20170912 Quotation opty is mandatory for all regions now (Except AENC, Siebel is not applied to them.)
        If Not _IsAENCUser Then
            If String.IsNullOrEmpty(Me.txtOptyRowID.Text) OrElse String.IsNullOrEmpty(Me.txtOptyName.Text.Trim) Then
                Util.showMessage("Please associate the quote with new/existed opportunity first.")
                Exit Sub
            End If
        End If


        'SaveQuote()
        Dim myQD As New quotationDetail("EQ", "quotationDetail")

        If _IsAUSUser OrElse Pivot.CurrentProfile.UserId.ToString().EndsWith("@advantech.com") _
            OrElse _IsATWUser OrElse _IsCNAonline OrElse _IsKRAonline OrElse _IsHQDCUser Then

            If COMM.Util.IsTesting AndAlso _IsAACUser Then

                Dim GPInfo1 As SAPDAL.SAPDAL.GPInfo = SAPDAL.SAPDAL.CalcANAQuoteGP(hdQuoteId.Value)
                If GPInfo1.LineItems IsNot Nothing Then
                    Me.gvGPQuoteLine.DataSource = GPInfo1.LineItems : Me.gvGPQuoteLine.DataBind()
                    Dim gpinfoList As New List(Of SAPDAL.SAPDAL.GPInfo)
                    gpinfoList.Add(GPInfo1)
                    gvAACGPTotalInfo.DataSource = gpinfoList : gvAACGPTotalInfo.DataBind()
                    Dim isgongingGP As Boolean = False
                    For Each i In GPInfo1.LineItems
                        If i.GPBlock Then
                            isgongingGP = True
                            Exit For
                        End If
                    Next
                    GPreason.Visible = False : HF_isgoingGP.Value = 0
                    If isgongingGP Then
                        gp2mail.Text = "GP approval request mail will be sent to GPApproval.AAC@advantech.com"
                        HF_isgoingGP.Value = 1 : GPreason.Visible = True
                    End If
                    Me.UP_AACGPConfirm.Update()
                    Me.MP_AACGP_Confirm.Show()
                    Exit Sub
                End If
                'ElseIf _IsHQDCUser Then
                '    'Ryan 20170128 Add for Intercon, quotation must input opty if needs GP approval.
                '    Dim GPitems As New List(Of Advantech.Myadvantech.DataAccess.QuotationDetail)()
                '    If QuoteBusinessLogic.IsNeedGPforIntercon(UID, 0, GPitems) Then
                '        If String.IsNullOrEmpty(Me.txtOptyRowID.Text) OrElse String.IsNullOrEmpty(Me.txtOptyName.Text.Trim) Then
                '            Util.showMessage("Please associate the quote with new/existed opportunity if the quote has low GP issue.")
                '            Exit Sub
                '        End If
                '    End If
                'ElseIf _IsKRAonline Then
                '    If String.IsNullOrEmpty(Me.txtOptyRowID.Text) OrElse String.IsNullOrEmpty(Me.txtOptyName.Text.Trim) Then
                '        Util.showMessage("Please associate the quote with new/existed opportunity first.")
                '        Exit Sub
                '    End If
            End If
        Else
            '20120707 TC: Only EU users need to do this check
            'Ryan 20161229 Comment below code out per Iris request - EU users new opty logic while coping quotations
            'If Me.txtOptyRowID.Text.Replace("'", "''").Trim = "" Or Me.txtOptyName.Text.Replace("'", "''").Trim = "" Then
            '    If myQD.getTotalAmount(hdQuoteId.Value) >= 20000 Then
            '        Util.showMessage("Opty ID or Opty Name cannot be null.")
            '        Exit Sub
            '    End If
            'End If
        End If

        Me.GoNext()

        'Dim optyId As String = Me.txtOptyRowID.Text.Trim.Replace("'", "''"), optyName As String = Me.txtOptyName.Text.Trim, opty_owner_email As String = TBopty_owner_email.Text.Trim
        'If _IsAJPUser Then
        '    Laberror.Text = ""
        '    Dim sb As New StringBuilder
        '    sb.AppendFormat(" select TOP 1 ROW_ID from siebel_contact where EMAIL_ADDRESS='{0}' and employee_flag='Y' ", opty_owner_email)
        '    Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", sb.ToString)
        '    If dt.Rows.Count = 0 Then
        '        Laberror.Text = "The sales email you input is invalid or does not exist in Siebel."
        '        Exit Sub
        '    End If
        'End If
        'Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
        'myOptyQuote.DeleteOptyByQuoteID(hdQuoteId.Value)
        'If Not String.IsNullOrEmpty(optyId) Then

        '    'Nada revised............................................

        '    Dim optyQuote1 As IBUS.iOptyQuoteLine = Pivot.NewLineOptyQuote
        '    optyQuote1.optyId = optyId
        '    optyQuote1.optyName = optyName
        '    optyQuote1.Opty_Owner_Email = opty_owner_email
        '    optyQuote1.quoteId = hdQuoteId.Value
        '    optyQuote1.optyStage = DDLOptyStage.SelectedValue
        '    Dim OptyQuoteO As IBUS.iOptyQuote = Pivot.NewObjOptyQuote
        '    OptyQuoteO.DeleteByQuoteID(quoteId)
        '    OptyQuoteO.Add(optyQuote1)
        '    '/Nada revised............................................
        'End If

        'Dim QMDT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(hdQuoteId.Value, COMM.Fixer.eDocType.EQ), QDDT As New DataTable
        'QDDT = myQD.GetDT(String.Format("quoteId='{0}'", hdQuoteId.Value), "line_No")
        'Dim detail As New List(Of struct_GP_Detail)
        'For Each x As DataRow In QDDT.Rows
        '    Dim detailLine As New struct_GP_Detail
        '    detailLine.lineNo = x.Item("line_no") : detailLine.PartNo = x.Item("partno") : detailLine.Price = x.Item("newunitprice")
        '    detailLine.QTY = x.Item("qty") : detailLine.Itp = x.Item("newitp") : detail.Add(detailLine)
        'Next
        'Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(QMDT.Key)
        'If _QMaster Is Nothing Then Exit Sub
        'Dim _ME As QuotationExtension = _QMaster.QuoteExtensionX
        'If _ME Is Nothing Then Exit Sub
        'Dim msg As String = String.Empty
        ''If Not Business.isNeedGPControl(hdQuoteId.Value, QMDT.org) Then
        ''1: GP  2: 30天 12: gp+30天
        'If _ME.ApprovalFlowType = eQApprovalFlowType.Normal Then
        '    Dim myQuoteApproval As New quotation_approval("EQ", "quotation_approval")
        '    myQuoteApproval.Delete(String.Format("quote_Id='{0}'", hdQuoteId.Value))
        '    'Chiara:I am not receiving anymore the finished eQuotations by email authomatically
        '    If _IsAEUUser Then Business.send_Quotation_Approval(hdQuoteId.Value, "", COMM.Fixer.eDocType.EQ)

        '    Business.transQuote2Siebel(hdQuoteId.Value)
        'Else
        '    Dim itemType As SAPDAL.SAPDAL.itpType = SAPDAL.SAPDAL.itpType.EU
        '    If _IsAJPUser Then itemType = SAPDAL.SAPDAL.itpType.JP
        '    If _ME.ApprovalFlowTypeX = eQApprovalFlowType.ThirtyDaysExpiration Then
        '        QuoteBusinessLogic.InitQuotationApprovalExpirationFlow(_QMaster.quoteId, msg)
        '        Business.send_Quotation_Approval_Expiration(hdQuoteId.Value, "", COMM.Fixer.eDocType.EQ)
        '    Else
        '        GPControl.InitApprovalFlow(hdQuoteId.Value, QMDT.AccRowId, QMDT.AccErpId, detail, itemType, QMDT.AccOfficeName, QMDT.AccGroupName)
        '        If GPControl.isApproved(hdQuoteId.Value) Then
        '            'Chiara:I am not receiving anymore the finished eQuotations by email authomatically
        '            If _IsAEUUser Then Business.send_Quotation_Approval(hdQuoteId.Value, "", COMM.Fixer.eDocType.EQ)
        '            Business.transQuote2Siebel(hdQuoteId.Value)
        '        Else
        '            Business.send_Quotation_Approval(hdQuoteId.Value, "", COMM.Fixer.eDocType.EQ)
        '        End If
        '    End If
        'End If
        'SavePI2DB(hdQuoteId.Value, Business.getPageStrInternal(hdQuoteId.Value, QMDT.DocReg, False))
        'Pivot.NewObjDocHeader.Update(hdQuoteId.Value, String.Format("quoteDate=getDate()"), COMM.Fixer.eDocType.EQ)

        ''20120720 Rudy: Forward Quote to customer
        'If cbForward.Checked Then

        '    Dim _errmsg As String = String.Empty, _IsSuccessforward As Boolean = True, _IsShowPageNum As Boolean = False
        '    If Me._IsAJPUser Then _IsShowPageNum = True
        '    _IsSuccessforward = Business.ForwardeQuotation(Me.quoteId, Me.forwardEquotationUI.ContentType _
        '                                                   , Me.forwardEquotationUI.RecipientEmail _
        '                                                   , Pivot.CurrentProfile.UserId _
        '                                                   , Pivot.CurrentProfile.UserId _
        '                                                   , "myadvantech@advantech.com" _
        '                                                   , Me.forwardEquotationUI.Subject _
        '                                                   , Me.forwardEquotationUI.EmailGreeting _
        '                                                   , Nothing _
        '                                                   , _IsShowPageNum, _errmsg)
        'End If

        ''Frank 2012/07/26
        'Response.Redirect("~/quote/quoteByAccountOwner.aspx")
    End Sub
    Sub SavePI2DB(ByVal quoteid As String, ByVal str As String)

        Dim bt As New BigText("EQ", "BigText")
        bt.Delete(String.Format("quoteid='{0}'", quoteid))
        bt.Add(quoteid, str.Replace("'", "''"))
    End Sub



    Protected Sub PLPickOpty_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        If txtOptyRowID.Text = "new ID" Then
            Me.txtOptyName.BackColor = Drawing.ColorTranslator.FromHtml("#ebebe4") : Me.cbx_NewOpty.Checked = True
            Me.LabelOptyName.Visible = True : Me.txtOptyName.Visible = True
            Me.LabelOptyStage.Visible = True : Me.DDLOptyStage.Visible = True
        Else
            Me.txtOptyName.BackColor = Drawing.ColorTranslator.FromHtml("#ebebe4") : Me.cbx_NewOpty.Checked = False
            Me.LabelOptyName.Visible = True : Me.txtOptyName.Visible = True
            Me.LabelOptyStage.Visible = False : Me.DDLOptyStage.Visible = False
        End If
    End Sub

    Protected Sub btnPDFSpeed_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'SavePI2DB( hdQuoteId.Value, Me.editor.Content)
        SavePI2DB(hdQuoteId.Value, Me.NoToolBarEditor.Content)
    End Sub

    Protected Sub cbForward_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If cbForward.Checked Then
            PanelForward.Visible = True
            Me.forwardEquotationUI.QuoteID = Me.quoteId
            Me.upForward.Update()
        Else
            PanelForward.Visible = False
        End If
    End Sub

    Protected Sub RadioButtonList_ListPriceOnly_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonList_ListPriceOnly.SelectedIndexChanged
        'Nadia 20170824:Sales can select
        '1) Nego price only ==> Nego / nego vat / total
        '2) List price only ==> List / List vat / total
        '3) List & Nego ==> List/ Nego / Nego vat / total
        Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(Me.quoteId)
        'Dim _selectval As Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod = Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod.ListAndNegoPrice
        '[Enum].TryParse(Of Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod)(Me.RadioButtonList_ListPriceOnly.SelectedValue, _selectval)
        '_QM.isShowListPrice = _selectval.ToString()


        Dim _selval As Integer = 0
        Integer.TryParse(Me.RadioButtonList_ListPriceOnly.SelectedValue, _selval)
        _QM.isShowListPrice = _selval
        MyUtil.Current.CurrentDataContext.SubmitChanges()
        getPageStr(Request("UID"))
        Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub RadioButtonList_PriviewMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        getPageStr(Request("UID"), Me.RadioButtonList_PriviewMode.SelectedValue)
        Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub drpAJPOffice_SelectedIndexChanged(sender As Object, e As EventArgs)

        'Log user selected value to DB and redirect to generate quote template again.
        Dim selected_value As String = Me.drpAJPOffice.SelectedValue
        If Not String.IsNullOrEmpty(selected_value) Then
            Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
            If _ME IsNot Nothing Then
                _ME.JPCustomerOffice = selected_value
                MyQuoteX.LogQuoteMasterExtension(_ME)
                getPageStr(Request("UID"), Me.RadioButtonList_PriviewMode.SelectedValue)
                Me.UP_QuotationPreview.Update()
            End If
        End If
    End Sub

    Protected Sub ImageBtPdf_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If _IsAJPUser Then
            Dim QM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(Me.quoteId, COMM.Fixer.eDocType.EQ)
            If QM IsNot Nothing Then
                Dim myContentByte As Byte() = Util.DownloadQuotePDFByHtmlString(NoToolBarEditor.Content.ToString.Trim, True)
                Dim fname As String = Me.quoteId.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
                If QM.CustomId IsNot Nothing AndAlso Not IsDBNull(QM.CustomId) AndAlso Not String.IsNullOrEmpty(QM.CustomId) Then
                    fname = QM.CustomId.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
                End If
                Response.Clear()
                Response.AddHeader("Content-Type", "binary/octet-stream")
                Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".pdf;size = " & myContentByte.Length.ToString())
                Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
            End If
        End If

    End Sub

    Protected Sub btAdjus_Click(sender As Object, e As EventArgs) Handles btAdjus.Click
        Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(Me.quoteId)
        If Decimal.TryParse(TBFreight.Text.Trim, 0) = False Then
            If _QM IsNot Nothing Then TBFreight.Text = _QM.freight
            Util.AjaxShowLoading(Me.UP_QuotationPreview, String.Empty, "Freight must be a number", 10)
            Exit Sub
        End If
        If Decimal.Parse(TBFreight.Text.Trim) < 0 Then
            If _QM IsNot Nothing Then TBFreight.Text = _QM.freight
            Util.AjaxShowLoading(Me.UP_QuotationPreview, String.Empty, "Freight must be greater than zero", 10)
            Exit Sub
        End If
        If _QM IsNot Nothing AndAlso Decimal.TryParse(TBFreight.Text.Trim, 0) Then
            _QM.freight = Decimal.Parse(TBFreight.Text.Trim)
            MyUtil.Current.CurrentDataContext.SubmitChanges()
        End If
        getPageStr(Request("UID"), Me.RadioButtonList_PriviewMode.SelectedValue)
        Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub rdship_CheckedChanged(sender As Object, e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, RadioButton).NamingContainer, GridViewRow)
        Dim _ExpressCompany As String = row.Cells(1).Text
        Dim _Freight As String = gvship.DataKeys(row.DataItemIndex).Values(0)
        Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
        If _ME Is Nothing Then
            _ME = New Quote_Master_Extension
            _ME.QuoteID = UID
        End If
        _ME.ExpressCompany = _ExpressCompany
        MyQuoteX.LogQuoteMasterExtension(_ME)
        Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(Me.quoteId)
        If _QM IsNot Nothing AndAlso Decimal.TryParse(_Freight, 0) Then
            _QM.freight = Decimal.Parse(_Freight)
            MyUtil.Current.CurrentDataContext.SubmitChanges()
        End If
        Dim _NoteText As String = String.Empty
        Dim NoteText As Object = tbOPBase.dbExecuteScalar("EQ", String.Format("SELECT  TOP 1 NOTETEXT from QuotationNote where QuoteID ='{0}' AND notetype ='SalesNote'", Me.quoteId))
        If NoteText IsNot Nothing Then
            _NoteText = NoteText.ToString()
        End If
        _NoteText = _NoteText + vbTab + String.Format("To Shipping: Signature Req. Online Freight: {0}. .**ship to address verified**", _Freight)
        Dim sql As String = String.Format("delete from QuotationNote where QuoteID ='{0}' AND notetype ='{1}';insert QuotationNote (quoteid,notetype,notetext) values('{0}','{1}','{2}')", Me.quoteId, "SalesNote", _NoteText)
        tbOPBase.dbExecuteNoQuery("EQ", sql)
        getPageStr(Request("UID"), Me.RadioButtonList_PriviewMode.SelectedValue)
        If Session("ShipRatedt") IsNot Nothing Then
            gvship.DataSource = CType(Session("ShipRatedt"), DataTable)
            gvship.DataBind()
        End If
        Me.UP_QuotationPreview.Update()
    End Sub

    Protected Sub gvship_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvship.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
            If _ME IsNot Nothing Then
                ' Dim RadioButtionRB As RadioButton = DirectCast(e.Row.FindControl("rdship"), RadioButton)
                'RadioButtionRB.Checked = False    
                '  Label1.Text = _ME.ExpressCompany + vbTab + e.Row.Cells(1).Text.Trim + "<hr>"
                If _ME.ExpressCompany IsNot Nothing AndAlso _ME.ExpressCompany.Trim() = System.Web.HttpUtility.HtmlDecode(e.Row.Cells(1).Text.Trim) Then
                    'RadioButtionRB.Checked = True
                    e.Row.Cells(0).Text = String.Format("<input name='rdship' type='radio'   data-freight='{0}'  data-name='{1}' checked='checked' />", gvship.DataKeys(e.Row.RowIndex).Value, e.Row.Cells(1).Text.Trim)
                End If
            End If
        End If
    End Sub

    Protected Sub btConfirm_Click(sender As Object, e As EventArgs) Handles btConfirm.Click
        Dim _ExpressCompany As String = HFdataname.Value.Trim
        Dim _Freight As String = HFdatafreight.Value
        Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
        If _ME Is Nothing Then
            _ME = New Quote_Master_Extension
            _ME.QuoteID = UID
        End If
        _ME.ExpressCompany = _ExpressCompany
        MyQuoteX.LogQuoteMasterExtension(_ME)
        Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(Me.quoteId)
        If _QM IsNot Nothing AndAlso Decimal.TryParse(_Freight, 0) Then
            _QM.freight = Decimal.Parse(_Freight)
            Dim _Inco2 As String = _ExpressCompany
            If _ExpressCompany.Length > 28 Then _Inco2 = _ExpressCompany.Substring(0, 28)
            _QM.INCO2 = IIf(_Inco2 = "N/A", "", _Inco2)
            MyUtil.Current.CurrentDataContext.SubmitChanges()
        End If
        'Dim _NoteText As String = String.Empty
        'Dim NoteText As Object = tbOPBase.dbExecuteScalar("EQ", String.Format("SELECT  TOP 1 NOTETEXT from QuotationNote where QuoteID ='{0}' AND notetype ='SalesNote'", Me.quoteId))
        'If NoteText IsNot Nothing Then
        '    _NoteText = NoteText.ToString()
        'End If
        '_NoteText = _NoteText + vbTab + String.Format("To Shipping: Signature Req. Online Freight: {0}. .**ship to address verified**", _Freight)
        'Dim sql As String = String.Format("delete from QuotationNote where QuoteID ='{0}' AND notetype ='{1}';insert QuotationNote (quoteid,notetype,notetext) values('{0}','{1}','{2}')", Me.quoteId, "SalesNote", _NoteText)
        'tbOPBase.dbExecuteNoQuery("EQ", sql)
        getPageStr(Request("UID"), Me.RadioButtonList_PriviewMode.SelectedValue)
        If Session("ShipRatedt") IsNot Nothing Then
            gvship.DataSource = CType(Session("ShipRatedt"), DataTable)
            gvship.DataBind()
        End If
        Me.UP_QuotationPreview.Update()
    End Sub
    ''' <summary>
    ''' 单独点击按钮时，才去调用eStore WS 去抓取运费资料，只针对AUS
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btAUSfreight_Click(sender As Object, e As EventArgs) Handles btAUSfreight.Click
        gvship.Visible = True : labnomsg.Text = "" : tbbt.Visible = True
        Dim AUSShipRatedt As DataTable = Util.getNewShipRate(Me.quoteId)
        If AUSShipRatedt IsNot Nothing AndAlso AUSShipRatedt.Rows.Count > 0 Then
            HFcopytext.Value = String.Empty
            For Each curdr As DataRow In AUSShipRatedt.Rows
                HFcopytext.Value += curdr.Item(2).ToString() + Environment.NewLine
            Next
            Dim dr As DataRow = AUSShipRatedt.NewRow()
            dr("name") = "N/A" : dr("freight") = "0"
            AUSShipRatedt.Rows.InsertAt(dr, 0)
            gvship.DataSource = AUSShipRatedt
            gvship.DataBind()
        Else
            gvship.Visible = False : tbbt.Visible = False
            labnomsg.Text = "System is busy, please try again later."
        End If
        Me.UPAUSFreight.Update()
        ScriptManager.RegisterStartupScript(Me.UPAUSFreight, Me.GetType(), "myjs", "   $(function () {  return ShowFreight();  })", True)
    End Sub
    ''' <summary>
    ''' 欧洲添加Refresh按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btRefresh_Click(sender As Object, e As EventArgs) Handles btRefresh.Click
        Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(Me.quoteId)
        labnodata.Text = ""
        Dim ShipRatedt As DataTable = Util.getNewShipRate(Me.quoteId)
        'ShipRatedt = Nothing
        If ShipRatedt IsNot Nothing AndAlso ShipRatedt.Rows.Count > 0 Then
            For Each dr As DataRow In ShipRatedt.Rows
                If dr("name") IsNot Nothing AndAlso String.Equals(dr("name"), "TNT Economy", StringComparison.CurrentCultureIgnoreCase) Then
                    If dr("freight") IsNot Nothing AndAlso Decimal.TryParse(dr("freight"), 0) Then
                        TBFreight.Text = dr("freight")
                        If _IsAEUUser AndAlso _QM IsNot Nothing Then
                            _QM.freight = Decimal.Parse(dr("freight"))
                            MyUtil.Current.CurrentDataContext.SubmitChanges()
                        End If
                        Exit For
                    End If
                End If
            Next
            getPageStr(Request("UID"), Me.RadioButtonList_PriviewMode.SelectedValue)
        Else
            'ScriptManager.RegisterStartupScript(Me.UP_QuotationPreview, Me.GetType(), "myjs", "   $(function () { ShowMasterErr('Alert','System is busy, please try again later.',5);  })", True)
            labnodata.Text = "System is busy, please click the ""Refresh"" button to try again later."
        End If
        Me.UP_QuotationPreview.Update()
    End Sub

End Class