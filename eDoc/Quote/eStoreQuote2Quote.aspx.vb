Public Class eStoreQuote2Quote
    Inherits System.Web.UI.Page

    Private _isNumericItem As Boolean

    Private Property IsNumericItem(ByVal PartNO As String) As Boolean
        Get
            Return _isNumericItem
        End Get
        Set(ByVal value As Boolean)
            _isNumericItem = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("quoteid") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.QueryString("quoteid")) Then
                TBquoteid.Text = Request.QueryString("quoteid")
                BtCopy_Click(Me.BtCopy, Nothing)
            End If
        End If
    End Sub

    Protected Sub BtCopy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtCopy.Click
        Dim NewQuoteID As String = Pivot.NewObjDoc.NewUID()
        Dim NewQuoteNo As String = Business.GetNoByPrefix(Pivot.CurrentProfile) 'ex:AUSQ003054 
        Dim quoteno As String = TBquoteid.Text.Trim
        Dim QMdt = GetMasterFromeStore(quoteno)

        Dim QM As IBUS.iDocHeaderLine = Pivot.NewLineHeader
        Dim QML As New List(Of IBUS.iDocHeaderLine)

        With QMdt.Rows(0)
            QM.Key = NewQuoteID
            QM.CustomId = .Item("customId")
            QM.AccRowId = .Item("quoteToRowId")
            QM.AccErpId = .Item("quoteToErpId")
            QM.AccName = .Item("quoteToName")
            QM.AccOfficeName = .Item("office")
            QM.currency = .Item("currency")
            QM.salesEmail = .Item("salesEmail")
            QM.salesRowId = .Item("salesRowId")
            QM.directPhone = .Item("directPhone")
            QM.attentionRowId = .Item("attentionRowId")
            QM.attentionEmail = .Item("attentionEmail")
            QM.bankInfo = .Item("bankInfo")
            QM.DocDate = Now 'ParseSAPDate(.Item("AUDAT"))
            QM.deliveryDate = Now.AddDays(3).Date 'ParseSAPDate(.Item("vdatu"))
            'QM.expiredDate = Now.AddDays(10).ToShortDateString
            QM.expiredDate = Today.AddDays(COMM.MasterFixer.getExpDaysByReg(Pivot.CurrentProfile.CurrDocReg)).ToShortDateString()
            QM.shipTerm = .Item("shipTerm")
            QM.paymentTerm = .Item("paymentTerm")
            QM.freight = .Item("Freight")
            QM.insurance = 0
            If .Item("insurance") IsNot Nothing AndAlso Decimal.TryParse(.Item("insurance"), 0) Then
                QM.insurance = Decimal.Parse(.Item("insurance"))
            End If
            QM.specialCharge = 0
            If .Item("specialCharge") IsNot Nothing AndAlso Decimal.TryParse(.Item("specialCharge"), 0) Then
                QM.specialCharge = Decimal.Parse(.Item("specialCharge"))
            End If
            QM.tax = 0
            If .Item("Tax") IsNot Nothing AndAlso Not IsDBNull(.Item("Tax")) AndAlso Decimal.TryParse(.Item("Tax"), 0) Then
                QM.tax = Decimal.Parse(.Item("Tax"))
            End If
            QM.quoteNote = "" ' .Item("quoteNote")
            QM.relatedInfo = .Item("relatedInfo")
            QM.CreatedBy = Pivot.CurrentProfile.UserId
            QM.CreatedDate = Now
            QM.preparedBy = .Item("preparedBy")
            QM.qStatus = CInt(COMM.Fixer.eDocStatus.QDRAFT)
            QM.isShowListPrice = 0
            QM.isShowDiscount = 0
            QM.isShowDueDate = 0
            QM.isLumpSumOnly = 0
            QM.isRepeatedOrder = 0
            QM.AccGroupName = .Item("ogroup")
            QM.delDateFlag = 0
            QM.org = .Item("Org")
            QM.siebelRBU = .Item("siebelRBU")
            If IsDBNull(.Item("DIST_CHAN")) OrElse String.IsNullOrEmpty(.Item("DIST_CHAN")) Then
                QM.DIST_CHAN = 30
            Else
                QM.DIST_CHAN = .Item("DIST_CHAN")
            End If
            QM.DIVISION = .Item("DIVISION")
            QM.AccGroupCode = .Item("SALESGROUP")
            QM.AccOfficeCode = .Item("SALESOFFICE")
            QM.SalesDistrict = .Item("DISTRICT")
            QM.PO_NO = .Item("PO_NO") ' vbkd.Rows(0).Item("bstkd")
            QM.CARE_ON = .Item("CARE_ON")
            QM.OriginalQuoteID = .Item("quoteid")
            QM.isExempt = 0 ' IIf(Relics.SAPDAL.isTaxExempt(.Item("COMPANYID")), 1, 0)
            If .Item("isExempt") IsNot Nothing AndAlso Integer.TryParse(.Item("isExempt"), 0) Then
                QM.isExempt = Integer.Parse(.Item("isExempt"))
            End If

            QM.Inco1 = .Item("INCO1")
            QM.Inco2 = .Item("INCO2")
            'QM.quoteNo = quoteno
            QM.Revision_Number = 1
            QM.Active = True
            QM.DocReg = 96
            If .Item("org").ToString.StartsWith("EU", StringComparison.CurrentCultureIgnoreCase) Then
                QM.DocReg = 31
            End If
            QM.DOCSTATUS = 0
        End With
        QML.Add(QM)
        gv1.DataSource = QML
        gv1.DataBind()
        Dim QDdt = GetDetailFromeStore(quoteno)
        '  Dim QD As New List(Of QuoteItem)
        Dim i As Integer = 1, ParentLineNo As Integer = 0, IsParentItem As Boolean = False
        For Each r As DataRow In QDdt.Rows
            Dim QDdr As New QuoteItem
            IsParentItem = False
            With r
                QDdr.quoteId = NewQuoteID
                QDdr.line_No = .Item("line_No")
                QDdr.HigherLevel = 0 'Integer.Parse(.Item("HigherLevel"))
                QDdr.partNo = .Item("PARTNO")
                If IsNumeric(.Item("otype")) AndAlso Integer.TryParse(.Item("otype"), 0) Then
                    Select Case Integer.Parse(.Item("otype"))
                        Case 0 : QDdr.line_No = i : QDdr.HigherLevel = 0
                        Case -1
                            QDdr.line_No = MyQuoteX.getBtosParentLineNo(NewQuoteID)
                            ParentLineNo = QDdr.line_No
                            QDdr.HigherLevel = 0
                            IsParentItem = True
                            If Not IsDBNull(.Item("BTONO")) AndAlso Not String.IsNullOrEmpty(.Item("BTONO")) Then
                                QDdr.partNo = .Item("BTONO")
                            End If
                        Case 1 : QDdr.line_No = ParentLineNo + i : QDdr.HigherLevel = ParentLineNo
                    End Select
                End If
                QDdr.description = .Item("description")
                QDdr.qty = 1
                If IsNumeric(.Item("QTY")) Then
                    QDdr.qty = Integer.Parse(.Item("QTY"))
                End If
                QDdr.listPrice = 0
                If Decimal.TryParse(.Item("listPrice"), 0) AndAlso Not IsParentItem Then
                    QDdr.listPrice = Decimal.Parse(.Item("listPrice"))
                End If
                QDdr.unitPrice = 0
                If Decimal.TryParse(.Item("unitPrice"), 0) AndAlso Not IsParentItem Then
                    QDdr.unitPrice = Decimal.Parse(.Item("unitPrice"))
                End If
                'QDdr.newUnitPrice = .Item("newUnitPrice")
                QDdr.newUnitPrice = 0
                If Decimal.TryParse(.Item("newUnitPrice"), 0) AndAlso Not IsParentItem Then
                    QDdr.newUnitPrice = Decimal.Parse(.Item("newUnitPrice"))
                End If
                QDdr.itp = 0
                QDdr.newItp = 0
                QDdr.deliveryPlant = .Item("deliveryPlant")
                QDdr.category = ""
                QDdr.classABC = ""
                QDdr.rohs = 0
                QDdr.ewFlag = 0
                QDdr.reqDate = Now
                QDdr.dueDate = Now
                QDdr.satisfyFlag = 0
                QDdr.canBeConfirmed = 0
                QDdr.custMaterial = ""
                QDdr.inventory = 0
                QDdr.ItemType = 0
                If IsNumeric(.Item("otype")) AndAlso Integer.Parse(.Item("otype")) = -1 Then
                    QDdr.ItemType = 1
                End If
                QDdr.modelNo = .Item("modelNo")
                QDdr.sprNo = ""
                ' QDdr.HigherLevel = Integer.Parse(.Item("HigherLevel"))
            End With
            'QD.Add(QDdr)
            MyUtil.Current.CurrentDataContext.QuoteItems.InsertOnSubmit(QDdr)
            MyUtil.Current.CurrentDataContext.SubmitChanges()
            i = i + 1
        Next


        'gv2.DataSource = QD
        'gv2.DataBind()
        Dim QP As New EQDS.EQPARTNERDataTable
        If QM.AccErpId IsNot Nothing AndAlso Not String.IsNullOrEmpty(QM.AccErpId) AndAlso Business.is_Valid_Company_Id(QM.AccErpId) Then
            Dim dt As DataTable = tbOPBase.dbGetDataTable("CRM", SiebelTools.GET_Account_info_By_ERPID(QM.AccErpId))
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                QM.AccRowId = dt.Rows(0).Item("ROW_ID")
            End If
            Dim SapCustAddrDt As Relics.SalesOrder.PartnerAddressesDataTable = Relics.SAPDAL.GetSAPPartnerAddressesTableByKunnr(QM.AccErpId, True)
            If SapCustAddrDt.Count > 0 Then
                Dim QPdr As EQDS.EQPARTNERRow = QP.NewEQPARTNERRow()
                QPdr.QUOTEID = NewQuoteID
                QPdr.ERPID = QM.AccErpId
                Dim SapCustAddrRow As Relics.SalesOrder.PartnerAddressesRow = SapCustAddrDt(0)
                QPdr.NAME = SapCustAddrRow.Name
                QPdr.ADDRESS = ""
                QPdr.ATTENTION = SapCustAddrRow.C_O_Name
                QPdr.TEL = SapCustAddrRow.Tel1_Numbr
                QPdr.MOBILE = "" 'DT.Rows(0).Item("ADRNR")
                QPdr.ZIPCODE = SapCustAddrRow.Postl_Cod1
                QPdr.COUNTRY = SapCustAddrRow.Country
                QPdr.CITY = SapCustAddrRow.City
                QPdr.STREET = SapCustAddrRow.Street
                QPdr.STATE = SapCustAddrRow.Region_str
                QPdr.DISTRICT = ""
                QPdr.STREET2 = ""
                For j As Integer = 1 To 3
                    Select Case j
                        Case 1
                            QPdr.TYPE = "SOLDTO"
                            QP.Rows.Add(QPdr)
                        Case 2
                            Dim QPdr2 As EQDS.EQPARTNERRow = QP.NewEQPARTNERRow()
                            QPdr2.ItemArray = QPdr.ItemArray
                            QPdr2.TYPE = "S"
                            QP.Rows.Add(QPdr2)
                        Case 3
                            Dim QPdr3 As EQDS.EQPARTNERRow = QP.NewEQPARTNERRow()
                            QPdr3.ItemArray = QPdr.ItemArray
                            QPdr3.TYPE = "B"
                            QP.Rows.Add(QPdr3)
                    End Select
                Next
            End If
        End If
            gv3.DataSource = QP
            gv3.DataBind()
            tbOPBase.adddblog("delete from quotationMaster where quoteid='" & NewQuoteID & "'")
            Dim QDA As New EQDSTableAdapters.QuotationDetailTableAdapter
        'QDA.DeleteQuoteDetailById(NewQuoteID)
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
        '  Dim strErr As String = ""
        ' Dim addItemType As EnumSetting.ItemType = COMM.Fixer.eItemType.Others, intParentLineNo As Integer = 0, strCategoryName As String = ""
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
        '        Response.Write(dr.partNo + strErr) : Exit Sub
        '    End If
        'Next
        If conn.State <> ConnectionState.Closed Then conn.Close()
        Business.RefreshQuotationInventory(NewQuoteID)
        Response.Redirect("~/quote/QuotationMaster.aspx?UID=" & NewQuoteID)
    End Sub

    Public Function GetMasterFromeStore(ByVal quoteid As String) As DataTable
        Dim OrgID As String = Pivot.CurrentProfile.getCurrOrg
        Dim sb As New StringBuilder
        sb.AppendLine(" SELECT TOP 1 * from V_eStoreAllQuotation where 1=1 ")
        If Not String.IsNullOrEmpty(quoteid) Then
            sb.AppendFormat(" and quoteid = '{0}' ", quoteid)
        End If
        If Not String.IsNullOrEmpty(OrgID) Then
            sb.AppendFormat(" and ORG ='{0}' ", OrgID)
        End If
        Dim dt As New DataTable("SAPOrders")
        dt = tbOPBase.dbGetDataTableSchema("Estore", sb.ToString)
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return Nothing
    End Function
    Public Shared Function GetDetailFromeStore(ByVal quoteid As String) As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTableSchema("Estore", String.Format("SELECT top 50 * FROM V_eStoreQuotationDetail where quoteId = '{0}'", quoteid))
        If dt.Rows.Count > 0 Then
            Return dt
        End If
        Return New DataTable("Detail")
    End Function
    Public Function IsNumericItem_Shrink(ByVal PartNO As String) As String
        If IsNumeric(PartNO) Then
            IsNumericItem_Shrink = Mid(PartNO, 9)
        Else
            IsNumericItem_Shrink = PartNO
        End If
    End Function
End Class