Public Class OrderInfo
    Inherits PageBase
    Dim rbtnIsPartial As RadioButtonList = Nothing, txtShipTo As TextBox = Nothing, txtShipToAttention As TextBox = Nothing, txtBillTo As TextBox = Nothing
    Dim EQpaymentTerm As String = ""

    Private _DetailRef As IBUS.iCart(Of IBUS.iCartLine) = Nothing
    Public ReadOnly Property DetailRef As IBUS.iCart(Of IBUS.iCartLine)
        Get
            If IsNothing(_DetailRef) Then
                _DetailRef = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Me.MasterRef.Org)
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

    Private Property QuoteNotes As Object

    Protected Sub FillSalesEmployees(ByVal Org As String)
        Dim SalesEmployees As DataTable = Pivot.NewObjDoc.getSalesEmployeeList(Org)
        SalesEmployees.Columns.Add("DisplayName", GetType(String), "FULL_NAME + ' ('+ SALES_CODE +')'")
        ddlSE.DataTextField = "DisplayName" : ddlSE.DataValueField = "SALES_CODE"
        ddlSE.DataSource = SalesEmployees
        ddlSE.DataBind()
        ddlSE.Items.Insert(0, New ListItem("Select…", ""))
    End Sub
    Protected Function GetPrimarySales(ByVal companyId As String, ByVal OrgId As String) As String
        Dim sql As New StringBuilder()
        sql.Append(" select TOP 1  isnull(SALES_CODE,'') as salescode from SAP_COMPANY_PARTNERS ")
        sql.AppendFormat(" where COMPANY_ID='{0}' and ORG_ID='{1}' and PARTNER_FUNCTION='VE' ", companyId, OrgId)
        sql.Append("  and SALES_CODE<>'00000000' order by SALES_CODE ")
        Dim objsale As Object = tbOPBase.dbExecuteScalar("MY", sql.ToString)
        If objsale IsNot Nothing Then Return objsale.ToString.Trim
        Return ""
    End Function
    Sub setPropUCS()
        Me.soldtoaddress.QM = MasterRef
        Me.shiptoaddress.QM = MasterRef
        Me.billtoaddress.QM = MasterRef
        Me.endcustomer.QM = MasterRef
        Me.PartialDeliver1.QM = MasterRef
        Me.newbilladdress.QM = MasterRef
    End Sub
    Public Sub PickCalEnd(ByVal Arg As Date)
        Dim PickedCalDate As Date = Arg
        Me.txtreqdate.Text = PickedCalDate.ToString(Pivot.CurrentProfile.DatePresentationFormat)
        Me.UpReqDate.Update()
        Me.MPPickCalendar.Hide()
    End Sub
    Public Function CheckStatus(ByVal DocStatus As COMM.Fixer.eDocStatus) As Boolean
        If DocStatus = CInt(COMM.Fixer.eDocStatus.QFINISH) Then
            Return True
        End If
        Return False
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Me.MasterRef) Then


            If MasterRef.DocType <> COMM.Fixer.eDocType.EQ Then
                Util.showMessage("Doc Type is invalid, Must be an Quotation.", "back")
            End If
            If MasterRef.DOCSTATUS <> COMM.Fixer.eDocStatus.QFINISH Then
                Util.showMessage("Doc Status is invalid, Must be an Finished Quotation.", "back")
            End If

            setPropUCS()
            rbtnIsPartial = CType(Me.PartialDeliver1.FindControl("rbtnIsPartial"), RadioButtonList)
            txtShipTo = CType(Me.shiptoaddress.FindControl("txtShipTo"), TextBox)
            txtShipToAttention = CType(Me.shiptoaddress.FindControl("txtShipToAttention"), TextBox)
            txtBillTo = CType(Me.billtoaddress.FindControl("txtShipTo"), TextBox)
            If Not Page.IsPostBack Then
                CType(Me.ascxCalendar.FindControl("hCompany"), HiddenField).Value = MasterRef.AccErpId
                If Me.MasterRef.AccErpId = "SAID" Then
                    Me.trDelPlant.Visible = True
                End If
                If String.Equals(Me.MasterRef.AccErpId, "EDDEAM01", StringComparison.CurrentCultureIgnoreCase) Then
                    trSE.Visible = True
                    FillSalesEmployees(Me.MasterRef.org)
                    Dim custom_salescode As New ArrayList
                    custom_salescode.Add("30040003")
                    Dim GetPrimary_Sales As String = GetPrimarySales(Me.MasterRef.AccErpId, Me.MasterRef.org)
                    If Not String.IsNullOrEmpty(GetPrimary_Sales) Then custom_salescode.Add(GetPrimary_Sales)
                    Dim listsales As New List(Of ListItem)
                    For i As Integer = 0 To ddlSE.Items.Count - 1
                        If custom_salescode.Contains(ddlSE.Items(i).Value) Then
                            listsales.Add(ddlSE.Items(i))
                        End If
                    Next
                    ddlSE.Items.Clear()
                    For Each i As ListItem In listsales
                        ddlSE.Items.Add(i)
                    Next
                    ddlSE.Items.Insert(0, New ListItem("Select…", ""))
                End If
                If String.Equals(Me.MasterRef.AccErpId, "UUMM001", StringComparison.CurrentCultureIgnoreCase) Then
                    tdendcustomer.Visible = True
                End If

                If Role.IsInternalUser() AndAlso (Role.IsFranchiser Or Role.IsInternalUser()) Then
                    ' Set  Sales Employee
                    trSE.Visible = True
                    FillSalesEmployees(Me.MasterRef.org)
                    ddlSE2.Items.Clear() : ddlSE3.Items.Clear()
                    For Each r As ListItem In ddlSE.Items
                        ddlSE2.Items.Add(New ListItem(r.Text, r.Value))
                        ddlSE3.Items.Add(New ListItem(r.Text, r.Value))
                    Next
                    Dim KeyInPersonDT As DataTable = Pivot.NewObjDoc.GetKeyInPerson(Pivot.CurrentProfile.UserId)
                    If KeyInPersonDT.Rows.Count > 0 Then
                        KeyInPersonDT.Columns.Add("DisplayName", GetType(String), "FULL_NAME + ' ('+ SALES_CODE +')'")
                        ddlKeyInPerson.DataTextField = "DisplayName" : ddlKeyInPerson.DataValueField = "SALES_CODE"
                        ddlKeyInPerson.DataSource = KeyInPersonDT
                        ddlKeyInPerson.DataBind()
                        trKeyInPerson.Visible = True
                    Else
                        trKeyInPerson.Visible = False
                    End If
                End If

                initInterface()

                If Role.IsFranchiser Then
                    trSN.Visible = False : trBillInfo.Visible = False : trFreTax.Visible = False
                End If
                If Role.IsInternalUser AndAlso (Role.IsFranchiser Or Role.IsInternalUser) Then
                    trPayTerm.Visible = True

                    'If Util.IsAEUIT() Or User.Identity.Name.EndsWith("@advantech.com", StringComparison.CurrentCultureIgnoreCase) Then
                    If Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin Or Role.IsUsaUser Then
                        trDSGSO.Visible = True
                    End If

                    '20120716 TC: Show ucShipToUS for US Employees
                    If MasterRef.org.ToString.Equals("US01", StringComparison.OrdinalIgnoreCase) Or Role.IsFranchiser Then
                        Me.spShipc.Visible = False : Me.drpShipCondition.Visible = False
                        tdbilltoascx.Visible = True : tdbilltoascx.Visible = True : tyEarlyShip.Visible = True
                        tdE2name.Visible = True : tdE3name.Visible = True : tdE2.Visible = True : tdE3.Visible = True
                        litRD.Text = "Req delivery date" : litRDF.Text = "yyyy/MM/dd"
                        If Date.TryParse(txtreqdate.Text, Now) = True Then
                            txtreqdate.Text = CDate(txtreqdate.Text).ToString("yyyy/MM/dd")
                        End If

                        If drpIncoterm.Items.FindByValue("FB1") IsNot Nothing Then
                            drpIncoterm.SelectedValue = "FB1"
                            drpIncoterm.Enabled = False
                        End If
                    End If
                    'Get all regional payment term options
                    dlPayterm.DataSource = tbOPBase.dbGetDataTable("MY", _
                        " select distinct CREDIT_TERM from SAP_DIMCOMPANY where ORG_ID='" + MasterRef.org + "' and CREDIT_TERM is not null " + _
                        " and CREDIT_TERM <> '' order by CREDIT_TERM")
                    'Get current customer's payment term
                    If String.IsNullOrEmpty(EQpaymentTerm) Then
                        Dim objcustCTerm As Object = tbOPBase.dbExecuteScalar("MY", _
                       String.Format("select top 1 CREDIT_TERM from SAP_DIMCOMPANY where company_id='{0}' and org_id='{1}'", _
                                    MasterRef.AccErpId, MasterRef.org))
                        If objcustCTerm IsNot Nothing AndAlso Not String.IsNullOrEmpty(objcustCTerm.ToString) Then
                            EQpaymentTerm = objcustCTerm.ToString
                        End If
                    End If

                    dlPayterm.DataTextField = "CREDIT_TERM" : dlPayterm.DataValueField = "CREDIT_TERM" : dlPayterm.DataBind()
                    If dlPayterm.Items.Count > 0 AndAlso Not String.IsNullOrEmpty(EQpaymentTerm) Then
                        dlPayterm.SelectedIndex = -1
                        For Each liCreditTermItem As ListItem In dlPayterm.Items
                            If String.Equals(liCreditTermItem.Value, EQpaymentTerm, StringComparison.CurrentCultureIgnoreCase) Then
                                liCreditTermItem.Selected = True
                            End If
                        Next
                    End If
                    dlPayterm_SelectedIndexChanged(Nothing, Nothing)
                End If
                If Me.MasterRef.AccErpId.ToString.Equals("ULTR00001", StringComparison.CurrentCultureIgnoreCase) OrElse _
                 Role.IsUsaUser OrElse Pivot.CurrentProfile.UserId.ToString.Equals("jessamine.ku@advantech.com.tw", StringComparison.CurrentCultureIgnoreCase) Then
                    Me.shiptoaddress.Editable = True
                Else
                    Me.shiptoaddress.Editable = False
                End If


                '  Get po number and ShipCondition for upload order
                'Dim PoNo As String = String.Empty, ShipCondition As String = String.Empty
                'Dim retint As Integer = OrderUtilities.GetParsForUploadOrder(CartId, "", PoNo, ShipCondition)
                'If retint = 1 Then
                '    txtPONo.Text = PoNo
                '    For Each itemsc As ListItem In drpShipCondition.Items
                '        If String.Equals(itemsc.Text.Trim.Replace(" ", ""), ShipCondition.Replace(" ", "")) Then
                '            drpShipCondition.SelectedValue = itemsc.Value
                '        End If
                '    Next
                'End If

            End If
        End If
    End Sub
    Sub initInterface()
        initShipConDrp() : initIncoDrp()
        For i As Integer = Now.Year To Now.Year + 15
            dlCCardExpYear.Items.Add(New ListItem(i.ToString(), i.ToString()))
        Next
        Dim PoNum As String = "", attention As String = "", isPartial As String = "", shipCon As String = "", incotermdrp As String = ""
        Dim incotermText As String = "", orderNote As String = "", salesNote As String = "", opNote As String = "", pjNote As String = ""
        Dim DMFflag As String = "", ShipTodrp As String = "", ShipToText As String = "", ShipToAtt As String = ""
        ' nada adjusted ''ming get local time
        Dim localtime As DateTime = Pivot.NewObjDoc.GetLocalTime(Me.MasterRef.Org.ToString.Substring(0, 2))
        Dim reqDate As String = Now
        Dim Doc As IBUS.iDoc = Pivot.NewObjDoc
        Dim PAT As IBUS.iPatch = Pivot.NewObjPatch
        Dim CL As IBUS.iCartList = Me.DetailRef.GetListAll(COMM.Fixer.eDocType.ORDER)
        Dim ISHASBTO As Boolean = PAT.isHasBto(CL)
        Dim ISSBCBTO As Boolean = PAT.isSBCBtoOrder(CL, MasterRef.AccErpId)
        If Not MasterRef.org.ToString.Trim.Equals("US01", StringComparison.OrdinalIgnoreCase) Then
            reqDate = DateAdd(DateInterval.Day, 1, localtime).Date.ToString(COMM.Fixer.eDateFormat.SAPDATE)
            If ISHASBTO Then
                reqDate = Doc.getBTOParentDueDate(reqDate, MasterRef.org)
            Else
                reqDate = Doc.getCompNextWorkDate(reqDate, MasterRef.org)
            End If
        End If
        If Me.MasterRef.org = "US01" Then
            If ISHASBTO Then
                If ISSBCBTO Then
                    reqDate = Doc.getCompNextWorkDate(localtime, MasterRef.org, 1) ' SBC: +1
                Else
                    reqDate = Doc.getCompNextWorkDate(localtime, MasterRef.org, COMM.Fixer.eBTOAssemblyDays.US) ' Normal: +5
                End If
            Else
                If localtime.Hour >= 13 Then
                    reqDate = Doc.getCompNextWorkDate(localtime, MasterRef.org, 1) ' not BTOS & > 13:00: +1
                Else
                    reqDate = localtime.Date.ToString(Pivot.CurrentProfile.DatePresentationFormat) ' not BTOS & < 13:00: +0
                End If
            End If
        End If

        Me.txtreqdate.Text = reqDate

        'end
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", String.Format("select * from SAP_DIMCOMPANY where company_id='{0}' and org_id='{1}'", MasterRef.AccErpId, MasterRef.org))
        If dt.Rows.Count > 0 Then
            attention = dt.Rows(0).Item("ATTENTION") : shipCon = dt.Rows(0).Item("SHIPCONDITION")
            If Not IsDBNull(dt.Rows(0).Item("INCO1")) Then
                incotermdrp = dt.Rows(0).Item("INCO1")
            End If
            If Not IsDBNull(dt.Rows(0).Item("INCO2")) Then
                incotermText = dt.Rows(0).Item("INCO2")
            End If
        End If

        '20120503 TC: If company id has weekly ship date setup in SAP, then get nearest ship week date
        Dim tmpNextWeekShipDate As Date = CDate(Me.txtreqdate.Text)
        If Doc.GetNextWeeklyShippingDate(CDate(Me.txtreqdate.Text), tmpNextWeekShipDate, MasterRef.AccErpId) Then Me.txtreqdate.Text = tmpNextWeekShipDate.ToString("yyyy/MM/dd")


        ''Frank 2012/11/23:If Order was created by uploading excel file,then getting max require date of cart detail and to be req_date
        'Dim _upDA As New MyCartDSTableAdapters.UPLOAD_ORDER_PARATableAdapter
        'Dim _UploadFromExcelCount As Integer = _upDA.GetCountByCartID(Me.CartId)
        'If _UploadFromExcelCount > 0 Then
        '    Dim _sql As String = "Select Max(req_date) as Max_Req_Date From CART_DETAIL Where Cart_Id='" & Me.CartId & "'"
        '    Dim _dtMaxReqDate As DataTable = dbUtil.dbGetDataTable("MY", _sql)
        '    If _dtMaxReqDate IsNot Nothing AndAlso _dtMaxReqDate.Rows.Count > 0 Then Me.txtreqdate.Text = Format(_dtMaxReqDate.Rows(0).Item("Max_Req_Date"), "yyyy/MM/dd")
        'End If
        If Me.tbExempt.Visible = True Then
            Me.cbxIsTaxExempt.Checked = IIf(SAPDAL.SAPDAL.isTaxExempt(MasterRef.AccErpId), 1, 0)
        End If

        Dim quoteId As String = ""
        If True Then
            'orderaddressesforus.Visible = True
            Dim Partners As List(Of IBUS.iPartnerLine) = Pivot.NewObjPartner.GetListAll(UID, COMM.Fixer.eDocType.EQ)

            ' Dim QuoteDetail As EQDS.QuotationDetailDataTable = Nothing, QuotePartner As EQDS.EQPARTNERDataTable = Nothing, QuoteNotes As EQDS.QuotationNoteDataTable = Nothing
            If Partners IsNot Nothing AndAlso Partners.Count > 0 Then
                For Each partner As IBUS.iPartnerLine In Partners
                    With partner
                        Dim _address As OrderAddress = Nothing
                        If .TYPE.ToUpper = "SOLDTO" Then
                            _address = soldtoaddress
                        ElseIf .TYPE.ToUpper = "S" Then
                            _address = shiptoaddress
                        ElseIf .TYPE.ToUpper = "B" Then
                            _address = billtoaddress
                        End If
                        If _address IsNot Nothing Then
                            If String.IsNullOrEmpty(.ERPID) AndAlso .TYPE.ToUpper <> "B" Then
                                _address.ERPID = MasterRef.AccErpId
                            Else
                                _address.ERPID = .ERPID
                            End If
                            _address.Name = .NAME
                            _address.Tel = .TEL : _address.Attention = .ATTENTION
                            _address.City = .CITY : _address.State = .STATE
                            _address.Street = .STREET : _address.Zipcode = .ZIPCODE
                            _address.Country = .COUNTRY : _address.Street2 = .STREET2
                        End If
                        If .TYPE.Trim.Equals("E", StringComparison.OrdinalIgnoreCase) Then
                            If Not IsDBNull(.ERPID) AndAlso Not String.IsNullOrEmpty(.ERPID) Then
                                If ddlSE.Items.FindByValue(.ERPID) IsNot Nothing Then
                                    ddlSE.SelectedValue = .ERPID
                                End If
                            End If
                        End If
                        If .TYPE.Trim.Equals("E2", StringComparison.OrdinalIgnoreCase) Then
                            If Not IsDBNull(.ERPID) AndAlso Not String.IsNullOrEmpty(.ERPID) Then
                                If ddlSE2.Items.FindByValue(.ERPID) IsNot Nothing Then
                                    ddlSE2.SelectedValue = .ERPID
                                End If
                            End If
                        End If
                        If .TYPE.Trim.Equals("E3", StringComparison.OrdinalIgnoreCase) Then
                            If Not IsDBNull(.ERPID) AndAlso Not String.IsNullOrEmpty(.ERPID) Then
                                If ddlSE3.Items.FindByValue(.ERPID) IsNot Nothing Then
                                    ddlSE3.SelectedValue = .ERPID
                                End If
                            End If
                        End If
                    End With
                Next
                Dim ML As IBUS.iDocHeaderLine = MasterRef
                With ML
                    If Not String.IsNullOrEmpty(.PO_NO) Then
                        txtPONo.Text = .PO_NO
                    End If
                    If Not String.IsNullOrEmpty(.shipTerm) AndAlso .shipTerm.Equals("EX Works", StringComparison.OrdinalIgnoreCase) Then
                        shipCon = .shipTerm
                    End If
                    If Not IsDBNull(.DIST_CHAN) AndAlso Not String.IsNullOrEmpty(.DIST_CHAN) Then
                        If dlDistChann.Items.FindByValue(.DIST_CHAN) IsNot Nothing Then
                            dlDistChann.SelectedValue = .DIST_CHAN : dlDistChann_SelectedIndexChanged(Nothing, Nothing)
                        End If
                    End If

                    If Not IsDBNull(.paymentTerm) AndAlso Not String.IsNullOrEmpty(.paymentTerm) Then
                        'If dlPayterm.Items.FindByValue(.paymentTerm) IsNot Nothing Then
                        '    dlPayterm.SelectedValue = .paymentTerm : dlPayterm_SelectedIndexChanged(Nothing, Nothing)
                        'End If
                        EQpaymentTerm = .paymentTerm
                    End If
                    If Not IsDBNull(.DIVISION) AndAlso Not String.IsNullOrEmpty(.DIVISION) Then
                        If ddlDivision.Items.FindByValue(.DIVISION) IsNot Nothing Then
                            ddlDivision.SelectedValue = .DIVISION
                        End If
                    End If
                    If Not IsDBNull(.AccGroupCode) AndAlso Not String.IsNullOrEmpty(.AccGroupCode) Then
                        If ddlSalesGroup.Items.FindByValue(.AccGroupCode) IsNot Nothing Then
                            ddlSalesGroup.SelectedValue = .AccGroupCode
                        End If
                    End If
                    If Not IsDBNull(.AccOfficeCode) AndAlso Not String.IsNullOrEmpty(.AccOfficeCode) Then
                        If ddlSalesOffice.Items.FindByValue(.AccOfficeCode) IsNot Nothing Then
                            ddlSalesOffice.SelectedValue = .AccOfficeCode
                        End If
                    End If
                    If Not IsDBNull(.SalesDistrict) AndAlso Not String.IsNullOrEmpty(.SalesDistrict) Then
                        txtSalesDistrict.Text = .SalesDistrict
                    End If
                    If Not IsDBNull(.Inco1) AndAlso Not String.IsNullOrEmpty(.Inco1) Then
                        If drpIncoterm.Items.FindByValue(.Inco1) IsNot Nothing Then
                            incotermdrp = .Inco1
                        End If
                    End If
                    If Not IsDBNull(.Inco2) AndAlso Not String.IsNullOrEmpty(.Inco2) Then
                        txtIncoterm.Text = .Inco2 : incotermText = txtIncoterm.Text
                    End If
                    If .isExempt = 1 Then
                        cbxIsTaxExempt.Checked = True
                    Else
                        cbxIsTaxExempt.Checked = False
                    End If
                End With
            End If
            Dim Notes As List(Of IBUS.iNoteLine) = Pivot.NewObjNote.GetListAll(UID, COMM.Fixer.eDocType.EQ)
            If Notes IsNot Nothing AndAlso Notes.Count > 0 Then
                For Each dr As IBUS.iNoteLine In Notes
                    If dr.notetype.Trim.Equals("SalesNote", StringComparison.CurrentCultureIgnoreCase) Then
                        txtSalesNote.Text = dr.notetext
                    End If
                    If dr.notetype.Trim.Equals("OrderNote", StringComparison.CurrentCultureIgnoreCase) Then
                        txtOrderNote.Text = dr.notetext
                    End If
                Next
            End If
        End If
        Me.drpShipCondition.SelectedValue = shipCon : Me.drpIncoterm.SelectedValue = incotermdrp
        Me.txtIncoterm.Text = incotermText
        'Nada modified to append sales note and ctos note
        'Me.txtSalesNote.Text += vbLf + MYSAPBIZ.getSalesNotebyCustomer(Session("Company_id")).Trim()
        Me.txtSalesNote.Text += vbLf + Pivot.NewObjCustomer.getSalesNotebyCustomer(MasterRef.AccErpId).Trim()
        'Dim cnot As String = Relics.SAPDAL.GetCTOSAssemblyInstructionListByERPIdFromMyadvantech(MasterRef.AccErpId)
        'If Not String.IsNullOrEmpty(cnot.Trim().Trim("****")) Then
        'Me.txtSalesNote.Text += vbLf + "CTOS Special Introduction : " + vbLf + cnot
        'End If
        'Me.txtSalesNote.Text += vbLf + Pivot.NewObjCustomer.getSalesNotebyCustomer(MasterRef.AccErpId).Trim()
        If MasterRef.org.ToString.Equals("EU10", StringComparison.OrdinalIgnoreCase) AndAlso Not Role.IsInternalUser Then
            Me.trSN.Visible = False ' : Me.trPJN.Visible = False : Me.trOPN.Visible = False 
        End If
        Me.trOPN.Visible = False
        If MasterRef.org.ToString.Equals("EU10", StringComparison.OrdinalIgnoreCase) Then
            Me.trOPN.Visible = True
        End If
        If Role.IsInternalUser Then trBillInfo.Visible = True
        If MasterRef.org.ToString.Equals("US01", StringComparison.OrdinalIgnoreCase) AndAlso Role.IsInternalUser Then
            Me.tbExempt.Visible = True
        End If
    End Sub


    Sub initShipConDrp()
        Dim dt As DataTable = tbOPBase.dbGetDataTable("my", String.Format("select distinct VSBED AS SHIPCONDITION,'' as SHIPCONTXT from SAP_SHIPCONDITION_BY_PLANT where WERKS like '" & Left(MasterRef.org, 2) & "%'"))
        If dt.Rows.Count > 0 Then
            For I As Integer = 0 To dt.Rows.Count - 1
                dt.Rows(I).Item("SHIPCONTXT") = Pivot.NewObjDoc.shipCode2Txt(dt.Rows(I).Item("SHIPCONDITION"))
            Next
        End If
        Me.drpShipCondition.DataSource = dt : Me.drpShipCondition.DataTextField = "SHIPCONTXT" : Me.drpShipCondition.DataValueField = "SHIPCONDITION" : Me.drpShipCondition.DataBind()
    End Sub
    Sub initIncoDrp()
        Dim dt As DataTable = tbOPBase.dbGetDataTable("my", String.Format("select distinct isnull(INCO1,'') as INCO1 from SAP_DimCompany"))
        Me.drpIncoterm.DataSource = dt : Me.drpIncoterm.DataTextField = "INCO1" : Me.drpIncoterm.DataValueField = "INCO1" : Me.drpIncoterm.DataBind()
    End Sub
    Sub ReloadLine(ByRef H As IBUS.iDocHeaderLine)
        H.PartialF = Me.rbtnIsPartial.SelectedValue
        H.ReqDate = CDate(Me.txtreqdate.Text)
        H.PO_NO = Me.txtPONo.Text
        H.shipTerm = Me.drpShipCondition.SelectedValue
        H.Inco1 = Me.drpIncoterm.SelectedValue
        H.Inco2 = Me.txtIncoterm.Text
        H.DocRealType = "ZOR2"
        H.DocType = COMM.Fixer.eDocType.ORDER
        H.quoteNo = ""
        If Role.IsCNAonlineSales Then
            H.DocRealType = "ZOR"
        End If
        If Pivot.NewObjPatch.isODMCart(DetailRef.GetListAll(COMM.Fixer.eDocType.ORDER)) Then
            H.DocRealType = "ZOR6"
        End If
        Dim D As Date = Now.Date.ToShortDateString
        If Date.TryParse(Me.txtPODate.Text, D) Then
            H.PODate = D
        End If
        H.OriginalQuoteID = UID
        H.paymentTerm = Me.dlPayterm.SelectedValue

        If dlDistChann.SelectedIndex > 0 Then
            H.DIST_CHAN = dlDistChann.SelectedValue : H.DIVISION = ddlDivision.SelectedValue : H.AccGroupCode = ddlSalesGroup.SelectedValue : H.AccOfficeCode = ddlSalesOffice.SelectedValue
        End If
        H.SalesDistrict = Me.txtSalesDistrict.Text
    End Sub
    Function DBfromCart2Order() As IBUS.iDocHeaderLine

        Dim F As Boolean = True
        Dim Header As IBUS.iDocHeader = Pivot.NewObjDocHeader
        Dim HeaderLine As IBUS.iDocHeaderLine = Header.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        If Not CheckStatus(HeaderLine.qStatus) Then
            Me.lbConMsg.Text = "Only Finished Quotation Can be flipped to Order."
            Me.btnPIPreview.Enabled = False
        End If
        If IsNothing(HeaderLine) Then
            F = False
        End If
        Dim Part As IBUS.iPartner = Pivot.NewObjPartner
        Dim PartL As List(Of IBUS.iPartnerLine) = Part.GetListAll(UID, COMM.Fixer.eDocType.EQ)
        If IsNothing(PartL) Then
            F = False
        End If
        Dim Cond As IBUS.iCond = Pivot.NewObjCond
        Dim CondL As List(Of IBUS.iCondLine) = Cond.GetListAll(UID)

        Dim Text As IBUS.iDocText = Pivot.NewObjDocText
        Dim TextL As List(Of IBUS.iDocTextLine) = Text.GetListAll(UID)

        Dim Cred As IBUS.iCredit = Pivot.NewObjCred
        Dim CredL As List(Of IBUS.iCreditLine) = Cred.GetListAll(UID)
        Dim doc As IBUS.iDoc = Pivot.NewObjDoc

        Dim CartOrg As IBUS.iCart(Of IBUS.iCartLine) = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Pivot.CurrentProfile.getCurrOrg)

        If F Then
            ReloadLine(HeaderLine)
            HeaderLine = Header.Add(HeaderLine, Pivot.CurrentProfile, COMM.Fixer.eDocType.ORDER)
            Part.Delete(HeaderLine.Key, COMM.Fixer.eDocType.ORDER)
            For Each R As IBUS.iPartnerLine In PartL
                R.ORDER_ID = HeaderLine.Key 'MasterRef.Key
                Part.Add(R, COMM.Fixer.eDocType.ORDER)
            Next

            If Not IsNothing(CondL) Then
                Cond.Delete(HeaderLine.Key)
                For Each R As IBUS.iCondLine In CondL
                    R.DocId = MasterRef.Key
                    Cond.Add(R)
                Next
            End If
            If Not IsNothing(TextL) Then
                Text.Delete(HeaderLine.Key)
                For Each R As IBUS.iDocTextLine In TextL
                    R.DocId = MasterRef.Key
                    Text.Add(R)
                Next
            End If
            If Not IsNothing(CredL) Then
                Cred.Delete(HeaderLine.Key)
                For Each R As IBUS.iCreditLine In CredL
                    R.DocID = MasterRef.Key
                    Cred.Add(R)
                Next
            End If
            If Not IsNothing(CartOrg) Then
                Dim CartNew As IBUS.iCart(Of IBUS.iCartLine) = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.Order, HeaderLine.Key, Pivot.CurrentProfile.getCurrOrg)
                CartOrg.CopyPaste(CartNew, COMM.Fixer.eDocType.ORDER)

            End If
        End If
        Return HeaderLine
    End Function
    Protected Sub addCC(ByVal OID As String)
        Dim C As IBUS.iCredit = Pivot.NewObjCred
        C.Delete(OID)
        Dim CreditCardExpireDate As DateTime = DateSerial(Integer.Parse(dlCCardExpYear.SelectedValue), Integer.Parse(dlCCardExpMonth.SelectedValue), 1)
        Dim CL As IBUS.iCreditLine = Pivot.NewLineCred
        CL.DocID = OID
        CL.HOLDER = txtCCardHolder.Text
        CL.EXPIRED = CreditCardExpireDate
        CL.TYPE = dlCCardType.SelectedValue
        CL.NUMBER = txtCreditCardNumber.Text
        CL.VERIFICATION_VALUE = txtCCardVerifyValue.Text
        C.Add(CL)
    End Sub
    Protected Sub addFreight(ByVal OID As String)
        Dim F As IBUS.iCond = Pivot.NewObjCond
        F.Delete(OID)
        If IsNumeric(Me.txtFtTax.Text.Trim) Then
            Dim FL As IBUS.iCondLine = Pivot.NewLineCond
            FL.DocId = OID
            FL.Type = "ZHD1"
            FL.Value = Me.txtFtTax.Text.Trim
            F.Add(FL)
        End If
        If IsNumeric(Me.txtFtFre.Text.Trim) Then
            Dim FL As IBUS.iCondLine = Pivot.NewLineCond
            FL.DocId = OID
            FL.Type = "ZHDA"
            FL.Value = Me.txtFtTax.Text.Trim
            F.Add(FL)

        End If
    End Sub
    Protected Sub addText(ByVal OID As String)
        Dim C As IBUS.iDocText = Pivot.NewObjDocText
        C.Delete(OID)
        Dim CL As IBUS.iDocTextLine = Pivot.NewLineDocText
        CL.DocId = OID
        CL.Type = "0002"
        CL.Txt = Me.txtOrderNote.Text
        Dim CL1 As IBUS.iDocTextLine = Pivot.NewLineDocText
        CL1.DocId = OID
        CL1.Type = "0001"
        CL1.Txt = Me.txtSalesNote.Text
        Dim CL2 As IBUS.iDocTextLine = Pivot.NewLineDocText
        CL2.DocId = OID
        CL2.Type = "ZEOP"
        CL2.Txt = Me.txtOPNote.Text
        Dim CL3 As IBUS.iDocTextLine = Pivot.NewLineDocText
        CL3.DocId = OID
        CL3.Type = "ZBIL"
        CL3.Txt = Me.txtBillingInstructionInfo.Text
        C.Add(CL) : C.Add(CL1) : C.Add(CL2) : C.Add(CL3)
    End Sub
    Function VerifyCreditCardInfo() As Integer
        If tbCreditCardInfo.Visible = False Then
            Return 0
        End If
        lbCCardMsg.Text = ""
        If trPayTerm.Visible AndAlso String.Equals(dlPayterm.SelectedValue, "CODC", StringComparison.CurrentCultureIgnoreCase) AndAlso tbCreditCardInfo.Visible Then
            If String.IsNullOrEmpty(txtCreditCardNumber.Text) Then
                lbCCardMsg.Text = "Please input credit card number" : Return -1
            End If
            If String.IsNullOrEmpty(txtCCardVerifyValue.Text) Then
                lbCCardMsg.Text = "Please input credit card verification value" : Return -1
            End If
            If String.IsNullOrEmpty(txtCCardHolder.Text) Then
                lbCCardMsg.Text = "Please input credit card holder name" : Return -1
            End If
        End If
        Return 1
    End Function
    Function goNext() As String
        Dim IsCred As Integer = VerifyCreditCardInfo()
        If IsCred < 0 Then
            Return ""
        End If
        If Date.TryParse(txtreqdate.Text, Now) = False Then txtreqdate.Text = Now.ToString("yyyy/MM/dd")
        Dim tmpNextWeekShipDate As Date = CDate(Me.txtreqdate.Text)
        Dim OID As String = DBfromCart2Order().Key : addFreight(OID) : InsertORDER_PARTNERS(OID) : addText(OID) : SyncCustomerID(OID)
        If IsCred > 0 Then
            addCC(OID)
        End If
        Return OID
    End Function
    Protected Sub SyncCustomerID(ByVal OrderID As String)
        Dim P As IBUS.iPartner = Pivot.NewObjPartner
        Dim PL As List(Of IBUS.iPartnerLine) = P.GetListAll(OrderID, COMM.Fixer.eDocType.ORDER)
        For Each op As IBUS.iPartnerLine In PL
            If Not String.IsNullOrEmpty(op.ERPID) AndAlso _
                (String.Equals(op.TYPE, "SOLDTO", StringComparison.CurrentCultureIgnoreCase) _
                 OrElse String.Equals(op.TYPE, "S", StringComparison.CurrentCultureIgnoreCase) _
                 OrElse String.Equals(op.TYPE, "B", StringComparison.CurrentCultureIgnoreCase)) Then
                Dim companycount As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select COUNT(COMPANY_ID) as c  FROM SAP_DIMCOMPANY  where COMPANY_ID ='{0}'", op.ERPID))
                If companycount IsNot Nothing AndAlso Integer.TryParse(companycount, 0) AndAlso Integer.Parse(companycount) = 0 Then
                    ' Dim RURL As String = HttpContext.Current.Server.UrlEncode(String.Format("/admin/SyncCustomer.aspx?companyid={0}&auto=1", op.ERPID))
                    'Server.Execute(String.Format("http://my.advantech.com/SSO.aspx?id={0}&tempid={1}&ReturnUrl={2}", Pivot.CurrentProfile.UserId, Pivot.CurrentProfile.SSOID, RURL))
                    Business.SyncCompanyIdFromSAP(op.ERPID)
                End If
            End If
        Next
    End Sub
    Protected Sub btnPIPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim OID As String = goNext()
        If Not IsNothing(OID) AndAlso Not String.IsNullOrEmpty(OID) Then
            If cbxIsDir.Checked Then
                Response.Redirect("~/Order/OrderDueDate.aspx?UID=" & OID & "&ISDIRSAP=1")
            End If
            Response.Redirect("~/Order/OrderDueDate.aspx?UID=" & OID)
        End If
    End Sub

    Protected Sub InsertORDER_PARTNERS(ByVal OID As String)
        Dim OrderAddressS As OrderAddress() = {Me.soldtoaddress, Me.shiptoaddress, Me.billtoaddress, Me.endcustomer}
        Dim A As IBUS.iPartner = Pivot.NewObjPartner
        A.Delete(OID, COMM.Fixer.eDocType.ORDER)
        For Each OrderAddress As OrderAddress In OrderAddressS
            With OrderAddress
                If Not String.IsNullOrEmpty(.ERPID.Trim) Then
                    Dim PL As IBUS.iPartnerLine = Pivot.NewLinePartner
                    PL.ORDER_ID = OID
                    PL.ERPID = .ERPID
                    PL.NAME = .Name
                    PL.TYPE = .Type
                    PL.ATTENTION = .Attention
                    PL.TEL = .Tel
                    PL.ZIPCODE = .Zipcode
                    PL.COUNTRY = .Country
                    PL.CITY = .City
                    PL.STREET = .Street
                    PL.STATE = .State
                    PL.STREET2 = .Street2
                    A.Add(PL, COMM.Fixer.eDocType.ORDER)
                End If
            End With
        Next
        If ddlSE.SelectedIndex > 0 Then
            Dim PL As IBUS.iPartnerLine = Pivot.NewLinePartner
            PL.ORDER_ID = OID
            PL.ERPID = ddlSE.SelectedValue
            PL.TYPE = "E"
            A.Add(PL, COMM.Fixer.eDocType.ORDER)
        End If
        If ddlSE2.SelectedIndex > 0 Then
            Dim PL As IBUS.iPartnerLine = Pivot.NewLinePartner
            PL.ORDER_ID = OID
            PL.ERPID = ddlSE2.SelectedValue
            PL.TYPE = "E2"
            A.Add(PL, COMM.Fixer.eDocType.ORDER)
        End If
        If ddlSE3.SelectedIndex > 0 Then
            Dim PL As IBUS.iPartnerLine = Pivot.NewLinePartner
            PL.ORDER_ID = OID
            PL.ERPID = ddlSE3.SelectedValue
            PL.TYPE = "E3"
            A.Add(PL, COMM.Fixer.eDocType.ORDER)
        End If
        If ddlKeyInPerson.Items.Count > 0 AndAlso Not String.IsNullOrEmpty(ddlKeyInPerson.SelectedValue.Trim) Then
            Dim PL As IBUS.iPartnerLine = Pivot.NewLinePartner
            PL.ORDER_ID = OID
            PL.ERPID = ddlKeyInPerson.SelectedValue
            PL.TYPE = "KIP"
            A.Add(PL, COMM.Fixer.eDocType.ORDER)
        End If
    End Sub


    Protected Sub dlPayterm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If String.Equals(dlPayterm.SelectedValue, "CODC", StringComparison.CurrentCultureIgnoreCase) Then
            tbCreditCardInfo.Visible = True
        Else
            tbCreditCardInfo.Visible = False
        End If
    End Sub

    Protected Sub dlDistChann_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If dlDistChann.SelectedIndex > 0 Then
            tbDivSalesGrpOffice.Visible = True
            Dim sql As New StringBuilder
            Dim strDivision As String = "", strDistChann As String = "", strSalesGrp As String = "", strSalesOffice As String = ""
            Dim DocU As IBUS.iDoc = Pivot.NewObjDoc

            DocU.GetDefaultDistChannDivisionSalesGrpOfficeByCompanyId(MasterRef.AccErpId, strDistChann, strDivision, strSalesGrp, strSalesOffice)
            sql.Clear()
            sql.AppendFormat("select distinct DIVISION as Value from SAP_COMPANY_LOV where ORG_ID='{0}' order by DIVISION", MasterRef.org)
            ddlDivision.DataTextField = "Value" : ddlDivision.DataValueField = "Value"
            ddlDivision.DataSource = tbOPBase.dbGetDataTable("MY", sql.ToString())
            ddlDivision.DataBind()
            If MasterRef.org.ToString.Equals("US01", StringComparison.OrdinalIgnoreCase) Then
                Dim liDivDoubleZero As ListItem = ddlDivision.Items.FindByValue("00")
                If liDivDoubleZero IsNot Nothing Then ddlDivision.Items.Remove(liDivDoubleZero)
            End If
            If Not String.IsNullOrEmpty(strDivision) Then
                If ddlDivision.Items.FindByValue(strDivision) IsNot Nothing Then
                    ddlDivision.SelectedValue = strDivision
                End If
            End If
            'end
            ' Set  SalesGroup
            sql.Clear()
            sql.AppendFormat("select distinct SALESGROUP as Value from SAP_COMPANY_LOV where ORG_ID='{0}' and SALESGROUP<>'' order by SALESGROUP", MasterRef.org)
            ddlSalesGroup.DataTextField = "Value" : ddlSalesGroup.DataValueField = "Value"
            ddlSalesGroup.DataSource = tbOPBase.dbGetDataTable("MY", sql.ToString())
            ddlSalesGroup.DataBind()
            If Not String.IsNullOrEmpty(strSalesGrp) Then
                If ddlSalesGroup.Items.FindByValue(strSalesGrp) IsNot Nothing Then
                    ddlSalesGroup.SelectedValue = strSalesGrp
                End If
            End If
            'end
            ' Set  SalesOffice
            sql.Clear()
            sql.AppendFormat("select distinct SALESOFFICE as Value from SAP_COMPANY_LOV where ORG_ID='{0}' and SALESOFFICE<>'' order by SALESOFFICE", MasterRef.org)
            ddlSalesOffice.DataTextField = "Value" : ddlSalesOffice.DataValueField = "Value"
            ddlSalesOffice.DataSource = tbOPBase.dbGetDataTable("MY", sql.ToString())
            ddlSalesOffice.DataBind()
            If Not String.IsNullOrEmpty(strSalesOffice) Then
                If ddlSalesOffice.Items.FindByValue(strSalesOffice) IsNot Nothing Then
                    ddlSalesOffice.SelectedValue = strSalesOffice
                End If
            End If
        Else
            tbDivSalesGrpOffice.Visible = False
        End If
    End Sub

    'Protected Sub txtCCardVerifyValue_TextChanged(sender As Object, e As System.EventArgs)
    '    If Not String.IsNullOrEmpty(txtCCardVerifyValue.Text) Then
    '        If String.IsNullOrEmpty(Trim(txtSalesNote.Text)) Then
    '            txtSalesNote.Text = "CVV Code:" + txtCCardVerifyValue.Text
    '        Else
    '            txtSalesNote.Text += " CVV Code:" + txtCCardVerifyValue.Text
    '        End If
    '    End If
    'End Sub

    Protected Sub txtPONo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lbPoDuplicateMsg.Text = ""
        If Not String.IsNullOrEmpty(txtPONo.Text) AndAlso Role.IsInternalUser Then
            Dim poDt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", _
            "select vbeln from saprdp.vbak where BSTNK='" + Replace(txtPONo.Text, "'", "''") + "' and rownum<=20 and vkorg='" + MasterRef.org + _
            "' and auart in ('ZOR','ZOR2') order by erdat desc")
            If poDt.Rows.Count > 0 Then
                Dim arySo As New ArrayList
                For Each poRow As DataRow In poDt.Rows
                    arySo.Add(COMM.Util.RemoveZeroString(poRow.Item("vbeln").ToString()))
                Next
                lbPoDuplicateMsg.Text = "Purchase order number already exists in SO: " + String.Join(",", arySo.ToArray())
            End If
        End If
    End Sub

    Protected Sub lBtnAuthCcInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelCreditAuthInfo.Visible = False
        If String.IsNullOrEmpty(txtCreditCardNumber.Text) OrElse String.IsNullOrEmpty(txtCCardVerifyValue.Text) Then
            Exit Sub
        End If
        Dim cl As IBUS.iCartList = Me.DetailRef.GetListAll(COMM.Fixer.eDocType.EQ)

        Dim decTotalAmount As Decimal = cl.getTotalAmount()
        'Dim txtBillToStreet As String = "", txtBillToZip As String = ""
        'Dim BillSoldToDt As MyOrderDS.ORDER_PARTNERSDataTable = aptOrderPartner.GetPartnerByOrderIDAndType(CartId, "B")
        'If BillSoldToDt.Count = 0 OrElse String.IsNullOrEmpty(BillSoldToDt(0).STREET) OrElse String.IsNullOrEmpty(BillSoldToDt(0).ZIPCODE) Then
        '    BillSoldToDt = aptOrderPartner.GetPartnerByOrderIDAndType(CartId, "SOLDTO")
        '    If BillSoldToDt.Count = 0 Then
        '        Exit Sub
        '    End If
        'End If
        Dim ccaddress As OrderAddress
        If Me.ckbUserNewBillAddress.Checked Then
            ccaddress = Me.newbilladdress
        Else
            ccaddress = Me.billtoaddress
        End If
        Dim txtFirstName As String = "", txtLastName As String = ""
        Dim txtBillToStreet As String = "", txtCity As String = "", txtState As String = "", txtBillToZip As String = ""
        Dim cardholder As String
        If Not String.IsNullOrEmpty(Me.txtCCardHolder.Text.Trim()) Then
            cardholder = Me.txtCCardHolder.Text
        Else
            cardholder = ccaddress.Attention
        End If
        If Not String.IsNullOrEmpty(cardholder) Then
            If cardholder.Contains(" ") Then
                txtFirstName = cardholder.Substring(0, cardholder.LastIndexOf(" "))
                txtLastName = cardholder.Substring(cardholder.LastIndexOf(" ") + 1)
            Else
                txtFirstName = cardholder
            End If
        End If
        txtCity = ccaddress.City
        txtState = ccaddress.State
        txtBillToStreet = ccaddress.Street : txtBillToZip = ccaddress.Zipcode
        Dim retBool As Boolean = False, newaddress As String = ""
        retBool = AuthCreditResult1.Auth(decTotalAmount, txtFirstName, txtLastName, txtBillToStreet, txtCity, txtState, txtBillToZip, txtPONo.Text, txtCreditCardNumber.Text, _
                               txtCCardVerifyValue.Text, New Date(dlCCardExpYear.SelectedValue, dlCCardExpMonth.SelectedValue, 1))

        If retBool Then
            If Not String.IsNullOrEmpty(txtCCardVerifyValue.Text.Trim) Then
                If String.IsNullOrEmpty(txtSalesNote.Text.Trim) Then
                    txtSalesNote.Text = "CVV Code: " + txtCCardVerifyValue.Text.Trim + vbCrLf
                Else
                    txtSalesNote.Text += vbCrLf + "CVV Code: " + txtCCardVerifyValue.Text.Trim
                End If
            End If
            If Me.ckbUserNewBillAddress.Checked Then
                newaddress = "Address: " + newbilladdress.Street + vbTab + newbilladdress.City + vbTab + newbilladdress.State + vbTab + newbilladdress.Country + vbTab + newbilladdress.Zipcode
            Else
                newaddress = "Address: " + billtoaddress.Street + vbTab + billtoaddress.City + vbTab + billtoaddress.State + vbTab + billtoaddress.Country + vbTab + billtoaddress.Zipcode
            End If

            If String.IsNullOrEmpty(txtSalesNote.Text.Trim) Then
                txtSalesNote.Text = newaddress + vbCrLf
            Else
                txtSalesNote.Text += vbCrLf + newaddress
            End If
            If String.IsNullOrEmpty(txtBillingInstructionInfo.Text.Trim) Then
                txtBillingInstructionInfo.Text = "PN Reference: " + AuthCreditResult1.PNReference + vbCrLf
            Else
                txtBillingInstructionInfo.Text += vbCrLf + "PN Reference: " + AuthCreditResult1.PNReference
            End If
        End If
        PanelCreditAuthInfo.Visible = True
    End Sub
    Protected Sub ckbUserNewBillAddress_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.newbilladdress.Visible = ckbUserNewBillAddress.Checked
    End Sub
    Protected Sub lnkCloseCreditCardAuthInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelCreditAuthInfo.Visible = False
    End Sub

  

End Class