Public Class OrderPreview
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
    Public Function IsNumericItem(ByVal part_no As String) As Boolean
        Dim pChar() As Char = part_no.ToCharArray()
        For i As Integer = 0 To pChar.Length - 1
            If Not IsNumeric(pChar(i)) Then
                Return False
                Exit Function
            End If
        Next
        Return True
    End Function

    Public Function getMassage() As String
        'Dim isSimulate As Boolean = False
        'If OID.ToString.Length > 15 Then
        '    isSimulate = True
        'End If
        If Role.IsInternalUser Then

            If Not IsNothing(MasterRef) Then
                Dim ordermasterDR As IBUS.iDocHeaderLine = MasterRef
                If Not IsDBNull(ordermasterDR.qStatus) AndAlso ordermasterDR.qStatus = CInt(COMM.Fixer.eDocStatus.OFINISH) Then
                    Return ""
                End If
            End If
            Dim mm As String = ""
            Dim OPS As IBUS.iOrderProcS = Pivot.NewObjOrderProcS
            Dim Message_DT As List(Of IBUS.iOrderProcSLine) = OPS.GetListAll(UID) 'myFailedOrder.GetDT(String.Format("order_no='{0}'", Request("NO")), "LINE_SEQ")
            If Not IsNothing(Message_DT) Then
                Dim j As Integer = 0
                While j <= Message_DT.Count - 1
                    If Message_DT(j).NUMBER <> "311" And Message_DT(j).NUMBER <> "233" Then
                        mm &= "<font color=""red"">&nbsp;&nbsp;+&nbsp;" & Message_DT(j).MESSAGE & "</font>"
                        mm &= "<br/>"
                    End If
                    j = j + 1
                End While
            End If
            Return mm.Replace(UID, "SO")
        End If
        Return ""
    End Function
    Public Function SOCreateV5(ByVal Order_No As String, ByRef ErrMsg As String, Optional ByVal isSimulate As Boolean = False, Optional ByVal Quote_NO As String = "", Optional ByVal IsCreateSAPQuote As Boolean = False) As Boolean
        Dim REFORDERNO As String = String.Empty
        Dim IB As Integer = 0
        Dim pat As IBUS.iPatch = Pivot.NewObjPatch
        Dim DocU As IBUS.iDoc = Pivot.NewObjDoc
        Dim dtMaster As IBUS.iDocHeaderLine = MasterRef, dtDetail As IBUS.iCartList = DetailRef.GetListAll(COMM.Fixer.eDocType.ORDER)
        Dim myFt As List(Of IBUS.iCondLine) = Pivot.NewObjCond.GetListAll(UID)
        Dim Credit As List(Of IBUS.iCreditLine) = Pivot.NewObjCred.GetListAll(UID)
        Dim Partner As List(Of IBUS.iPartnerLine) = Pivot.NewObjPartner.GetListAll(UID, COMM.Fixer.eDocType.ORDER)
        Dim Txt As List(Of IBUS.iDocTextLine) = Pivot.NewObjDocText.GetListAll(UID)
        Dim LocalTime As DateTime = DocU.GetLocalTime(dtMaster.org.ToString.Substring(0, 2))
        'Frank 2013/07/22
        'Dim Revision_Number As Integer = dtMaster.Revision_Number
        Dim DtCompany As IBUS.iCustomerLine = Pivot.NewObjCustomer.getByErpIdOrg(dtMaster.AccErpId, dtMaster.org)
        Dim PSTS As IBUS.iOrderProcS = Pivot.NewObjOrderProcS()
        'Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        If IsNothing(Partner) OrElse Partner.Count = 0 Then
            ErrMsg = "Order without partner info"
            Return False
        End If

        If IsNothing(dtMaster) Or IsNothing(dtDetail) Then
            ErrMsg = "RAW DATA ERROR!"
            Return False
        End If

        Dim HDT As New SAPDAL.SalesOrder.OrderHeaderDataTable, DDT As New SAPDAL.SalesOrder.OrderLinesDataTable, PDT As New SAPDAL.SalesOrder.PartnerFuncDataTable
        Dim TDT As New SAPDAL.SalesOrder.HeaderTextsDataTable, CODT As New SAPDAL.SalesOrder.ConditionDataTable, CDT As New SAPDAL.SalesOrder.CreditCardDataTable
        'Header
        Dim HDR As SAPDAL.SalesOrder.OrderHeaderRow = HDT.NewRow
        With dtMaster
            REFORDERNO = .quoteNo
            Dim soldtoID As String = .AccErpId
            If IsNothing(DtCompany) Then
                ErrMsg = "Invalid SoldTo!"
                Return False
            End If
            Dim sales_org As String = UCase(.org), distr_chan As String = "10", division As String = "00"
            DocU.VerifyDisChannelAndDivision(soldtoID, distr_chan, division, dtMaster.org)
            HDR.ORDER_TYPE = .DocRealType : HDR.SALES_ORG = sales_org : HDR.DIST_CHAN = distr_chan : HDR.DIVISION = division
            If IsCreateSAPQuote Then
                HDR.ORDER_TYPE = "AG"
                'Dim ActiveVersion As String = String.Empty
                'Pivot.NewObjDocHeader.GetActiveRevisionQuoteIDByQuoteNo(Quote_NO, COMM.Fixer.eDocType.EQ, ActiveVersion)
                'If ActiveVersion IsNot Nothing AndAlso Not String.IsNullOrEmpty(ActiveVersion) Then
                If .Revision_Number > 0 Then
                    'HDR.VERSION = Quote_NO + "V" + ActiveVersion.ToString.Trim
                    'HDR.VERSION = Quote_NO & "V" & Revision_Number
                    HDR.VERSION = "V" & .Revision_Number
                End If
            End If
            If Not String.IsNullOrEmpty(.DIST_CHAN) Then
                HDR.DIST_CHAN = .DIST_CHAN.ToString() : HDR.DIVISION = .DIVISION.ToString()
                HDR.SalesGroup = .AccGroupCode.ToString() : HDR.SalesOffice = .AccOfficeCode
            End If
            HDR.INCO1 = .Inco1
            Dim INCO2 As String = "blank"
            If .Inco2 <> "" Then INCO2 = .Inco2
            HDR.INCO2 = INCO2
            Dim Company_Country As String = ""
            If DtCompany.COUNTRY_NAME <> "" Then
                Company_Country = DtCompany.COUNTRY_NAME
            End If
            If Company_Country.ToUpper = "NL" Then
                HDR.SHIPTO_COUNTRY = Company_Country.ToUpper : HDR.TRIANGULAR_INDICATOR = "X"
            End If
            If .paymentTerm <> "" Then
                HDR.PAYTERM = .paymentTerm
            End If

            HDR.TAX_CLASS = ""
            Dim rd As DateTime = LocalTime
            If CDate(.ReqDate) > rd Then
                rd = CDate(.ReqDate)
            End If
            HDR.REQUIRE_DATE = rd.ToString(COMM.Fixer.eDateFormat.SAPDATE) : HDR.SHIP_CONDITION = Left(.shipTerm, 2)
            HDR.CUST_PO_NO = IIf(.PO_NO = "", REFORDERNO, .PO_NO) : HDR.SHIP_CUST_PO_NO = ""
            HDR.PO_DATE = COMM.Util.FormatSAPDate(.DocDate)
            If .PartialF = "0" Then HDR.PARTIAL_SHIPMENT = "X"
            HDR.EARLY_SHIP = "0001"
            If .AccErpId = "SAID" Then
                HDR.TAXDEL_CTY = "SG" : HDR.TAXDES_CTY = "ID"
            End If
        End With
        If Not String.IsNullOrEmpty(Quote_NO) AndAlso Not IsCreateSAPQuote Then
            HDR.Ref_Doc = Quote_NO
        End If
        If Not String.IsNullOrEmpty(Quote_NO) AndAlso isSimulate = True Then
            HDR.Ref_Doc = Quote_NO
        End If
        If COMM.Util.IsTesting Then
            HDR.DEST_TYPE = 1
        End If
        If Role.IsJPAonlineSales() Then
            HDR.DEST_TYPE = 0
        End If
        HDT.Rows.Add(HDR)
        '/Header
        'Detail
        For Each R As IBUS.iCartLine In dtDetail
            Dim DR As SAPDAL.SalesOrder.OrderLinesRow = DDT.NewRow
            With R
                DR.PART_Dlv = ""

                DR.HIGHER_LEVEL = .parentLineNo.Value
                'End If
                DR.LINE_NO = .lineNo.Value

                'If dtMaster.Rows(0).Item("SOLDTO_ID") = "SAID" Then 
                If Not IsDBNull(.divPlant.Value) AndAlso .divPlant.Value.ToString.Length > 0 Then
                    DR.PLANT = .divPlant.Value
                End If
                If IsNumericItem(.partNo.Value) Then
                    DR.MATERIAL = "00000000" & .partNo.Value
                Else
                    DR.MATERIAL = pat.ReplaceCartBTO(.partNo.Value, dtMaster.org)
                End If
                If Not IsDBNull(.partDesc.Value) Then
                    If Not .partDesc.Value.ToString.Trim.Length > 40 Then
                        DR.Description = .partDesc.Value.ToString.Trim
                    End If
                End If
                DR.CUST_MATERIAL = .CustMaterial.Value : DR.DMF_FLAG = .DMFFlag.Value : DR.QTY = .Qty.Value
                Dim rd As DateTime = LocalTime
                If CDate(.reqDate.Value) > rd Then rd = CDate(.reqDate.Value)
                DR.REQ_DATE = rd.ToString(COMM.Fixer.eDateFormat.SAPDATE) : DR.PRICE = .newunitPrice.Value : DR.CURRENCY = dtMaster.currency
                'ODM Spacial setting 

                If pat.isODMCart(dtDetail) Then
                    DR.PLANT = "TWM3" : DR.ShipPoint = "TWH1"
                    'DR.StorageLoc = "0018"
                End If
                'End ODM Spacial setting
            End With
            DDT.Rows.Add(DR)
        Next
        '/Detail

        'Text
        If Not IsNothing(Txt) Then
            For Each R As IBUS.iDocTextLine In Txt
                Dim TR As SAPDAL.SalesOrder.HeaderTextsRow = TDT.NewRow
                TR.TEXT_ID = R.Type  'SALESNOTE
                TR.LANG_ID = "EN" : TR.TEXT_LINE = R.Txt
                TDT.Rows.Add(TR)
            Next
        End If


        If Not IsNothing(Credit) Then
            For Each R As IBUS.iCreditLine In Credit
                Dim TR As SAPDAL.SalesOrder.CreditCardRow = CDT.NewRow
                TR.HOLDER = R.HOLDER
                TR.EXPIRED = CDate(R.EXPIRED.ToString()).ToString(COMM.Fixer.eDateFormat.SAPDATE)
                TR.TYPE = R.TYPE
                TR.NUMBER = R.NUMBER
                TR.VERIFICATION_VALUE = R.VERIFICATION_VALUE
                CDT.Rows.Add(TR)
            Next
        End If


        '/Text
        'Partner
        Dim TempSOLDTO As String = String.Empty
        Dim TempB As String = String.Empty
        Dim KeyInPerson As String = String.Empty
        For Each r As IBUS.iPartnerLine In Partner
            Dim PR As SAPDAL.SalesOrder.PartnerFuncRow = PDT.NewRow
            PR.NUMBER = r.ERPID.ToUpper.Trim
            If r.TYPE.Equals("SOLDTO", StringComparison.OrdinalIgnoreCase) Then
                PR.ROLE = "AG"
                TempSOLDTO = r.ERPID.ToUpper.Trim
                PDT.Rows.Add(PR)
            ElseIf r.TYPE.Equals("S", StringComparison.OrdinalIgnoreCase) Then
                PR.ROLE = "WE"
                PDT.Rows.Add(PR)
            ElseIf r.TYPE.Equals("B", StringComparison.OrdinalIgnoreCase) Then
                PR.ROLE = "RE"
                TempB = r.ERPID.ToUpper.Trim
                PDT.Rows.Add(PR)
            ElseIf r.TYPE.Equals("E", StringComparison.OrdinalIgnoreCase) Then
                PR.ROLE = "VE"
                PDT.Rows.Add(PR)
            ElseIf r.TYPE.Equals("E2", StringComparison.OrdinalIgnoreCase) Then
                PR.ROLE = "Z2"
                PDT.Rows.Add(PR)
            ElseIf r.TYPE.Equals("E3", StringComparison.OrdinalIgnoreCase) Then
                PR.ROLE = "Z3"
                PDT.Rows.Add(PR)
            ElseIf r.TYPE.Equals("KIP", StringComparison.OrdinalIgnoreCase) Then
                KeyInPerson = r.ERPID.ToUpper.Trim
            ElseIf r.TYPE.Equals("EM", StringComparison.OrdinalIgnoreCase) Then
                PR.ROLE = "EM"
                PDT.Rows.Add(PR)
            ElseIf r.TYPE.Equals("ER_EMPLOYEE", StringComparison.OrdinalIgnoreCase) Then
                PR.ROLE = "ZM"
                PDT.Rows.Add(PR)
            ElseIf r.TYPE.Equals("END_CUST", StringComparison.OrdinalIgnoreCase) Then
                PR.ROLE = "EM"
                PDT.Rows.Add(PR)
            End If
            PDT.AcceptChanges()
        Next

        If Not String.IsNullOrEmpty(TempB) AndAlso Not String.IsNullOrEmpty(TempSOLDTO) Then ' AndAlso TempB <> TempSOLDTO Then
            Dim PR As SAPDAL.SalesOrder.PartnerFuncRow = PDT.NewRow()
            PR.NUMBER = TempB
            PR.ROLE = "RG"
            PDT.Rows.Add(PR)
            PDT.AcceptChanges()
        End If
        If Not String.IsNullOrEmpty(KeyInPerson) Then
            Dim PR6 As SAPDAL.SalesOrder.PartnerFuncRow = PDT.NewRow : PR6.ROLE = "ZR" : PR6.NUMBER = KeyInPerson : PDT.Rows.Add(PR6)
        End If



        '/Partner
        'Condition
        If Not IsNothing(myFt) Then
            For Each R As IBUS.iCondLine In myFt
                Dim conLine As SAPDAL.SalesOrder.ConditionRow = CODT.NewRow
                With R
                    conLine.TYPE = .Type : conLine.VALUE = .Value : conLine.CURRENCY = dtMaster.currency
                End With
                CODT.Rows.Add(conLine)
            Next
        End If


        '/Condition
        Dim RDT As New DataTable : RDT.TableName = "RDTABLE"
        Dim WS As New SAPDAL.SAPDAL

        Dim B As Boolean = False

        If IsCreateSAPQuote Then
            Dim Temp_QuoteID As String = Quote_NO
            'Frank 2013/07/22 If quote no already existed in SAP, CreateQuotation will return True
            B = WS.CreateQuotation(Quote_NO, ErrMsg, HDT, DDT, PDT, CODT, TDT, RDT)
            If B = False Then
                'Util.SendEmail("eBusiness.AEU@advantech.eu,ming.zhao@advantech.com.cn", "myadvanteh@advantech.com", "Create SAP Quote Failed:" + Temp_QuoteID, ErrMsg, True, "", "")
                Dim PSTSL As IBUS.iOrderProcSLine = Pivot.NewLineOrderProcS()
                PSTSL.ORDER_NO = Order_No
                PSTSL.MESSAGE = "Create Quotation Failed."
                PSTSL.TYPE = "AG"
                PSTS.Add(PSTSL)
                DocU.SendFailedOrderMail(dtMaster)
            End If
            Return B
        End If
        If dtMaster.DocRealType.ToString.ToUpper = "ZOR2" Or dtMaster.DocRealType.ToString.ToUpper = "ZOR" _
            Or dtMaster.DocRealType.ToString.ToUpper = "ZOR6" Then
            If isSimulate Then
                B = WS.SimulateSO("SIMSO", ErrMsg, HDT, DDT, PDT, CODT, TDT, CDT, RDT, LocalTime)
            Else
                B = WS.CreateSO(REFORDERNO, ErrMsg, HDT, DDT, PDT, CODT, TDT, CDT, RDT, LocalTime)
            End If
        ElseIf dtMaster.DocRealType.ToString.ToUpper = "AG" Then
            'B = WS.CreateQuotation(REFORDERNO, ErrMsg, HDT, DDT, PDT, CODT, TDT, RDT)
        Else
            ErrMsg = "DOC TYPE ERR!"
            Return False
        End If
        WS.Dispose()
        'OrderUtilities.showDT(RDT) : HttpContext.Current.Response.End()
        If B Then IB = 1
        If Not IsNothing(RDT) AndAlso RDT.Rows.Count > 0 Then
            Dim N As Integer = 0
            For Each R As DataRow In RDT.Rows
                Dim PSTSL As IBUS.iOrderProcSLine = Pivot.NewLineOrderProcS()
                PSTSL.ORDER_NO = Order_No
                PSTSL.MESSAGE = R.Item("MESSAGE")
                PSTSL.TYPE = dtMaster.DocRealType.ToString.ToUpper
                N += 1
                PSTSL.LINE_SEQ = N
                PSTS.Add(PSTSL)
            Next
        End If

        'Catch ex As Exception
        '    ErrMsg = ex.ToString()
        '    Return False
        'End Try
        If IB = 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    '<System.Web.Services.WebMethod()> _
    Public Function PlaceOrder(ByVal OrderNo As String) As String
        Dim ret As Boolean = False, ErrMsg As String = "", old_id As String = OrderNo, order_no As String = old_id
        Dim DocU As IBUS.iDoc = Pivot.NewObjDoc
        Dim HeaderLine As IBUS.iDocHeaderLine = MasterRef
        Dim C As IBUS.iCartList = DetailRef.GetListAll(COMM.Fixer.eDocType.ORDER)
        If Not IsNothing(HeaderLine) Then
            order_no = UID  'DocU.NewUID(HeaderLine.DocReg, COMM.Fixer.eDocType.ORDER)
            Dim QuoteNo As String = Pivot.NewObjDocHeader.GetByDocID(HeaderLine.OriginalQuoteID, COMM.Fixer.eDocType.EQ).quoteNo
            '20121012 Ming CreateSAPQuote
            Dim CQuoteret As Boolean = False
            Try
                If Role.IsUsaUser AndAlso QuoteNo <> "" Then
                    If (HeaderLine.DocReg And COMM.Fixer.eDocReg.ANA) = HeaderLine.DocReg OrElse (HeaderLine.DocReg And COMM.Fixer.eDocReg.AMX) = HeaderLine.DocReg Then
                        If DocU.CheckSAPQuote(QuoteNo) = False Then
                            CQuoteret = SOCreateV5(UID, ErrMsg, False, QuoteNo, True)
                        End If
                    End If
                End If
            Catch ex As Exception
                Util.SendEmail("myadvantech@advantech.com", "myadvanteh@advantech.com", "", "", "Create SAP Quote Failed.", "", ex.ToString, "")
            End Try
            Dim dtMsg As New DataTable
            If CQuoteret Then
                For i As Integer = 0 To 3
                    If DocU.CheckSAPQuote(QuoteNo) Then
                        Exit For
                    End If
                    If i = 3 Then
                        QuoteNo = ""
                        Util.SendEmail("myadvantech@advantech.com", "myadvanteh@advantech.com", "", "", "Find SAP Quote Failed.", "", "", "")

                        Exit For
                    End If
                    Threading.Thread.Sleep(1000)
                Next
            Else
                If DocU.CheckSAPQuote(QuoteNo) = False Then
                    QuoteNo = ""
                End If
            End If
            ret = SOCreateV5(order_no, ErrMsg, False, QuoteNo)
            If ret Then
                DocU.ProcessAfterOrderSuccess(MasterRef, C, COMM.Fixer.eDocType.ORDER)

                'Frank 2013/08/26
                '=====Write quote to order log=====
                Dim quote_to_order_logDT As New EQDS.QUOTE_TO_ORDER_LOGDataTable
                Dim quote_to_order_logDR As EQDS.QUOTE_TO_ORDER_LOGRow = quote_to_order_logDT.NewQUOTE_TO_ORDER_LOGRow()
                quote_to_order_logDR.PO_NO = HeaderLine.PO_NO
                quote_to_order_logDR.SO_NO = HeaderLine.quoteNo
                quote_to_order_logDR.QUOTEID = HeaderLine.OriginalQuoteID
                quote_to_order_logDR.ORDER_DATE = HeaderLine.CreatedDate
                quote_to_order_logDR.ORDER_BY = HeaderLine.CreatedBy
                quote_to_order_logDT.Rows.Add(quote_to_order_logDR)
                quote_to_order_logDT.AcceptChanges()
                Dim WS As New EDOC.quoteExit1
                WS.WriteQuoteToOrderLog(quote_to_order_logDT)
                'End=====Write quote to order log=====


                Dim retTable As New DataTable : Dim IsSAPProductionServer As Boolean = True
                If COMM.Util.IsTesting Then
                    IsSAPProductionServer = False
                End If

                If (HeaderLine.DocReg And COMM.Fixer.eDocReg.AUS) = HeaderLine.DocReg Then
                    Dim FirstRow As IBUS.iPartnerLine = Pivot.NewObjPartner.GetListAll(UID, COMM.Fixer.eDocType.ORDER).Where(Function(x) x.TYPE = "S").FirstOrDefault
                    If FirstRow IsNot Nothing AndAlso Not String.IsNullOrEmpty(FirstRow.ERPID) AndAlso Not String.IsNullOrEmpty(order_no) Then
                        With FirstRow
                            DocU.UpdateSAPSOShipToAttentionAddress(order_no, .ERPID, .NAME, .ATTENTION, .STREET, _
                                                                       .STREET2, .CITY, .STATE, .ZIPCODE, .COUNTRY, retTable, IsSAPProductionServer)
                        End With
                    End If
                    '20120816 Ming: Update SO Zero Price Items
                    Threading.Thread.Sleep(1000)
                    DocU.UpdateSOZeroPriceItems(order_no, C, retTable)
                    '20120816 TC: If Early ship is not allowed, update it on SAP SO

                    If HeaderLine.IS_EARLYSHIP = COMM.Fixer.eEarlyShipment.Early_Shipment_Not_Allowed Then
                        Dim dtReturn As DataTable = Nothing
                        Threading.Thread.Sleep(2000)
                        If Not DocU.UpdateSOSpecId(order_no, COMM.Fixer.eEarlyShipment.Early_Shipment_Not_Allowed, dtReturn) Then
                            '20120816 TC: should log this failure and inform IT
                        End If
                    End If
                End If
                'end

            Else

                DocU.ProcessAfterOrderFailed(MasterRef, C, COMM.Fixer.eDocType.ORDER)


                Util.showMessage(COMM.errMsg.getErrMsg(DocU.errCode))
                'OrderUtilities.showDT(dtMsg)
            End If

            'End If
        End If
        Return order_no
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(MasterRef) Then
            If Not IsPostBack Then
                Dim HeaderLine As IBUS.iDocHeaderLine = MasterRef
                'If Role.IsUsaUser Then
                '    CBPI2Customer.Checked = True : trTermConditionContent.Visible = False
                'Else
                '    CBPI2Customer.Checked = False
                'End If
                'If Role.IsInternalUser Then
                '    Me.trPI2In.Visible = True : TandC_Button.SelectedIndex = 0
                'End If
                btnOrder.Enabled = IIf(COMM.MasterFixer.ISRBU(HeaderLine.AccErpId) = True, False, True)


                If HeaderLine.qStatus <> CInt(COMM.Fixer.eDocStatus.OFINISH) AndAlso DetailRef.GetListAll(COMM.Fixer.eDocType.ORDER).Count > 0 Then
                    Me.btnOrder.Visible = True 'Me.TCtb.Visible = True
                    If HeaderLine.qStatus = CInt(COMM.Fixer.eDocStatus.OFAILED) Then
                        Me.btnOrder.Text = " Re Try "
                    End If
                End If
                If Role.IsUsaUser Then
                    SOCreateV5(UID, "", True)
                End If
                If HeaderLine.qStatus = CInt(COMM.Fixer.eDocStatus.OFAILED) Then
                    If Not Role.IsInternalUser Then
                        Me.lbThanks.Text = "Thanks for Order: " & HeaderLine.quoteNo & "."
                        Me.lbThanks.ForeColor = Drawing.Color.Green
                        Me.lbThanks.Font.Bold = True
                    Else

                        Me.lbThanks.Text = "MyAdvantech failed to sync this order to SAP due to following reason:"
                        Me.lbThanks.ForeColor = Drawing.Color.Red
                        Me.lbThanks.Font.Bold = True
                    End If
                ElseIf HeaderLine.qStatus = CInt(COMM.Fixer.eDocStatus.OFINISH) Then
                    Me.lbThanks.Text = "Thanks for Order: " & HeaderLine.quoteNo & "."
                    Me.lbThanks.ForeColor = Drawing.Color.Green
                    Me.lbThanks.Font.Bold = True
                    Me.btnOrder.Visible = False
                    'Me.TCtb.Visible = False
                End If

                If ISDIRSAP = "1" AndAlso Me.btnOrder.Visible = True AndAlso Me.btnOrder.Enabled = True Then
                    Me.btnOrder_Click(Me.btnOrder, Nothing)
                End If

                GETORDERINFO(UID)
            End If
        End If

    End Sub

    Protected Sub GETORDERINFO(ByVal ORDERNO As String)
        Dim customerBlock As String = "", orderBlock As String = "", detailBlock As String = ""
        Dim url As String = ""
        url = "PI.aspx?UID=" & ORDERNO
        Dim MyDOC As New System.Xml.XmlDocument
        Util.HtmlToXML(url, MyDOC)
        'Global_Inc.getXmlBlockByID("div", "divCustInfo", MyDOC, customerBlock)
        'Global_Inc.getXmlBlockByID("div", "divOrderInfo", MyDOC, orderBlock)
        Util.getXmlBlockByID("div", "divDetailInfo", MyDOC, detailBlock)
        Me.lb_Cust.Text = UIUtil.GetAscxStr(MasterRef, 0)
        Me.lb_Order.Text = UIUtil.GetAscxStr(MasterRef, 1)
        Me.lb_Detail.Text = detailBlock
    End Sub



    Protected Sub btnOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOrder.Click

        '    Glob.ShowInfo("Order cannot be placed via Sales Offices.") : Exit Sub
        'End If
        Dim ORDERNO As String = PlaceOrder(UID)

        Response.Redirect("~/order/OrderPreview.aspx?UID=" + UID)
    End Sub

End Class