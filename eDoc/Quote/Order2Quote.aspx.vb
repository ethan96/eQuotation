Public Class Order2Quote
    Inherits System.Web.UI.Page

    Public ReadOnly Property VBPA As DataTable
        Get
            Dim _VBPA As New DataTable
            If ViewState("VBPA") Is Nothing Then
                _VBPA = GetEQPARTNERBySoNo(TBsono.Text.Trim)
            Else
                _VBPA = CType(ViewState("VBPA"), DataTable)
            End If
            Return _VBPA
        End Get

    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("SoNo") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.QueryString("SoNo")) Then
                TBsono.Text = Request.QueryString("SoNo")
                BT1_Click(Me.BT1, Nothing)
            End If
        End If
    End Sub
    Shared Function ParseSAPDate(ByVal strSAPDate As String) As Date
        Dim strOutputDate As Date = Date.MinValue
        If Date.TryParseExact(strSAPDate, "yyyyMMdd", New Globalization.CultureInfo("en-US"), Globalization.DateTimeStyles.None, strOutputDate) Then
            Return strOutputDate
        End If
        Return Now
    End Function
    Protected Sub BT1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim NewQuoteID As String = Pivot.NewObjDoc.NewUID()
        Dim NewQuoteNo As String = Business.GetNoByPrefix(Pivot.CurrentProfile) 'ex:AUSQ003054 
        Dim sono As String = TBsono.Text.Trim
        Dim QMdt = GetOrderMasterFromSAP(sono)
        Dim vbkd As DataTable = Getvbkd(sono)
        Dim QM As IBUS.iDocHeaderLine = Pivot.NewLineHeader
        Dim QML As New List(Of IBUS.iDocHeaderLine)
        Dim ShipTo As String = String.Empty

        Dim _IsKRSales As Boolean = Role.IsKRAonlineSales
        Dim _IsJPSales As Boolean = Role.IsJPAonlineSales
        Dim _IsTWSales As Boolean = Role.IsTWAonlineSales


        Dim SyncPrice As Boolean = True
        If _IsJPSales Then
            SyncPrice = False
        End If



        '1. Quotation Master
        With QMdt.Rows(0)
            QM.Key = NewQuoteID
            QM.CustomId = ""
            QM.AccRowId = ""
            QM.AccErpId = .Item("COMPANYID")
            Dim dt As DataTable = tbOPBase.dbGetDataTable("CRM", SiebelTools.GET_Account_info_By_ERPID(.Item("COMPANYID")))
            If dt.Rows.Count > 0 Then
                QM.AccRowId = dt.Rows(0).Item("ROW_ID")
            End If
            If _IsJPSales Then
                QM.AccRowId = .Item("COMPANYID")
            End If
            QM.AccName = .Item("COMPANYNAME")
            QM.AccOfficeName = .Item("OFFICE")
            QM.currency = .Item("CURR")
            QM.salesEmail = ""
            QM.salesRowId = ""
            If Me.VBPA IsNot Nothing Then
                Dim drs() As DataRow = VBPA.Select("PARVW = 'VE'")
                If drs.Length > 0 Then
                    QM.salesRowId = drs(0).Item("PERNR")
                End If
                drs = VBPA.Select("PARVW = 'WE'")
                If drs.Length > 0 Then
                    ShipTo = drs(0).Item("kunnr")
                End If
            End If
            QM.directPhone = ""
            QM.attentionRowId = ""
            QM.attentionEmail = ""
            QM.bankInfo = ""
            QM.DocDate = Now 'ParseSAPDate(.Item("AUDAT"))
            QM.deliveryDate = Now.AddDays(3).Date 'ParseSAPDate(.Item("vdatu"))
            'QM.expiredDate = Now.AddDays(10).ToShortDateString
            QM.expiredDate = Today.AddDays(COMM.MasterFixer.getExpDaysByReg(Pivot.CurrentProfile.CurrDocReg)).ToShortDateString()
            QM.shipTerm = ""
            QM.paymentTerm = vbkd.Rows(0).Item("zterm")
            QM.freight = 0
            QM.insurance = 0
            QM.specialCharge = 0
            QM.tax = 0
            QM.quoteNote = ""
            QM.relatedInfo = ""
            QM.CreatedBy = Pivot.CurrentProfile.UserId
            QM.CreatedDate = Now
            QM.preparedBy = ""
            QM.qStatus = CInt(COMM.Fixer.eDocStatus.QDRAFT)
            QM.isShowListPrice = 0
            QM.isShowDiscount = 0
            QM.isShowDueDate = 0
            QM.isLumpSumOnly = 0
            QM.isRepeatedOrder = 0
            QM.AccGroupName = ""
            QM.delDateFlag = 0
            QM.org = .Item("VKORG")

            QM.siebelRBU = ""
            If _IsJPSales Then
                QM.siebelRBU = "AJP"
                QM.tax = Convert.ToDecimal(0.08)
            End If
            If _IsKRSales Then
                QM.siebelRBU = "AKR"
                QM.tax = Convert.ToDecimal(0.1)
            End If


            QM.DIST_CHAN = .Item("VTWEG")
            QM.DIVISION = .Item("SPART")
            QM.AccGroupCode = .Item("VKGRP")
            QM.AccOfficeCode = .Item("VKBUR")
            QM.SalesDistrict = vbkd.Rows(0).Item("bzirk")
            QM.PO_NO = "" ' vbkd.Rows(0).Item("bstkd")
            QM.CARE_ON = ""
            QM.isExempt = IIf(SAPDAL.SAPDAL.isTaxExempt(.Item("COMPANYID")), 1, 0)
            QM.Inco1 = vbkd.Rows(0).Item("inco1")
            QM.Inco2 = vbkd.Rows(0).Item("inco2")
            QM.quoteNo = NewQuoteNo
            QM.Revision_Number = 1
            QM.Active = True
        End With
        QML.Add(QM)
        gv1.DataSource = QML
        gv1.DataBind()
        Dim QDdt = GetOrderDetailFromSAPByPoNo(sono)
        Dim QD As New EQDS.QuotationDetailDataTable
        QD.Columns.Add("RecyclingFee", GetType(Decimal))

        'Ryan 20170616 Reprocessing LineNo
        If QDdt.AsEnumerable().Where(Function(p) p("HIGHERLEVEL").ToString().Equals("0") AndAlso Not p("PARTNO").ToString().ToUpper().EndsWith("BTO")).Any() Then
            Dim i As Integer = 1
            For Each d As DataRow In QDdt.Rows
                If d("HIGHERLEVEL").ToString().Equals("0") AndAlso Not d("PARTNO").ToString().ToUpper().EndsWith("BTO") Then
                    d("LINENO") = i
                    i += 1
                End If
            Next
            QDdt.AcceptChanges()
        End If

        Dim objCurrencyMarkUp As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", String.Format("SELECT CURRDEC FROM SAPRDP.TCURX WHERE CURRKEY = '{0}'", QM.currency))
        Dim CurrencyMarkUp As Decimal = 1
        If objCurrencyMarkUp IsNot Nothing AndAlso Int32.TryParse(objCurrencyMarkUp.ToString(), CurrencyMarkUp) Then
            CurrencyMarkUp = Convert.ToInt32(100 * Math.Pow(10, CurrencyMarkUp))
        End If



        '2. Quotation Detail
        For Each r As DataRow In QDdt.Rows
            Dim QDdr As EQDS.QuotationDetailRow = QD.NewQuotationDetailRow()
            With r
                QDdr.quoteId = NewQuoteID
                QDdr.line_No = .Item("LINENO")
                ' QDdr.partNo = .Item("PARTNO")
                QDdr.partNo = SAPDAL.Global_Inc.RemovePrecedingZeros(.Item("PARTNO").ToString.Trim)
                QDdr.VirtualPartNo = QDdr.partNo  '20160601 Alex add: virtualPartNo default equal partNo 
                QDdr.description = .Item("ARKTX")
                QDdr.qty = Integer.Parse(.Item("QTY"))
                If Integer.Parse(.Item("QTY")) = 0 Then
                    QDdr.qty = 1
                End If
                QDdr.listPrice = 0
                QDdr.unitPrice = .Item("UNITPRICE")
                QDdr.newUnitPrice = 0
                QDdr.itp = 0
                QDdr.newItp = 0
                QDdr.deliveryPlant = .Item("WERKS")
                QDdr.category = ""
                QDdr.classABC = ""
                QDdr.rohs = 0
                QDdr.ewFlag = 0
                QDdr.reqDate = Now ' ParseSAPDate(.Item("REQDATE"))
                QDdr.dueDate = Now ' ParseSAPDate(.Item("REQDATE"))
                QDdr.satisfyFlag = 0
                QDdr.canBeConfirmed = 0
                QDdr.custMaterial = ""
                QDdr.inventory = 0
                QDdr.itemType = 0
                QDdr.modelNo = ""
                QDdr.sprNo = ""
                QDdr.HigherLevel = .Item("HigherLevel")

                'Ryan 20170628 Add for AJP ITP
                If _IsJPSales Then
                    QDdr.itp = SAPDAL.SAPDAL.getItp(QM.org, QDdr.partNo, QM.currency, QM.AccErpId, SAPDAL.SAPDAL.itpType.JP)
                    QDdr.newItp = QDdr.itp

                    'Dim objCurrencyMarkUp As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", String.Format("SELECT CURRDEC FROM SAPRDP.TCURX WHERE CURRKEY = '{0}'", QM.currency))
                    'Dim CurrencyMarkUp As Decimal = 0
                    'If objCurrencyMarkUp IsNot Nothing AndAlso Int32.TryParse(objCurrencyMarkUp.ToString(), CurrencyMarkUp) Then
                    '    CurrencyMarkUp = Convert.ToInt32(100 * Math.Pow(10, CurrencyMarkUp))
                    '    QDdr.unitPrice = QDdr.unitPrice * CurrencyMarkUp
                    'End If
                    QDdr.unitPrice = QDdr.unitPrice * CurrencyMarkUp
                    QDdr.newUnitPrice = QDdr.unitPrice
                End If

                If _IsKRSales Then
                    QDdr.itp = SAPDAL.SAPDAL.getItp(QM.org, QDdr.partNo, QM.currency, QM.AccErpId, SAPDAL.SAPDAL.itpType.KR)
                    QDdr.newItp = QDdr.itp

                    'ICC 2016/1/14 When KRAonline add AGS-CTOS-SYS-A/B, set list price & unit price. Request by Jessica.Lee.
                    If QDdr.partNo.Equals("AGS-CTOS-SYS-A", StringComparison.OrdinalIgnoreCase) Then
                        QDdr.listPrice = 40000
                        QDdr.unitPrice = 40000
                    ElseIf QDdr.partNo.Equals("AGS-CTOS-SYS-B", StringComparison.OrdinalIgnoreCase) Then
                        QDdr.listPrice = 60000
                        QDdr.unitPrice = 60000
                    End If
                End If

            End With
            QD.Rows.Add(QDdr)
        Next

        '3. Sync Price
        Dim aCompList As New ArrayList
        For Each r As DataRow In QD.Rows
            If r.Item("itemType") <> COMM.Fixer.eItemType.Parent Then
                aCompList.Add(r.Item("partNo"))
            End If
        Next
        Dim strPNs As String = String.Join("|", aCompList.ToArray())
        Dim dtPrice As DataTable = Nothing
        'SAPTools.getSAPPriceByTable(strPNs, dth.Rows(0)("org"), dth.Rows(0)("quoteToErpId"), dtPrice, errormessage)
        Dim company_id As String = QM.AccErpId
        If String.IsNullOrEmpty(ShipTo) Then ShipTo = company_id
        Business.GetPriceBiz(company_id, ShipTo, QM.org, QM.currency, "", QM.AccOfficeName, strPNs, QM.CreatedBy, dtPrice, QM.DocReg)
        If dtPrice IsNot Nothing AndAlso dtPrice.Rows.Count > 0 Then
            Dim pricerow As DataRow()
            For Each r As DataRow In QD.Rows
                pricerow = dtPrice.Select(String.Format("MATNR='{0}'", r.Item("partNo")))
                If r.Item("itemType") <> COMM.Fixer.eItemType.Parent Then
                    If pricerow.Count > 0 AndAlso SyncPrice Then
                        r.Item("newunitPrice") = pricerow(0)("Netwr")
                        r.Item("listPrice") = pricerow(0)("Kzwi1")
                        r.Item("unitPrice") = pricerow(0)("Netwr")
                        'Ming 20150603 RecyclingFee
                        r.Item("RecyclingFee") = 0
                        If String.Equals(QM.org, "US01", StringComparison.InvariantCultureIgnoreCase) Then
                            Dim _RecycleFee As Decimal = 0
                            If Not IsDBNull(pricerow(0).Item("RECYCLE_FEE")) AndAlso Decimal.TryParse(pricerow(0).Item("RECYCLE_FEE").ToString(), _RecycleFee) Then
                                r.Item("RecyclingFee") = _RecycleFee
                                r.Item("newunitPrice") = pricerow(0)("Netwr") - _RecycleFee
                                r.Item("listPrice") = pricerow(0)("Kzwi1") - _RecycleFee
                                r.Item("unitPrice") = pricerow(0)("Netwr") - _RecycleFee
                            End If
                        End If
                    ElseIf pricerow.Count > 0 AndAlso Not SyncPrice Then
                        r.Item("listPrice") = pricerow(0)("Kzwi1")
                    End If

                    If _IsKRSales Then
                        If r.Item("partNo").ToString.Equals("AGS-CTOS-SYS-A", StringComparison.OrdinalIgnoreCase) Then
                            r.Item("newunitPrice") = 40000
                            r.Item("listPrice") = 40000
                            r.Item("unitPrice") = 40000
                        ElseIf r.Item("partNo").ToString.Equals("AGS-CTOS-SYS-B", StringComparison.OrdinalIgnoreCase) Then
                            r.Item("newunitPrice") = 40000
                            r.Item("listPrice") = 40000
                            r.Item("unitPrice") = 40000
                        End If
                    End If
                End If
            Next
        End If

        '3.5 Re-calculate EW price
        Try
            'Get All BTOS Parent Line No which contains EW items
            Dim TargetParentLineNo As List(Of Integer) = New List(Of Integer)
            Dim EWU As IBUS.iEWUtil = New DOCH.EWUtil
            For Each r As DataRow In QD.Select("partNo like 'AGS-EW*'")
                TargetParentLineNo.Add(r.Item("Higherlevel"))
            Next

            'Recursive process all target systems
            For Each i As Integer In TargetParentLineNo
                Dim TotalBTOSWarrantableAmount As Decimal = 0
                Dim EWPartNo As String = QD.Select("Higherlevel = '" & i & "' and partNo like 'AGS-EW*'").FirstOrDefault().Item("partNo")
                Dim EWMonth As Integer = IIf(String.IsNullOrEmpty(EWPartNo), 0, EWPartNo.Replace("AGS-EW-", ""))
                For Each _row As DataRow In QD.Select("Higherlevel = '" & i & "' and itemType<>'" & COMM.Fixer.eItemType.Parent & "'")
                    If SAPDAL.CommonLogic.isWarrantableV3(_row.Item("partNo"), QM.org) Then
                        _row.Item("ewFlag") = EWMonth
                        TotalBTOSWarrantableAmount = TotalBTOSWarrantableAmount + (_row.Item("newunitPrice") * _row.Item("qty"))
                    End If
                Next

                Dim EWRow As DataRow = QD.Select("partNo like 'AGS-EW*' and Higherlevel = '" & i & "'").FirstOrDefault()
                If EWRow IsNot Nothing AndAlso EWRow.Item("newunitPrice") IsNot Nothing Then
                    EWRow.Item("newunitPrice") = TotalBTOSWarrantableAmount / EWRow.Item("qty") * EWU.getRateByEWItem(EWU.getEWItemByMonth(EWMonth, Pivot.CurrentProfile.CurrDocReg), Pivot.CurrentProfile.CurrDocReg)
                    EWRow.Item("unitPrice") = EWRow.Item("newunitPrice")
                    EWRow.Item("listPrice") = EWRow.Item("newunitPrice")
                End If
            Next
        Catch ex As Exception

        Finally

        End Try

        '4. EQ Partner
        gv2.DataSource = QD
        gv2.DataBind()
        Dim QP As New EQDS.EQPARTNERDataTable
        For Each r As DataRow In Me.VBPA.Rows
            Dim QPdr As EQDS.EQPARTNERRow = QP.NewEQPARTNERRow()
            QPdr.QUOTEID = NewQuoteID
            If r.Item("PARVW") = "VE" Then
                QPdr.ERPID = r.Item("pernr")
                QPdr.TYPE = "E"
                QP.Rows.Add(QPdr)
            End If
            If r.Item("PARVW") = "AG" Then
                QPdr.ERPID = r.Item("kunnr")
                QPdr.TYPE = "SOLDTO"
                If Not String.IsNullOrEmpty(r.Item("ADRNR")) Then
                    Dim DT As DataTable = Getadrc(r.Item("ADRNR"))
                    If DT.Rows.Count > 0 Then
                        QPdr.NAME = DT.Rows(0).Item("name1")
                        QPdr.ADDRESS = ""
                        QPdr.ATTENTION = DT.Rows(0).Item("name_co")
                        QPdr.TEL = DT.Rows(0).Item("tel_number")
                        QPdr.MOBILE = "" 'DT.Rows(0).Item("ADRNR")
                        QPdr.ZIPCODE = DT.Rows(0).Item("post_code1")
                        QPdr.COUNTRY = DT.Rows(0).Item("country")
                        QPdr.CITY = DT.Rows(0).Item("city1")
                        QPdr.STREET = DT.Rows(0).Item("street")
                        QPdr.STATE = DT.Rows(0).Item("region")
                        QPdr.DISTRICT = DT.Rows(0).Item("city2")
                        QPdr.STREET2 = DT.Rows(0).Item("str_suppl3")
                    End If
                End If
                QP.Rows.Add(QPdr)
            End If
            If r.Item("PARVW") = "WE" Then
                QPdr.ERPID = r.Item("kunnr")
                QPdr.TYPE = "S"
                Dim DT As DataTable = Getadrc(r.Item("ADRNR"))
                If DT.Rows.Count > 0 Then
                    QPdr.NAME = DT.Rows(0).Item("name1")
                    QPdr.ADDRESS = ""
                    QPdr.ATTENTION = DT.Rows(0).Item("name_co")
                    QPdr.TEL = DT.Rows(0).Item("tel_number")
                    QPdr.MOBILE = "" 'DT.Rows(0).Item("ADRNR")
                    QPdr.ZIPCODE = DT.Rows(0).Item("post_code1")
                    QPdr.COUNTRY = DT.Rows(0).Item("country")
                    QPdr.CITY = DT.Rows(0).Item("city1")
                    QPdr.STREET = DT.Rows(0).Item("street")
                    QPdr.STATE = DT.Rows(0).Item("region")
                    QPdr.DISTRICT = DT.Rows(0).Item("city2")
                    QPdr.STREET2 = DT.Rows(0).Item("str_suppl3")
                End If
                QP.Rows.Add(QPdr)
            End If
            If r.Item("PARVW") = "RE" Then
                QPdr.ERPID = r.Item("kunnr")
                QPdr.TYPE = "B"
                Dim DT As DataTable = Getadrc(r.Item("ADRNR"))
                If DT.Rows.Count > 0 Then
                    QPdr.NAME = DT.Rows(0).Item("name1")
                    QPdr.ADDRESS = ""
                    QPdr.ATTENTION = DT.Rows(0).Item("name_co")
                    QPdr.TEL = DT.Rows(0).Item("tel_number")
                    QPdr.MOBILE = "" 'DT.Rows(0).Item("ADRNR")
                    QPdr.ZIPCODE = DT.Rows(0).Item("post_code1")
                    QPdr.COUNTRY = DT.Rows(0).Item("country")
                    QPdr.CITY = DT.Rows(0).Item("city1")
                    QPdr.STREET = DT.Rows(0).Item("street")
                    QPdr.STATE = DT.Rows(0).Item("region")
                    QPdr.DISTRICT = DT.Rows(0).Item("city2")
                    QPdr.STREET2 = DT.Rows(0).Item("str_suppl3")
                End If
                QP.Rows.Add(QPdr)
            End If
        Next
        gv3.DataSource = QP
        gv3.DataBind()


        tbOPBase.adddblog("delete from quotationMaster where quoteid='" & NewQuoteID & "'")
        Dim QDA As New EQDSTableAdapters.QuotationDetailTableAdapter
        QDA.DeleteQuoteDetailById(NewQuoteID)
        tbOPBase.adddblog("delete from quotationDetail where quoteid='" & NewQuoteID & "'")
        Dim QPA As New EQDSTableAdapters.EQPARTNERTableAdapter
        QPA.DeleteQuotePartnersByQuoteId(NewQuoteID)
        tbOPBase.adddblog("DELETE FROM EQPARTNER WHERE (QUOTEID = '" & NewQuoteID & "')")
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        conn.Open()
        Dim bk As New SqlClient.SqlBulkCopy(conn)
        bk.DestinationTableName = "EQPARTNER" : bk.WriteToServer(QP)
        Pivot.NewObjDocHeader.AddByAssignedUID(NewQuoteID, QM, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
        ' bk.DestinationTableName = "QuotationDetail" : bk.WriteToServer(QD)
        Dim strErr As String = ""
        Dim addItemType As EnumSetting.ItemType = COMM.Fixer.eItemType.Others, intParentLineNo As Integer = 0, strCategoryName As String = ""
        'For Each dr As EQDS.QuotationDetailRow In QD.Rows
        '    strCategoryName = Business.GetCategoryName(dr.partNo, QM.org)
        '    If Integer.Parse(dr.line_No) > 100 Then
        '        addItemType = COMM.Fixer.eItemType.Others : intParentLineNo = 100
        '    End If
        '    If Integer.Parse(dr.line_No) = 100 Then
        '        addItemType = COMM.Fixer.eItemType.Parent : intParentLineNo = 0
        '    End If
        '    'If Not Business.ADD2QUOTE_V2(NewQuoteId, IsNumericItem_Shrink(dr.partNo), dr.qty, 0, addItemType, strCategoryName, 1, 1, dr.reqDate, intParentLineNo, dr.line_No, strErr) Then
        '    If Not Business.ADD2QUOTE_V2_1(NewQuoteID, IsNumericItem_Shrink(dr.partNo), dr.qty, 0, addItemType, strCategoryName, 1, 1, dr.reqDate, intParentLineNo, dr.deliveryPlant, dr.line_No, strErr, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ) Then
        '        ' Response.Write(dr.partNo + strErr) : Exit Sub
        '    End If
        '    'Dim QItem As QuoteItem = MyQuoteX.GetQuoteItem(NewQuoteID, dr.line_No)
        '    'QItem.description = 
        'Next

        '5. Add Quotation Detail
        Dim _Quotelist As New List(Of QuoteItem)
        For Each dr As EQDS.QuotationDetailRow In QD.Rows
            Dim _Qitem As New QuoteItem
            _Qitem.quoteId = NewQuoteID : _Qitem.line_No = dr.line_No : _Qitem.partNo = dr.partNo

            '20160601 Alex add: Set virtualPartNo default equal partNo if quoteNo contain TWQ
            'If Left(NewQuoteNo, 3) = "TWQ" Then
            If _IsTWSales OrElse _IsKRSales Then
                _Qitem.VirtualPartNo = dr.VirtualPartNo
            End If

            _Qitem.description = dr.description : _Qitem.qty = dr.qty
            _Qitem.listPrice = dr.listPrice : _Qitem.unitPrice = dr.unitPrice
            _Qitem.newUnitPrice = dr.unitPrice : _Qitem.itp = dr.itp
            If _Qitem.unitPrice Is Nothing Then _Qitem.unitPrice = _Qitem.newUnitPrice
            _Qitem.newItp = dr.itp : _Qitem.deliveryPlant = dr.deliveryPlant
            _Qitem.category = Business.GetCategoryName(dr.partNo, QM.org)
            _Qitem.classABC = dr.classABC
            _Qitem.rohs = dr.rohs : _Qitem.ewFlag = dr.ewFlag
            _Qitem.reqDate = dr.reqDate : _Qitem.dueDate = dr.dueDate
            _Qitem.satisfyFlag = dr.satisfyFlag : _Qitem.canBeConfirmed = dr.canBeConfirmed
            _Qitem.custMaterial = dr.custMaterial : _Qitem.inventory = dr.inventory
            _Qitem.oType = 0 : _Qitem.modelNo = dr.modelNo
            _Qitem.ItemType = 0 : _Qitem.RecyclingFee = 0
            If Not IsDBNull(dr.Item("RecyclingFee")) AndAlso dr.Item("RecyclingFee") IsNot Nothing Then
                _Qitem.RecyclingFee = dr.Item("RecyclingFee")
            End If
            Dim HigherLevel As Integer = 0
            If Integer.TryParse(dr.HigherLevel, 0) Then
                HigherLevel = Integer.Parse(dr.HigherLevel)
            End If
            If HigherLevel = 0 AndAlso Integer.Parse(dr.line_No) Mod 100 = 0 AndAlso Integer.Parse(dr.line_No) >= 100 Then
                ' _Qitem.oType = 1
                _Qitem.ItemType = 1
            End If
            'If Integer.Parse(dr.line_No) = 100 Then
            '    _Qitem.oType = -1
            '    _Qitem.ItemType = 1
            'End If
            _Qitem.HigherLevel = HigherLevel 'dr.HigherLevel
            _Qitem.sprNo = ""
            _Quotelist.Add(_Qitem)
        Next
        MyUtil.Current.CurrentDataContext.QuoteItems.InsertAllOnSubmit(_Quotelist)

        'end
        Dim SalesNote As String = "", OrderNote As String = ""
        Try
            GetNotesFromSAP(sono, SalesNote, OrderNote)
        Catch ex As Exception
            Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "GetNotesFromSAP Error Message : " + sono, "", ex.ToString, "")
        End Try

        Dim _NoteType As String = "SalesNote"
        SaveNote(NewQuoteID, "SalesNote", SalesNote)
        SaveNote(NewQuoteID, "OrderNote", OrderNote)
        Dim QuoteList As List(Of QuoteItem) = MyQuoteX.GetQuoteList(NewQuoteID)

        For Each i As QuoteItem In QuoteList
            If i.HigherLevel = 0 AndAlso i.line_No Mod 100 = 0 Then
                i.ItemType = 1
            End If
            Dim drarr() As EQDS.QuotationDetailRow = QD.Select(String.Format("partNo = '{0}'", i.partNo))
            If drarr.Length > 0 AndAlso Not IsNothing(drarr(0).Item("description")) Then
                i.description = drarr(0).Item("description")
                i.category = Business.GetCategoryName(i.partNo, QM.org)
            End If
            'For Each r As EQDS.QuotationDetailRow In QD.Rows
            '    If String.Equals(i.partNo, r.partNo, StringComparison.CurrentCultureIgnoreCase) Then
            '        i.description = r.description
            '        Exit For
            '    End If
            'Next
        Next
        MyUtil.Current.CurrentDataContext.SubmitChanges()
        If conn.State <> ConnectionState.Closed Then conn.Close()
        Response.Redirect("~/quote/QuotationMaster.aspx?UID=" & NewQuoteID)
    End Sub

    Public Sub GetNotesFromSAP(ByVal orderno As String, ByRef SalesNote As String, ByRef OrderNote As String)
        Dim OracleDt As DataTable = OraDbUtil.dbGetDataTable(GetConnStr(), "select TDNAME,TDID,TDOBJECT from SAPRDP.STXL where  TDNAME='" + orderno + "' AND MANDT='168' AND RELID='TX' and TDID in ('0001','0002')  and (TDOBJECT ='VBBK' )  and CLUSTD is not null and TDNAME IS NOT NULL AND TDSPRAS ='E'")
        If OracleDt.Rows.Count > 0 Then
            Dim eup As New Z_READ_TEXT.Z_READ_TEXT
            Dim pout1 As New Z_READ_TEXT.THEAD
            Dim pout2 As New Z_READ_TEXT.TLINETable
            Dim connstr As String = "SAP_PRD"
            If COMM.Util.IsTesting() Then
                'connstr = "SAP_PRD"
                connstr = "SAPConnTest"
            End If
            eup.Connection = New SAP.Connector.SAPConnection(System.Configuration.ConfigurationManager.AppSettings(connstr))
            eup.Connection.Open()
            For i As Integer = 0 To OracleDt.Rows.Count - 1
                Dim outstr As String = ""
                eup.Zread_Text(0, "168", OracleDt.Rows(i).Item("TDID").ToString.Trim, "EN", "", OracleDt.Rows(i).Item("TDNAME").ToString.Trim.Replace("'", "''"), OracleDt.Rows(i).Item("TDOBJECT").ToString, pout1, pout2)
                Dim ExportDT As DataTable = pout2.ToADODataTable()
                If ExportDT.Rows.Count > 0 Then
                    For j As Integer = 0 To ExportDT.Rows.Count - 1
                        If Not IsDBNull(ExportDT.Rows(j).Item("Tdline")) Then
                            outstr += ExportDT.Rows(j).Item("Tdline").ToString.Trim.Replace("'", "''") + vbCrLf
                        End If
                    Next
                End If
                If String.Equals(OracleDt.Rows(i).Item("TDID").ToString.Trim, "0001") Then
                    SalesNote = outstr
                End If
                If String.Equals(OracleDt.Rows(i).Item("TDID").ToString.Trim, "0002") Then
                    OrderNote = outstr
                End If
            Next
            eup.Connection.Close()
        End If
    End Sub
    Private Sub SaveNote(ByVal _Quoteid As String, ByVal _NoteType As String, ByVal _notetext As String)
        Dim _QuotNoteDA As New EQDSTableAdapters.QuotationNoteTableAdapter
        _QuotNoteDA.DeleteQuotationNote(_Quoteid, _NoteType) : _QuotNoteDA.InsertQuotationNote(_Quoteid, _NoteType, _notetext)
    End Sub
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
    Public Function IsNumericItem_Shrink(ByVal PartNO As String) As String
        If IsNumericItem(PartNO) Then
            IsNumericItem_Shrink = Mid(PartNO, 9)
        Else
            IsNumericItem_Shrink = PartNO
        End If
    End Function
    Public Shared Function GetOrderMasterFromSAP(ByVal SoNo As String) As DataTable
        Dim STR As String = " select VBELN AS ORDNO,WAERK AS CURR,VKORG AS ORG, " &
                            " (SELECT DISTINCT BEZEI FROM SAPRDP.TVKBT WHERE VKBUR=A.VKBUR AND ROWNUM=1) AS OFFICE, " &
                            " KUNNR AS COMPANYID, " &
                            " (SELECT KUNNR FROM SAPRDP.VBPA WHERE SAPRDP.VBPA.VBELN=A.VBELN AND SAPRDP.VBPA.PARVW='WE' AND ROWNUM=1) AS SHIPTOID, " &
                            " (SELECT KUNNR FROM SAPRDP.VBPA WHERE SAPRDP.VBPA.VBELN=A.VBELN AND SAPRDP.VBPA.PARVW='RE' AND ROWNUM=1) AS BILLTOID, " &
                            " (SELECT NAME1 FROM SAPRDP.KNA1 WHERE KUNNR=A.KUNNR AND ROWNUM=1) AS COMPANYNAME " &
                            " , A.* from SAPRDP.VBAK A where A.VBELN ='" & SoNo.ToUpper.Trim & "'"

        Dim dt As New DataTable("SAPOrders")

        dt = OraDbUtil.dbGetDataTable(GetConnStr(), STR) 'GetConnStr()
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return Nothing
    End Function
    Public Shared Function GetOrderDetailFromSAPByPoNo(ByVal PoNo As String) As DataTable
        If String.IsNullOrEmpty(PoNo) Then Return New DataTable("SAPDT")
        PoNo = Replace(Trim(PoNo.ToUpper), "'", "")
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine("  select cast(VBAP.UEPOS as integer) AS HigherLevel, cast(VBAP.POSNR as integer) AS Lineno, VBAP.MATWA AS  Partno,  ")
            .AppendLine("  cast(VBAP.LSMENG as integer) AS  Qty, VBAP.ZZ_EDATU AS ReqDate, VBAP.NETPR AS UnitPrice,VBAP.NETWR AS  Amount ,VBAP.*")
            .AppendFormat(" from saprdp.VBAP where VBAP.VBELN ='{0}' ", PoNo)
            .AppendLine(" order by Lineno ")
        End With
        Dim dt As New DataTable("SAPOrders")

        dt = OraDbUtil.dbGetDataTable(GetConnStr(), sb.ToString())
        'Util.showDT(dt)
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return New DataTable("SAPDT")
    End Function
    Public Shared Function GetEQPARTNERBySoNo(ByVal PoNo As String) As DataTable
        If String.IsNullOrEmpty(PoNo) Then Return New DataTable("SAPDT")
        PoNo = Replace(Trim(PoNo.ToUpper), "'", "")
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine("          SELECT VBPA.* FROM SAPRDP.VBPA   ")
            .AppendFormat(" where vbeln ='{0}'  ", PoNo)
        End With
        Dim dt As New DataTable("SAPOrders")

        dt = OraDbUtil.dbGetDataTable(GetConnStr(), sb.ToString())
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return New DataTable("SAPDT")
    End Function
    Public Shared Function Getvbkd(ByVal addrnumber As String) As DataTable
        If String.IsNullOrEmpty(addrnumber) Then Return New DataTable("SAPDT")
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendFormat("    select * from  saprdp.vbkd where vbeln ='{0}'   ", addrnumber)
        End With
        Dim dt As New DataTable("SAPOrders")

        dt = OraDbUtil.dbGetDataTable(GetConnStr(), sb.ToString())
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return New DataTable("SAPDT")
    End Function
    Public Shared Function Getadrc(ByVal addrnumber As String) As DataTable
        If String.IsNullOrEmpty(addrnumber) Then Return New DataTable("SAPDT")
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine("   SELECT * FROM  saprdp.adrc   ")
            .AppendFormat(" WHERE addrnumber ='{0}'   ", addrnumber)
        End With
        Dim dt As New DataTable("SAPOrders")
        dt = OraDbUtil.dbGetDataTable(GetConnStr(), sb.ToString())
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return New DataTable("SAPDT")
    End Function
    Public Shared Function GetConnStr() As String
        Dim connstr As String = "SAP_PRD"
        If COMM.Util.IsTesting() Then
            'connstr = "SAP_PRD"
            connstr = "SAP_Test"
        End If
        Return connstr
    End Function
End Class