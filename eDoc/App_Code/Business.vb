Imports Microsoft.VisualBasic
Imports HtmlAgilityPack
Imports EDOC
Imports IBUS
Imports Advantech.Myadvantech.DataAccess
Imports Advantech.Myadvantech.Business

Public Class Business

    Public Shared Function GetModelByPartNo(ByVal PN As String) As DataTable
        Dim _SQL As String = String.Empty, PISConn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("PIS").ConnectionString)
        '_SQL &= "Select a.model_name,a.part_no,b.Site_ID,b.Active_FLG"
        '_SQL &= " From model_product a left join Model_Publish b"
        '_SQL &= " on a.model_name=b.Model_name"
        '_SQL &= " where a.part_no=@PN "
        '_SQL &= " And a.relation='product'"
        '_SQL &= " And b.active_flg='Y'"
        '_SQL &= " And b.Site_ID='ACL'"
        '_SQL &= " Group by a.model_name,a.part_no,b.Site_ID,b.Active_FLG"

        If PISConn.State <> ConnectionState.Open Then PISConn.Open()

        'spGetModelByPN_estore_NEW '1-2MLAX2','1-2JKBQD',@pn
        Dim pisCMD As SqlClient.SqlCommand = New SqlClient.SqlCommand("spGetModelByPN_estore_NEW", PISConn)
        pisCMD.CommandType = CommandType.StoredProcedure
        pisCMD.Parameters.AddWithValue("@ID1", "1-2MLAX2")
        pisCMD.Parameters.AddWithValue("@ID2", "1-2JKBQD")
        pisCMD.Parameters.AddWithValue("@PN", PN)

        Dim myReader As SqlClient.SqlDataReader = pisCMD.ExecuteReader()

        Dim _dt As New DataTable
        _dt.Load(myReader)
        PISConn.Close()
        Return _dt
    End Function

    Public Shared Function getExipDay(ByVal org As String) As Integer
        If org.StartsWith("US", StringComparison.OrdinalIgnoreCase) Then
            Return 10
        End If
        Return 45
    End Function
    Public Shared Function GetModelByPartNoFromLOGISTICS(ByVal PN As String) As DataTable
        Dim _SQL As String = String.Empty
        _SQL &= "Select a.model_no as model_name,a.part_no,b.Site_ID,b.Active_FLG"
        _SQL &= " From PRODUCT_LOGISTICS_NEW a left join Model_Publish b"
        _SQL &= " on a.model_no=b.Model_name"
        _SQL &= " where a.part_no=@PN "
        _SQL &= " And b.active_flg='Y'"
        _SQL &= " And b.Site_ID='ACL'"
        _SQL &= " Group by a.model_no,a.part_no,b.Site_ID,b.Active_FLG"
        Dim apt As New SqlClient.SqlDataAdapter(_SQL, ConfigurationManager.ConnectionStrings("PIS").ConnectionString)
        apt.SelectCommand.Parameters.AddWithValue("PN", PN)
        Dim _dt As New DataTable
        apt.Fill(_dt)
        apt.SelectCommand.Connection.Close()
        Return _dt
    End Function


    Public Shared Function ForwardeQuotation(ByVal _QuoteID As String, ByVal _ContentType As EnumSetting.QuotationForwardType _
                                             , ByVal _RecipientEmail As String _
                                             , ByVal _SenderEmail As String _
                                             , ByVal _CCEmail As String _
                                             , ByVal _BCCEmail As String _
                                             , ByVal _EmailSubject As String _
                                             , ByVal _EmailGreeting As String _
                                             , ByVal _AttachedFiles As List(Of HttpPostedFile) _
                                             , ByVal ShowPageNumer As Boolean, ByRef _message As String) As Boolean

        Dim sendTo As String = Util.ReplaceSQLChar(_RecipientEmail)

        If String.IsNullOrEmpty(sendTo) Then
            _message = "Recipient email cannot be empty" : Return False
        End If
        Dim emails As String() = sendTo.Split(";")
        If emails.Length < 1 Then
            _message = "Email cannot be empty." : Return False
        Else
            For Each x As String In emails
                If Not Util.isEmail(x) Then
                    _message = x + " is not a valid email" : Return False
                End If
            Next
        End If

        Dim strFromMail As String = _SenderEmail, cc As String = _CCEmail, bcc As String = _BCCEmail 'bcc As String = "myadvantech@advantech.com"

        If Role.IsJPAonlineSales() Then cc = "AJP_IS@advantech.com"

        Dim subject As String = _EmailSubject, mailbody As String = "", AttachmentName As String = "", AccountRowId As String = "", ContactRowId As String = "", PrimarySales As String = "", RBU As String = ""

        Dim EQMR As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteID, COMM.Fixer.eDocType.EQ)

        If EQMR IsNot Nothing AndAlso Not IsDBNull(EQMR.CustomId) Then
            AttachmentName = String.Format("{0}.pdf", EQMR.quoteNo.ToString.Trim)
        End If

        Dim _isword As Boolean = False
        If Role.IsTWAonlineSales AndAlso
                   (EQMR.PRINTOUT_FORMAT = COMM.Fixer.USPrintOutFormat.ATW_Quote_Word_Englisn _
                    OrElse EQMR.PRINTOUT_FORMAT = COMM.Fixer.USPrintOutFormat.ATW_Quote_Word_TraditionalChinese) Then
            AttachmentName = String.Format("{0}.doc", EQMR.quoteNo.ToString.Trim)
            _isword = True
        End If

        If _ContentType = EnumSetting.QuotationForwardType.HTML Then
            'Frank 2012/08/29:Insert email greeting before quotation in the email content
            If Not String.IsNullOrEmpty(_EmailGreeting) Then
                mailbody &= _EmailGreeting & "<br>"
            End If
            'Frank 2012/07/26: If the quotation need to be sent to customer, quote display format should be external mode.
            If EQMR.DocReg = COMM.Fixer.eDocReg.AEU Then
                mailbody &= Util.GetAllStringForAEU(_QuoteID)
            Else
                'Ryan 20160412 If is AAC user then hide vertical text "Send all POs...".
                If Role.IsUSAACSales() AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2100") Then
                    mailbody &= Business.getPageStrInternal(_QuoteID, EQMR.DocReg, False).Replace("class=""verticaltext""", "class=""verticaltext"" style=""display:none;""")
                Else
                    mailbody &= Business.getPageStrInternal(_QuoteID, EQMR.DocReg, False)
                End If
            End If


            Dim _attcount As Integer = 0
            If _AttachedFiles IsNot Nothing AndAlso _AttachedFiles.Count > 0 Then
                _attcount = _AttachedFiles.Count - 1
            End If
            Dim _AttachedFilesStream(_attcount) As System.IO.Stream
            Dim _AttachedFilesName(_attcount) As String

            If _AttachedFiles.Count > 0 Then
                Dim j = 0
                For Each _attfile As HttpPostedFile In _AttachedFiles
                    _AttachedFilesStream(j) = _attfile.InputStream
                    _AttachedFilesName(j) = IO.Path.GetFileName(_attfile.FileName)
                    j += 1
                Next
            Else
                _AttachedFilesStream = Nothing
                _AttachedFilesName = Nothing
            End If



            COMM.Util.SendEmailV2(strFromMail, sendTo, cc, bcc, subject, _AttachedFilesStream, _AttachedFilesName, mailbody, "")

            'Util.SendEmail(strFromMail, sendTo, cc, bcc, subject, "", mailbody, "")
            'Util.SendEmail(strFromMail, sendTo, "", "", subject, "", mailbody, "") 'For testing
        Else
            Dim oStream As System.IO.Stream = Nothing
            'Frank 2012/08/29:Place email greeting to email body
            mailbody = _EmailGreeting
            'Dim PDFBYTE As Byte() = Util.DownloadQuotePDFbyStr(Business.getPageStrInternal(_QuoteID, False))

            'Frank 2013/08/01
            Dim PDFBYTE As Byte() = Nothing
            If EQMR.DocReg = COMM.Fixer.eDocReg.AEU Then
                PDFBYTE = Util.DownloadQuotePDFforAEU(_QuoteID)
            Else

                If _isword Then
                    PDFBYTE = Util.DownloadQuoteWordByHtmlString(Business.getPageStrInternal(_QuoteID, EQMR.DocReg, False), ShowPageNumer)
                Else
                    PDFBYTE = Util.DownloadQuotePDFByHtmlString(Business.getPageStrInternal(_QuoteID, EQMR.DocReg, False), ShowPageNumer)
                End If

            End If


            oStream = New System.IO.MemoryStream(PDFBYTE)


            Dim _attcount As Integer = 0
            If Not _AttachedFiles Is Nothing AndAlso _AttachedFiles.Count > 0 Then
                _attcount = _AttachedFiles.Count
            End If
            Dim _AttachedFilesStream(_attcount) As System.IO.Stream
            Dim _AttachedFilesName(_attcount) As String

            'index 0 for storing quotaton pdf file
            _AttachedFilesStream(0) = oStream
            _AttachedFilesName(0) = AttachmentName

            If _attcount > 0 Then
                Dim j = 1
                For Each _attfile As HttpPostedFile In _AttachedFiles
                    _AttachedFilesStream(j) = _attfile.InputStream
                    _AttachedFilesName(j) = IO.Path.GetFileName(_attfile.FileName)
                    j += 1
                Next
            End If

            COMM.Util.SendEmailV2(strFromMail, sendTo, cc, bcc, subject, _AttachedFilesStream, _AttachedFilesName, mailbody, "")

            'Util.SendEmail(strFromMail, sendTo, cc, bcc, subject, "", mailbody, "", oStream, AttachmentName)
            'Util.SendEmail(strFromMail, sendTo, "", "", subject, "", mailbody, "", oStream, AttachmentName)  'For testing
        End If
        ' Ming 2014/09/17 Log Mail
        Dim sb As New StringBuilder
        sb.Append(" INSERT INTO [MailLog] ([QuoteID] ,[MailFrom],[MailTo] ,[MailCC],[MailBCC] ,[Subject] ,[MailBody] ,[CreatedBy]  ,[CreatedDate]) ")
        sb.AppendFormat(" VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',getdate()) ", _QuoteID, strFromMail, sendTo, cc, bcc, subject, mailbody.Replace("'", "''"), Util.GetCurrentUserID())
        tbOPBase.dbExecuteNoQuery("EQ", sb.ToString())
        'Rudy 2012/09/21: Log Siebel Activity
        Dim _SiebelActid As String = String.Empty
        Try
            If EQMR IsNot Nothing Then
                AccountRowId = EQMR.AccRowId : PrimarySales = EQMR.salesEmail : RBU = EQMR.siebelRBU
                Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
                For Each x As String In emails
                    If Util.isEmail(x) AndAlso Not x Like "*@advantech*" Then
                        Dim dbCmd As New SqlClient.SqlCommand("select top 1 row_id from siebel_contact where email_address='" + x + "'", conn)
                        If conn.State <> ConnectionState.Open Then conn.Open()
                        Dim retObj As Object = dbCmd.ExecuteScalar()
                        If retObj IsNot Nothing Then ContactRowId = retObj.ToString : Exit For
                    End If
                Next
                conn.Close()
            End If
            '20120921 TC: Get pure text from html email greeting content, since we cannot insert html code to Siebel activity, it is not readable
            Dim strGreetingText As String = ""
            Dim doc1 As New HtmlAgilityPack.HtmlDocument
            doc1.LoadHtml(_EmailGreeting)
            strGreetingText = doc1.DocumentNode.InnerText
            'Ming add 20150402 call IC's API
            Dim _SiebelAcvitity As SiebelActive = New SiebelActive(_QuoteID, "", "", HttpContext.Current.User.Identity.Name, "")
            _SiebelAcvitity.Subject = subject
            _SiebelAcvitity.Greeting = IIf(strGreetingText.Length >= 1000, Left(strGreetingText, 1000), strGreetingText)
            _SiebelAcvitity.SendToMails = sendTo
            _SiebelAcvitity.ContactRowId = ContactRowId
            Dim retopty As Boolean = SiebelBusinessLogic.CreateQuoteActive(_SiebelAcvitity)
            If Not retopty Then Util.InsertMyErrLog(_SiebelAcvitity.FailedLog, Util.GetCurrentUserID())
            'Dim ws As New SiebelWS.Siebel_WS
            'ws.Timeout = -1 : ws.UseDefaultCredentials = True
            'Try
            '    _SiebelActid = ws.CreateSiebelActivity("Email - Outbound", "Done", "Activities Only", "(" + _QuoteID + ")" + subject, IIf(strGreetingText.Length >= 1000, Left(strGreetingText, 1000), strGreetingText), AccountRowId, ContactRowId, "", RBU, PrimarySales)
            'Catch ex As Exception
            '    Dim errormsg As String = ex.ToString + vbCrLf + "Business..vb:line:155"
            '    Util.InsertMyErrLog(errormsg, Util.GetCurrentUserID())
            'End Try
        Catch ex As Exception
            Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "Create Quotation Activity Failed: " + _QuoteID, "", ex.ToString, "")
        End Try
        _message = "Mail has been forwarded" : Return True
    End Function

    'Public Shared Sub RefreshBTOSMainitemAvailableDate(ByVal _QuoteId As String)
    '    Dim myQD As New EQDSTableAdapters.QuotationDetailTableAdapter
    '    myQD.UpdateBTOSMainItemDueDate(_QuoteId)
    'End Sub

    'Public Shared Sub RefreshPartInventory(ByVal _QuoteId As String, ByVal _lineNumber As Integer, ByVal oType As COMM.Fixer.eDocType)

    '    'Frank 2012/09/18
    '    '1.component
    '    ' 1.1 if available qty can not be found，then available date need to be changed to 12/31/9999；set available qty=0
    '    ' 1.2 if available qty is not enough for request qty, then available date need to be changed to 12/31/9999；and set available qty as real stock
    '    '2.BTOS 100 line：No need to query available qty，but available date need to be updated by lastest available date(due date) of sub components；and set available qty as request qty
    '    '3.If part's pref string is AGS-：No need to query available qty and set available date as today, set available qty as request qty

    '    '!!!!!!!!Please do not use session in this function!!!!!!!!!!!!!

    '    Dim myQD As New EQDSTableAdapters.QuotationDetailTableAdapter, DT As EQDS.QuotationDetailDataTable
    '    Dim QMDT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteId, oType)
    '    If _lineNumber < 1 Then
    '        DT = myQD.GetQuoteDetailById(_QuoteId)
    '    Else
    '        DT = myQD.GetSpecificLinePN(_QuoteId, _lineNumber)
    '    End If

    '    Dim prod_input As New SAPDAL.SAPDALDS.ProductInDataTable, _sapdal As New SAPDAL.SAPDAL, _errormsg As String = String.Empty
    '    Dim _deliveryPlant As String = "USH1", inventory_out As New SAPDAL.SAPDALDS.QueryInventory_OutputDataTable

    '    'Reading partno and require qty, fill into product in table
    '    For Each _row As EQDS.QuotationDetailRow In DT.Rows
    '        If String.IsNullOrEmpty(_row.partNo) Then Continue For
    '        If COMM.CartFixer.isValidParentLineNo(_row.line_No) Then Continue For
    '        prod_input.AddProductInRow(_row.partNo, _row.qty, _row.deliveryPlant)
    '    Next
    '    'Get real time inventory
    '    _sapdal.QueryInventory_V2(prod_input, _deliveryPlant, Now, inventory_out, _errormsg)
    '    Dim _MatchInventoryRow() As SAPDAL.SAPDALDS.QueryInventory_OutputRow = Nothing, _STOCK_DATE As String = String.Empty, _STOCK_DATE_NEXTHOLIDAY As String = String.Empty, _code As String = "TW"
    '    Dim _StockTotalValue As Integer = 0
    '    For Each _row As EQDS.QuotationDetailRow In DT.Rows

    '        If String.IsNullOrEmpty(_row.partNo) Then Continue For

    '        If _row.IsNull("dueDate") Then _row.dueDate = Now.Date

    '        'BTOS main item no need to query inventory
    '        If COMM.CartFixer.isValidParentLineNo(_row.line_No) Then
    '            myQD.UpdateProductAvaiableInfoByLineNo(_row.qty, _row.dueDate.ToString("yyyy-MM-dd"), _QuoteId, _row.line_No)
    '            Continue For
    '        End If

    '        'If part number start with AGS- , then just updating due date as today
    '        If _row.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase) Then
    '            myQD.UpdateProductAvaiableInfoByLineNo(_row.qty, Now.ToString("yyyy-MM-dd"), _QuoteId, _row.line_No)
    '            Continue For
    '        End If

    '        'If part's inventory cannot be found.
    '        If inventory_out.Rows.Count = 0 Then
    '            If (QMDT.DocReg And COMM.Fixer.eDocReg.AUS) = QMDT.DocReg Then
    '                myQD.UpdateProductAvaiableInfoByLineNo(0, "9999-12-31", _QuoteId, _row.line_No)
    '                Continue For
    '            Else
    '                myQD.UpdateProductAvaiableInfoByLineNo(0, Now.Date.AddDays(SAPTools.getLeadTime(_row.partNo, _row.deliveryPlant)), _QuoteId, _row.line_No)
    '                Continue For
    '            End If
    '        End If

    '        _MatchInventoryRow = inventory_out.Select("PART_NO='" & _row.partNo & "' and PLANT='" & _row.deliveryPlant & "'")

    '        If _MatchInventoryRow.Length > 0 Then
    '            'According to the quoteid and line no, update its inventory and duedate
    '            If Not _STOCK_DATE.Equals(CDate(_MatchInventoryRow(0).STOCK_DATE).ToString("yyyy-MM-dd"), StringComparison.InvariantCultureIgnoreCase) Then
    '                _STOCK_DATE = CDate(_MatchInventoryRow(_MatchInventoryRow.Count - 1).STOCK_DATE).ToString("yyyy-MM-dd")
    '                _STOCK_DATE_NEXTHOLIDAY = _STOCK_DATE
    '                SAPDAL.SAPDAL.Get_Next_WorkingDate_ByCode(_STOCK_DATE_NEXTHOLIDAY, 0, _code)
    '            End If
    '            'Calculate total available qty
    '            _StockTotalValue = 0
    '            _StockTotalValue = inventory_out.Compute("Sum(STOCK)", "PART_NO = '" & _row.partNo & "'")

    '            If _StockTotalValue >= _row.qty Then
    '                myQD.UpdateProductAvaiableInfoByLineNo(_StockTotalValue, _STOCK_DATE_NEXTHOLIDAY, _QuoteId, _row.line_No)
    '            Else
    '                If (QMDT.DocReg And COMM.Fixer.eDocReg.AUS) = QMDT.DocReg Then
    '                    myQD.UpdateProductAvaiableInfoByLineNo(_StockTotalValue, "9999-12-31", _QuoteId, _row.line_No)
    '                Else
    '                    myQD.UpdateProductAvaiableInfoByLineNo(_StockTotalValue, Now.Date.AddDays(SAPTools.getLeadTime(_row.partNo, _row.deliveryPlant)), _QuoteId, _row.line_No)
    '                    Continue For
    '                End If

    '            End If
    '        Else
    '            If (QMDT.DocReg And COMM.Fixer.eDocReg.AUS) = QMDT.DocReg Then
    '                myQD.UpdateProductAvaiableInfoByLineNo(0, "9999-12-31", _QuoteId, _row.line_No)
    '            Else
    '                myQD.UpdateProductAvaiableInfoByLineNo(0, Now.Date.AddDays(SAPTools.getLeadTime(_row.partNo, _row.deliveryPlant)), _QuoteId, _row.line_No)
    '                Continue For
    '            End If
    '        End If
    '    Next
    'End Sub

    Public Shared Sub RefreshQuotationInventory(ByVal _QuoteId As String)

        'Line numer= -1 means refresh available qty and date for all parts in a qoute
        'RefreshPartInventory(_QuoteId, -1, COMM.Fixer.eDocType.EQ)
        MyQuoteX.RefreshPartInventory(_QuoteId, -1)

        'RefreshBTOSMainitemAvailableDate(_QuoteId)

    End Sub

    Public Shared Function GetBTOSLevel1DefaultComponents(ByVal BTOItem As String, ByVal OrgId As String) As ArrayList
        Dim strSql As String =
            " select distinct b.PART_NO " +
            " from CBOM_CATALOG_CATEGORY a inner join SAP_PRODUCT b on a.CATEGORY_ID=b.PART_NO  " +
            " where CATEGORY_TYPE='Component' and ORG=@ORG and CONFIGURATION_RULE='DEFAULT' and PARENT_CATEGORY_ID in " +
            " ( " +
            " 	select CATEGORY_ID from CBOM_CATALOG_CATEGORY where PARENT_CATEGORY_ID=@BTOITEM and ORG=@ORG and CATEGORY_TYPE='Category' " +
            " ) " +
            " order by b.PART_NO "
        Dim apt As New SqlClient.SqlDataAdapter(strSql, ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        With apt.SelectCommand.Parameters
            .AddWithValue("ORG", OrgId) : .AddWithValue("BTOITEM", BTOItem)
        End With
        Dim comptDt As New DataTable
        apt.Fill(comptDt)
        apt.SelectCommand.Connection.Close()
        Dim aCompList As New ArrayList
        For Each r As DataRow In comptDt.Rows
            aCompList.Add(r.Item("PART_NO"))
        Next
        Return aCompList
    End Function

    Public Shared Function getCBOMDataSource(ByVal orgid As String, ByVal systemid As String) As DataSet
        Dim strSql As String =
      " EXEC SPGETCBOM @ORG,@CATEGORY_ID  "
        Dim apt As New SqlClient.SqlDataAdapter(strSql, ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        With apt.SelectCommand.Parameters
            .AddWithValue("ORG", orgid) : .AddWithValue("CATEGORY_ID", systemid)
        End With
        Dim cbomdatasource As New DataSet
        apt.Fill(cbomdatasource)
        apt.SelectCommand.Connection.Close()
        'add price table

        'add atp table

        Return cbomdatasource
    End Function

    Public Shared Function GetCategoryName(ByVal PartNo As String, ByVal Org_id As String) As String
        If Not String.IsNullOrEmpty(PartNo) AndAlso Not String.IsNullOrEmpty(Org_id) Then
            Dim org As String = Org_id.Trim.Substring(0, 2)
            Dim sb As New StringBuilder
            sb.Append("  select top 1 category_desc  from   CBOM_CATALOG_CATEGORY where category_id = ")
            sb.AppendLine(String.Format(" (select top 1 parent_category_id  from   CBOM_CATALOG_CATEGORY where category_id ='{0}' AND org ='{1}') ", PartNo, org))
            sb.Append("  and category_desc <> '' and category_desc is not null order by LEN(category_desc)")
            Dim catname As Object = tbOPBase.dbExecuteScalar("MY", sb.ToString)
            If catname IsNot Nothing Then
                Return catname.ToString.Trim
            End If

            'ICC 2015/6/1 Double check SAP_PRODUCT_MATERIAL_GROUP table to get description. By IC
            sb.Clear()
            sb.Append(" SELECT TOP 1 mg.MATERIAL_GROUP_DESCRIPTION FROM SAP_PRODUCT_MATERIAL_GROUP mg ")
            sb.Append(String.Format(" INNER JOIN SAP_PRODUCT sp ON mg.MATERIAL_GROUP = sp.MATERIAL_GROUP WHERE sp.PART_NO = '{0}' and sp.MATERIAL_GROUP <> 'PRODUCT'", PartNo))
            catname = tbOPBase.dbExecuteScalar("MY", sb.ToString)
            If catname IsNot Nothing Then
                Return catname.ToString.Trim
            End If
        End If
        Return "OTHER"
    End Function

    Public Shared Function replaceCartBTO(ByVal PN As String, ByVal Org As String) As String
        If PN.StartsWith("EZ-") Then PN = PN.Substring(3, PN.Length - 3)
        Dim vnumber As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PN.ToString, Org.ToUpper.Substring(0, 2)))
        If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
            Dim MaterialGroup As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select TOP 1 MATERIAL_GROUP from SAP_PRODUCT where PART_NO = '{0}' ", vnumber.ToString))
            If Not IsNothing(MaterialGroup) AndAlso MaterialGroup.ToString.Equals("BTOS", StringComparison.OrdinalIgnoreCase) Then
                Return vnumber
            End If
        End If
        vnumber = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PN.ToString.Trim + "-BTO", Org.ToUpper.Substring(0, 2)))
        If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
            Dim MaterialGroup As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select TOP 1 MATERIAL_GROUP from SAP_PRODUCT where PART_NO = '{0}' ", vnumber.ToString))
            If Not IsNothing(MaterialGroup) AndAlso MaterialGroup.ToString.Equals("BTOS", StringComparison.OrdinalIgnoreCase) Then
                Return vnumber
            End If
        End If
        If PN.Trim.EndsWith("-BTO") Then
            Dim Temp_PN = PN.Substring(0, PN.Length - 4)
            vnumber = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", Temp_PN, Org.ToUpper.Substring(0, 2)))
            If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
                Dim MaterialGroup As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select TOP 1 MATERIAL_GROUP from SAP_PRODUCT where PART_NO = '{0}' ", vnumber.ToString))
                If Not IsNothing(MaterialGroup) AndAlso MaterialGroup.ToString.Equals("BTOS", StringComparison.OrdinalIgnoreCase) Then
                    Return vnumber
                End If
            End If
        Else
            Return PN
        End If
        Return PN
    End Function
    Public Shared Function IsNumericItem(ByVal part_no As String) As Boolean

        Dim pChar() As Char = part_no.ToCharArray()

        For i As Integer = 0 To pChar.Length - 1
            If Not IsNumeric(pChar(i)) Then
                Return False
                Exit Function
            End If
        Next

        Return True
    End Function
    Public Shared Sub UpdatePartnerTableFromSAP(ByVal QuoteId As String)
        Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
        Dim strQuoteId As String = Trim(QuoteId)
        Dim dtPartners As EQDS.EQPARTNERDataTable = apt.GetPartnersByQuoteId(strQuoteId)

        For Each PartnerRow As EQDS.EQPARTNERRow In dtPartners
            Dim pType As EnumSetting.PartnerTypes
            Select Case PartnerRow.TYPE
                Case "S"
                    pType = EnumSetting.PartnerTypes.ShipTo
                Case "B"
                    pType = EnumSetting.PartnerTypes.BillTo
                Case "SOLDTO"
                    pType = EnumSetting.PartnerTypes.SoldTo
            End Select
            If String.IsNullOrEmpty(PartnerRow.ERPID) = False AndAlso
                (pType = EnumSetting.PartnerTypes.ShipTo Or pType = EnumSetting.PartnerTypes.BillTo Or pType = EnumSetting.PartnerTypes.SoldTo) Then
                Dim strCompanyName As String = "", strAttention As String = "", strTel As String = "", strAddr As String = "", strMobile As String = ""
                If SAPTools.GetSAPPartnerProfile(PartnerRow.ERPID, pType, strCompanyName, strAddr, strAttention, strTel, strMobile) Then
                    apt.UpdatePartnerAttributes(strCompanyName, strAddr, strAttention, strTel, strMobile, strQuoteId, "", "", "", "", "", "", PartnerRow.TYPE, PartnerRow.FAX)
                    tbOPBase.adddblog(String.Format("UPDATE EQPARTNER SET NAME = '{0}', ADDRESS ='{1}', ATTENTION = '{2}'," &
                                      " TEL = '{3}', MOBILE = '{4}', ZIPCODE = '{5}', " &
                                        " COUNTRY = '{6}', CITY = '{7}', STREET = '{8}', STATE = '{9}', DISTRICT = '{10}' " &
                                         " WHERE (QUOTEID = '{11}') AND (TYPE = '{12}')", strCompanyName, strAddr, strAttention, strTel, strMobile, strQuoteId, "", "", "", "", "", "", PartnerRow.TYPE))
                End If
            End If
        Next
    End Sub
    Shared Function getNCNRStr() As String
        Dim pageHolder As New TBBasePage()
        pageHolder.IsVerifyRender = False
        Dim ControlURL As String = "~/Ascx/NCNRAgreement.ascx"

        Dim cw1 As UserControl = DirectCast(pageHolder.LoadControl(ControlURL), UserControl)
        Dim viewControlType As Type = cw1.GetType

        Dim _meth As Reflection.MethodInfo = viewControlType.GetMethod("LoadData")
        _meth.Invoke(cw1, Nothing)
        pageHolder.Controls.Add(cw1)

        Dim output As New IO.StringWriter()
        HttpContext.Current.Server.Execute(pageHolder, output, False)
        Return output.ToString()
    End Function


    Shared Function getPageStrInternal(ByVal UID As String, ByVal DocReg As COMM.Fixer.eDocReg, Optional ByVal IsInternalUser As Boolean = True) As String
        'Frank 2014/05/12
        'Fei also need to approve the EU quotation, so the EU quotation need to be presented with EU template, not US template.
        'For example:GQ063524 was created by AEU sales, but the GP approval was going to Fei
        'If HttpContext.Current.Session IsNot Nothing Then
        If HttpContext.Current.Session IsNot Nothing AndAlso DocReg <> COMM.Fixer.eDocReg.AEU Then

            Dim qm As Quote_Master = MyQuoteX.GetQuoteMaster(UID)

            Dim _IsUsaUser As Boolean = Role.IsUsaUser
            Dim _IsAAC As Boolean = False

            If _IsUsaUser Then
                If qm.quoteNo.StartsWith("AACQ") Then
                    _IsAAC = True
                End If
            End If


            Dim _IsTWAonlineSales As Boolean = Role.IsTWAonlineSales
            Dim _IsCNAonlineSales As Boolean = Role.IsCNAonlineSales
            Dim _IsJPAonlineSales As Boolean = Role.IsJPAonlineSales
            Dim _IsKRAonlineSales As Boolean = Role.IsKRAonlineSales
            Dim _IsHQDCSales As Boolean = Role.IsHQDCSales
            Dim _IsAENCSales As Boolean = Role.IsUsaAenc
            Dim _IsABRSales As Boolean = Role.IsABRSales
            Dim _IsAAUSales As Boolean = Role.IsAAUSales
            Dim _IsCAPS As Boolean = Role.IsCAPS

            If _IsJPAonlineSales Or
                _IsUsaUser Or
                _IsTWAonlineSales Or
                _IsCNAonlineSales Or
                _IsKRAonlineSales Or
                _IsHQDCSales Or
                _IsABRSales Or
                _IsAAUSales Or
                _IsAENCSales Or
                _IsCAPS Then

                Dim pageHolder As New TBBasePage()
                pageHolder.IsVerifyRender = False
                Dim ControlURL As String = "~/Ascx/USAOnlineQuoteTemplate.ascx"

                'Frank 20150407 ANA iA has their own quote template
                If _IsAAC Then
                    ControlURL = "~/Ascx/USANAiAQuoteTemplate.ascx"
                End If

                If _IsAENCSales OrElse _IsCAPS Then
                    ControlURL = "~/Ascx/USAENCQuoteTemplate.ascx"
                End If

                If _IsTWAonlineSales Then
                    ControlURL = "~/Ascx/TWAOnlineQuoteTemplate.ascx"
                End If
                If _IsCNAonlineSales Then
                    ControlURL = "~/Ascx/CNAOnlineQuoteTemplate.ascx"
                End If
                If _IsJPAonlineSales Then
                    ControlURL = "~/Ascx/JPAOnlineQuoteTemplateV4.ascx"
                End If

                If _IsKRAonlineSales Then
                    ControlURL = "~/Ascx/KRAOnlineQuoteTemplate.ascx"
                End If

                If _IsHQDCSales Then
                    ControlURL = "~/Ascx/InterconQuoteTemplate.ascx"
                End If

                If _IsABRSales Then
                    ControlURL = "~/Ascx/BRAOnlineQuoteTemplate.ascx"
                End If


                Dim cw1 As UserControl = DirectCast(pageHolder.LoadControl(ControlURL), UserControl)
                Dim viewControlType As Type = cw1.GetType
                Dim p_QuoteId As Reflection.PropertyInfo = viewControlType.GetProperty("QuoteId")
                p_QuoteId.SetValue(cw1, UID, Nothing)
                Dim p_IsInternalUser As Reflection.PropertyInfo = viewControlType.GetProperty("IsInternalUser")
                p_IsInternalUser.SetValue(cw1, IsInternalUser, Nothing)
                Dim _meth As Reflection.MethodInfo = viewControlType.GetMethod("LoadData")
                _meth.Invoke(cw1, Nothing)
                pageHolder.Controls.Add(cw1)
                Dim output As New IO.StringWriter()
                HttpContext.Current.Server.Execute(pageHolder, output, False)
                Return output.ToString()
            End If
        End If

        Dim logoBlock As String = "", headerBlock As String = "", detailBlock As String = ""
        Dim _ROLE As Integer = 0
        If Not IsInternalUser Then _ROLE = 1
        Dim url As String = String.Format("~/Quote/{1}?UID={0}&ROLE={2}", UID, Business.getPiPage(UID, DocReg), _ROLE)
        Dim MyDOC As New System.Xml.XmlDocument
        Util.HtmlToXML(url, MyDOC)
        Util.getXmlBlockByID("div", "divLogo", MyDOC, logoBlock)
        Util.getXmlBlockByID("div", "divMaster", MyDOC, headerBlock)
        Util.getXmlBlockByID("div", "divDetail", MyDOC, detailBlock)
        Return logoBlock & headerBlock & detailBlock
    End Function


    Public Shared Function GetRuntimeSiteUrl() As String
        With HttpContext.Current
            Return String.Format("http://{0}{1}{2}",
                                 .Request.ServerVariables("SERVER_NAME"),
                                 IIf(.Request.ServerVariables("SERVER_PORT") = "80", "", ":" + .Request.ServerVariables("SERVER_PORT")),
                                 IIf(HttpRuntime.AppDomainAppVirtualPath = "/", "", HttpRuntime.AppDomainAppVirtualPath))
        End With

    End Function
    Shared Function GetAJPAccountFax(ByVal QuoteID As String, ByVal oType As COMM.Fixer.eDocType) As String

        Dim EQM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(QuoteID, oType)
        If EQM IsNot Nothing Then
            If EQM.AccErpId IsNot Nothing AndAlso Not String.IsNullOrEmpty(EQM.AccErpId) Then
                Dim Faxobj As Object = tbOPBase.dbExecuteScalar("B2B", String.Format("SELECT TOP 1 isnull(FAX_NO,'') as FAX FROM  SAP_DIMCOMPANY WHERE COMPANY_ID ='{0}'", EQM.AccErpId))
                If Faxobj IsNot Nothing AndAlso Not String.IsNullOrEmpty(Faxobj) Then
                    Return Faxobj.ToString.Trim
                End If
            End If
            If EQM.AccRowId IsNot Nothing AndAlso Not String.IsNullOrEmpty(EQM.AccRowId) Then
                Dim Faxobj As Object = tbOPBase.dbExecuteScalar("B2B", String.Format("SELECT   TOP 1 isnull(FAX_NUM,'') as FAX  FROM  SIEBEL_ACCOUNT WHERE ROW_ID ='{0}'", EQM.AccRowId))
                If Faxobj IsNot Nothing AndAlso Not String.IsNullOrEmpty(Faxobj) Then
                    Return Faxobj.ToString.Trim
                End If
            End If
        End If
        Return String.Empty
    End Function

    Shared Function getMargin(ByVal UID As String) As Decimal
        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim QDDT As New DataTable
        QDDT = myQD.GetDT(String.Format("quoteId='{0}'", UID), "line_No")
        Dim detail As New List(Of struct_GP_Detail)

        For Each x As DataRow In QDDT.Rows
            Dim detailLine As New struct_GP_Detail
            detailLine.lineNo = x.Item("line_no")
            detailLine.PartNo = x.Item("partno")
            detailLine.Price = x.Item("newunitprice")
            detailLine.QTY = x.Item("qty")
            detailLine.Itp = x.Item("newitp")
            detail.Add(detailLine)
        Next

        If detail.Count > 0 Then

            'Ryan 20160506 Add items validation
            detail = GPControl.GPDetailValidation(detail)

            Dim MSTD As Decimal = GPControl.getMarginWithOutAGS(detail)
            Dim MPTD As Decimal = GPControl.getMarginPTD(detail)
            Dim MLST As Decimal = -99999
            If MSTD <> -99999 Then
                MLST = MSTD
            Else
                MLST = MPTD
            End If
            If MLST = -99999 Then MLST = 0
            Return FormatNumber(MLST * 100, 2)
        End If
        Return 0
    End Function

    Shared Function getStandardPartsMargin(ByVal UID As String) As Decimal
        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim QDDT As New DataTable
        QDDT = myQD.GetDT(String.Format("quoteId='{0}'", UID), "line_No")
        Dim detail As New List(Of struct_GP_Detail)

        For Each x As DataRow In QDDT.Rows
            Dim detailLine As New struct_GP_Detail
            detailLine.lineNo = x.Item("line_no")
            detailLine.PartNo = x.Item("partno")
            detailLine.Price = x.Item("newunitprice")
            detailLine.QTY = x.Item("qty")
            detailLine.Itp = x.Item("newitp")
            detail.Add(detailLine)
        Next

        If detail.Count > 0 Then

            'Ryan 20160506 Add items validation
            detail = GPControl.GPDetailValidation(detail)

            Dim MSTD As Decimal = GPControl.getMarginWithOutAGS(detail)

            Dim MLST As Decimal = -99999
            If MSTD <> -99999 Then
                MLST = MSTD
            End If
            If MLST = -99999 Then MLST = 0
            Return FormatNumber(MLST * 100, 2)
        End If
        Return 0
    End Function

    Shared Function getPTDItemsMargin(ByVal UID As String) As Decimal
        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim QDDT As New DataTable
        QDDT = myQD.GetDT(String.Format("quoteId='{0}'", UID), "line_No")
        Dim detail As New List(Of struct_GP_Detail)

        For Each x As DataRow In QDDT.Rows
            Dim detailLine As New struct_GP_Detail
            detailLine.lineNo = x.Item("line_no")
            detailLine.PartNo = x.Item("partno")
            detailLine.Price = x.Item("newunitprice")
            detailLine.QTY = x.Item("qty")
            detailLine.Itp = x.Item("newitp")
            detail.Add(detailLine)
        Next

        If detail.Count > 0 Then

            'Ryan 20160506 Add items validation
            detail = GPControl.GPDetailValidation(detail)

            Dim MPTD As Decimal = GPControl.getMarginPTD(detail)
            Dim MLST As Decimal = -99999
            If MPTD <> -99999 Then
                MLST = MPTD
            End If
            If MLST = -99999 Then MLST = 0
            Return FormatNumber(MLST * 100, 2)
        End If
        Return 0
    End Function

    Shared Function getAllItemsMargin(ByVal UID As String) As Decimal
        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim QDDT As New DataTable
        QDDT = myQD.GetDT(String.Format("quoteId='{0}'", UID), "line_No")
        Dim detail As New List(Of struct_GP_Detail)

        For Each x As DataRow In QDDT.Rows
            Dim detailLine As New struct_GP_Detail
            detailLine.lineNo = x.Item("line_no")
            detailLine.PartNo = x.Item("partno")
            detailLine.Price = x.Item("newunitprice")
            detailLine.QTY = x.Item("qty")
            detailLine.Itp = x.Item("newitp")
            detail.Add(detailLine)
        Next

        If detail.Count > 0 Then

            Dim M As Decimal = GPControl.getTotalMargin(detail)
            Dim MLST As Decimal = -99999
            If M <> -99999 Then
                MLST = M
            End If
            If MLST = -99999 Then MLST = 0
            Return FormatNumber(MLST * 100, 2)
        End If
        Return 0
    End Function

    Shared Function getAllItemsITP(ByVal UID As String) As Decimal
        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim QDDT As New DataTable
        QDDT = myQD.GetDT(String.Format("quoteId='{0}'", UID), "line_No")
        Dim detail As New List(Of struct_GP_Detail)

        For Each x As DataRow In QDDT.Rows
            Dim detailLine As New struct_GP_Detail
            detailLine.lineNo = x.Item("line_no")
            detailLine.PartNo = x.Item("partno")
            detailLine.Price = x.Item("newunitprice")
            detailLine.QTY = x.Item("qty")
            detailLine.Itp = x.Item("newitp")
            detail.Add(detailLine)
        Next

        Dim result As Decimal = 0
        'If detail.Count > 0 Then
        '    For Each d As struct_GP_Detail In detail
        '        If Not Business.IsPTD(d.PartNo) And Not Advantech.Myadvantech.Business.PartBusinessLogic.IsNonStandardSensitiveITP(d.PartNo) Then
        '            result = result + (d.QTY * d.Itp)
        '        End If
        '    Next
        'End If

        'Ryan 20170417 Get all items ITP instead of Standard parts ITP per YC's request.
        If detail.Count > 0 Then
            For Each d As struct_GP_Detail In detail
                result = result + (d.QTY * d.Itp)
            Next
        End If
        Return result
    End Function

    Shared Function getShiptoERPID(ByVal quoteid As String) As String
        Dim SB As New EQPARTNER("EQ", "EQPARTNER")
        Dim DTS As New DataTable
        DTS = SB.GetDT(String.Format("QuoteId='{0}' AND TYPE='S'", quoteid), "")
        If DTS.Rows.Count > 0 Then
            Return DTS.Rows.Item("ERPID").ToString
        End If
        Return ""
    End Function
    Shared Function getBILLtoERPID(ByVal quoteid As String) As String
        Dim SB As New EQPARTNER("EQ", "EQPARTNER")
        Dim DTB As New DataTable
        DTB = SB.GetDT(String.Format("QuoteId='{0}' AND TYPE='B'", quoteid), "")
        If DTB.Rows.Count > 0 Then
            Return DTB.Rows.Item("ERPID").ToString
        End If
        Return ""
    End Function
    Shared Function GetCurrentPriceYearQuarter(ByRef pYear As String, ByVal pQuarter As String, ByVal org As String) As Boolean
        Dim pRBU As String = ""
        Select Case UCase(org)
            Case "AH"
                pRBU = ""
            Case "AU"
                pRBU = "AAU"
            Case "BR"
                pRBU = "ABR"
            Case "CN"
                pRBU = "ABJ"
            Case "DL"
                pRBU = ""
            Case "EU"
                pRBU = "AESC"
            Case "IN"
                pRBU = "HQDC"
            Case "JP"
                pRBU = "AJP"
            Case "KR"
                pRBU = "AKR"
            Case "MY"
                pRBU = "AMY"
            Case "SG"
                pRBU = "ASG"
            Case "TL"
                pRBU = ""
            Case "TW"
                pRBU = "ATW"
            Case "US"
                pRBU = "AAC"
            Case Else
                pRBU = ""
                Return False
        End Select
        If pRBU <> "" Then
            Dim dt As DataTable = tbOPBase.dbGetDataTable("EPRICER",
                                  " select pricec_dts_year, pricec_dts_quarter, org from Price_Control " +
                                  " where org='" + pRBU + "' and GETDATE()>=pricec_start_date and GETDATE()<pricec_end_date+1")
            If dt.Rows.Count = 1 Then
                pYear = dt.Rows(0).Item("pricec_dts_year").ToString()
                pQuarter = dt.Rows(0).Item("pricec_dts_quarter").ToString()
                Return True
            End If
        End If
        Return False
    End Function
    Shared Function isExpired(ByVal quoteId As String, ByVal oType As COMM.Fixer.eDocType) As Boolean
        Dim f As Boolean = False
        Dim DTM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(quoteId, oType)
        If Not IsNothing(DTM) Then
            Dim expDate As Date = CType(DTM.expiredDate, Date)
            Dim quoteDate As Date = CType(DTM.DocDate, Date)
            If isRepeatedOrder(quoteId, oType) Then
                If DateDiff(DateInterval.Day, quoteDate.AddMonths(6), Now()) > 0 Then
                    f = True
                End If
            Else
                If DateDiff(DateInterval.Day, expDate, Now()) > 0 Then
                    f = True
                End If
            End If
        Else
            f = True
        End If
        Return f
    End Function

    Shared Function isExpired(ByVal quoteId As String, ByVal quoteDate As String, ByVal expiredDate As String, ByVal isRepeatedOrder As Integer) As Boolean
        If String.IsNullOrEmpty(quoteDate) Or String.IsNullOrEmpty(expiredDate) Then
            Return False
        Else
            Dim f As Boolean = False
            Dim _expDate As Date = CType(expiredDate, Date)
            Dim _quoteDate As Date = CType(quoteDate, Date)
            If isRepeatedOrder Then
                If DateDiff(DateInterval.Day, _quoteDate.AddMonths(6), Now()) > 0 Then
                    f = True
                End If
            Else
                If DateDiff(DateInterval.Day, _expDate, Now()) > 0 Then
                    f = True
                End If
            End If
            Return f

        End If
    End Function
    Shared Function Add2CartCheck(ByVal partNo As String, ByVal orgID As String, ByVal desc As String) As Boolean
        If partNo.Trim = "" Then Return False
        Dim DT As DataTable = checkProduct(orgID, partNo, desc)
        If DT.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Shared Function Format2SAPItem(ByVal Part_No As String) As String
        'Return Relics.SAPDAL.Format2SAPItem(Part_No)
        Return SAPDAL.SAPDAL.Format2SAPItem(Part_No)
    End Function


    Shared Function GetNewUID() As String
        Dim H As IBUS.iDocHeader = Pivot.NewObjDocHeader
        Dim M As IBUS.iDocHeaderLine = Pivot.NewLineHeader
        If Role.IsUsaUser() Then
            M.AccErpId = "UEPP5001" : M.AccOfficeName = "ANA-DMF" : M.siebelRBU = "ANADMF" : M.org = "US01" : M.currency = "USD"
        ElseIf Role.IsEUSales() Then
            M.AccErpId = "EDATEV01" : M.AccOfficeName = "ADL" : M.siebelRBU = "ADL" : M.org = "EU10" : M.currency = "EUR"
        ElseIf Role.IsTWAonlineSales() Then
            M.AccErpId = "2NC00001" : M.AccOfficeName = "ATW" : M.siebelRBU = "ATW" : M.org = "TW01" : M.currency = "TWD"
        End If
        M.AccRowId = "preQuote"
        M.AccName = "preQuote"
        M.CustomId = "preQuote"
        M.preparedBy = Pivot.CurrentProfile.UserId
        M.Active = 1
        M.Revision_Number = 1
        Return H.Add(M, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ).Key
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="org"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getSapShipTermsByOrg(ByVal org As String) As DataTable
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", String.Format("select distinct SHIPTERM as Value, SHIPTERMNAME as Name from SAP_COMPANY_LOV where ORG_ID='{0}' and SHIPTERM<>'' order by SHIPTERM", org))
        Return dt
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="org"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getSapShipTermNameByOrg(ByVal org As String, ByVal shiptermID As String) As String
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", String.Format("select distinct SHIPTERM as Value, isnull(SHIPTERMNAME,'') as Name from SAP_COMPANY_LOV where ORG_ID='{0}' and SHIPTERM='{1}' and SHIPTERM<>'' order by SHIPTERM", org, shiptermID))
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("Name").ToString()
        End If
        Return String.Empty
    End Function



    Shared Function getSapPayTermsFORANA(ByVal org As String) As DataTable

        'Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", String.Format("select distinct PAYMENTTERM as Value, PAYMENTTERMNAME as Name from SAP_COMPANY_LOV where ORG_ID='{0}' and PAYMENTTERM<>'' and PAYMENTTERMNAME<>'' order by PAYMENTTERM", org))

        Dim dt As DataTable = Nothing, sql As New StringBuilder
        sql.AppendLine(" Select distinct PAYMENTTERM as Value, PAYMENTTERMNAME as Name from SAP_COMPANY_LOV ")
        sql.AppendLine(" where ORG_ID='" & org & "' and PAYMENTTERM<>'' and PAYMENTTERMNAME<>'' ")
        sql.AppendLine(" Order by PAYMENTTERM ")

        dt = tbOPBase.dbGetDataTable("B2B", sql.ToString)

        If org.Equals("KR01", StringComparison.InvariantCultureIgnoreCase) Then
            Dim _row As DataRow() = dt.Select("value='PPD'")
            If _row Is Nothing OrElse _row.Length = 0 Then
                Dim newrow As DataRow = dt.NewRow
                newrow.Item("Value") = "PPD" : newrow.Item("Name") = "Cash advance"
                dt.Rows.Add(newrow)
                dt.AcceptChanges()
            End If
        End If

        Return dt
    End Function

    Public Shared Function getRevisionNumberList(ByVal QuoteNo As String) As DataTable

        If String.IsNullOrEmpty(QuoteNo) Then Return Nothing

        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)

        Dim _sql As New StringBuilder
        _sql.AppendLine(" SELECT QuoteNo,Revision_Number,Active FROM QuotationMaster ")
        _sql.AppendLine(" Where QuoteNo=@QuoteNo ")
        _sql.AppendLine(" Group by QuoteNo,Revision_Number,Active ")
        _sql.AppendLine(" Order by QuoteNo,Revision_Number ")

        Dim cmd As New SqlClient.SqlCommand(_sql.ToString, conn)
        With cmd.Parameters
            .AddWithValue("QuoteNo", QuoteNo)
        End With
        conn.Open()

        Dim myReader As SqlClient.SqlDataReader = cmd.ExecuteReader()

        Dim _dt As New DataTable
        _dt.Load(myReader)
        Return _dt
    End Function


    Public Shared Function getSalesOfficeList(ByVal ORGID As String) As DataTable

        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)

        Dim _sql As New StringBuilder
        _sql.AppendLine(" SELECT SALES_OFFICE_CODE,SALES_OFFICE FROM SAP_ORG_OFFICE_GRP ")
        _sql.AppendLine(" Where SALES_ORG=@orgid ")
        _sql.AppendLine(" Group by SALES_OFFICE_CODE,SALES_OFFICE ")
        _sql.AppendLine(" Order by SALES_OFFICE_CODE ")

        Dim cmd As New SqlClient.SqlCommand(_sql.ToString, conn)
        With cmd.Parameters
            .AddWithValue("orgid", ORGID)
        End With
        conn.Open()

        Dim myReader As SqlClient.SqlDataReader = cmd.ExecuteReader()

        Dim _dt As New DataTable
        _dt.Load(myReader)
        Return _dt
    End Function

    Public Shared Function getSalesGroupListByOffice(ByVal ORGID As String, ByVal OfficeCode As String) As DataTable

        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)

        Dim _sql As New StringBuilder
        _sql.AppendLine(" SELECT SALES_GROUP_CODE,SALES_GROUP FROM MyAdvantechGlobal.dbo.SAP_ORG_OFFICE_GRP ")
        _sql.AppendLine(" Where SALES_ORG=@orgid and SALES_OFFICE_CODE=@officecode ")
        _sql.AppendLine(" Group by SALES_GROUP_CODE,SALES_GROUP ")
        _sql.AppendLine(" Order by SALES_GROUP_CODE ")

        Dim cmd As New SqlClient.SqlCommand(_sql.ToString, conn)
        With cmd.Parameters
            .AddWithValue("orgid", ORGID) : .AddWithValue("officecode", OfficeCode)
        End With
        conn.Open()

        Dim myReader As SqlClient.SqlDataReader = cmd.ExecuteReader()

        Dim _dt As New DataTable
        _dt.Load(myReader)
        Return _dt
    End Function



    Public Shared Function getSalesEmployeeList(ByVal ORGID As String, ByVal QuoteToERPID As String) As DataTable
        Dim _IsAonlineUsa As Boolean = Role.IsAonlineUsa()
        Dim _IsAonlineUsaIag As Boolean = Role.IsAonlineUsaIag()
        Dim _IsAonlineUsaISystem As Boolean = Role.IsAonlineUsaISystem()
        Dim _IsAEUSales As Boolean = Role.IsEUSales()

        If _IsAonlineUsa OrElse _IsAonlineUsaIag OrElse _IsAonlineUsaISystem Then

            Dim dt1 As DataTable = Nothing : Dim dt2 As DataTable = Nothing : Dim dt3 As DataTable = Nothing
            Dim _dttotal As New DataTable
            _dttotal.Columns.Add("SALES_CODE")
            _dttotal.Columns.Add("FULL_NAME")
            _dttotal.PrimaryKey = New DataColumn() {_dttotal.Columns("SALES_CODE")}

            If _IsAonlineUsa Then
                dt1 = Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetUSAonlineSalesEmployee(AOnlineRegion.AUS_AOnline, QuoteToERPID)
            End If
            If _IsAonlineUsaIag Then
                dt2 = Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetUSAonlineSalesEmployee(AOnlineRegion.AUS_AOnline_IAG, QuoteToERPID)
            End If
            If _IsAonlineUsaISystem Then
                dt3 = Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetUSAonlineSalesEmployee(AOnlineRegion.AUS_AOnline_iSystem, QuoteToERPID)
            End If
            'If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 AndAlso dt2 IsNot Nothing AndAlso dt2.Rows.Count > 0 Then
            '    dt1.PrimaryKey = New DataColumn() {dt1.Columns("sale_id")}
            '    dt1.Merge(dt2)
            '    Return dt1
            'End If
            'If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 AndAlso (dt2 Is Nothing OrElse dt2.Rows.Count = 0) Then
            '    Return dt1
            'End If
            'If dt2 IsNot Nothing AndAlso dt2.Rows.Count > 0 AndAlso (dt1 Is Nothing OrElse dt1.Rows.Count = 0) Then
            '    Return dt2
            'End If
            If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then
                _dttotal.Merge(dt1)
            End If
            If dt2 IsNot Nothing AndAlso dt2.Rows.Count > 0 Then
                _dttotal.Merge(dt2)
            End If
            If dt3 IsNot Nothing AndAlso dt3.Rows.Count > 0 Then
                _dttotal.Merge(dt3)
            End If
            dt1 = Nothing : dt2 = Nothing : dt3 = Nothing
            _dttotal.DefaultView.Sort = "FULL_NAME"
            Return _dttotal.DefaultView.ToTable()
        ElseIf Role.IsTWAonlineSales Then
            Return Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetTWAonlineSalesEmployee(QuoteToERPID)
        End If

        'AEU orgid is EU01 insteand of EU10 in SAP_EMPLOYEE table
        If _IsAEUSales Then ORGID = "EU01"

        Dim str As String = " select distinct a.FULL_NAME, a.SALES_CODE, IsNull(a.EMAIL,'') as EMAIL " &
                            " from SAP_EMPLOYEE a " &
                            " where a.PERS_AREA='" & ORGID & "' " '& _
        '" order by a.FULL_NAME "

        If ORGID.StartsWith("JP") Then
            str &= " order by a.SALES_CODE "
        Else
            str &= " order by a.FULL_NAME "
        End If

        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("B2B", str)
        Return dt
    End Function

    Public Shared Function getSalesEmployeeListWithSAP_COMPANY_EMPLOYEE(ByVal ORGID As String) As DataTable
        Dim str As String = " select distinct a.FULL_NAME, a.SALES_CODE, IsNull(a.EMAIL,'') as EMAIL " &
                            " from SAP_EMPLOYEE a inner join SAP_COMPANY_EMPLOYEE b on a.SALES_CODE=b.SALES_CODE " &
                            " where b.SALES_ORG='" & ORGID & "' and b.PARTNER_FUNCTION='VE' " &
                            " order by a.FULL_NAME "
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("B2B", str)
        Return dt
    End Function
    Public Shared Function getDefaultSalesEmployee(ByVal ORGID As String, ByVal ERPID As String) As String
        Dim STR As String = "SELECT top 1 sales_code FROM SAP_COMPANY_EMPLOYEE WHERE SALES_ORG='" & ORGID & "' AND PARTNER_FUNCTION='VE' AND COMPANY_ID='" & ERPID & "'"
        Dim o As Object = tbOPBase.dbExecuteScalar("B2B", STR)
        If Not IsNothing(o) AndAlso Not String.IsNullOrEmpty(o.ToString) Then
            Return o.ToString()
        End If
        Return ""
    End Function

    Shared Sub getShipTrem(ByVal ERPID As String, ByRef shipValue As String, ByRef shipName As String)
        Dim STR As String = " select SHIPCONDITION as Value, " &
                            " (select top 1 SHIPTERMNAME from SAP_COMPANY_LOV " &
                            " WHERE SHIPTERM=SAP_DIMCOMPANY.SHIPCONDITION) AS Name from SAP_DIMCOMPANY where COMPANY_ID='" & ERPID & "'"

        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("B2B", STR)
        If dt.Rows.Count > 0 Then
            shipValue = dt.Rows(0).Item("Value")
            shipName = dt.Rows(0).Item("Name")
        End If
    End Sub
    Shared Sub getPayMentTrem(ByVal ERPID As String, ByRef payValue As String, ByRef payName As String, ByVal ORGID As String)
        Dim STR As String = " select isnull(PAYMENT_TERM_CODE,'') as Value, " &
                            " isnull((select top 1 PAYMENTTERMNAME from SAP_COMPANY_LOV " &
                            " WHERE PAYMENTTERM=SAP_DIMCOMPANY.PAYMENT_TERM_CODE),'') AS Name from SAP_DIMCOMPANY where COMPANY_ID='" & ERPID & "'"
        If Not String.IsNullOrEmpty(ORGID) Then STR = STR & " AND ORG_ID='" & ORGID & "'"

        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("B2B", STR)
        If dt.Rows.Count > 0 Then
            payValue = dt.Rows(0).Item("Value")
            payName = dt.Rows(0).Item("Name")
        End If
    End Sub


    Shared Function SalesOfficeCode2Org(ByVal SalesOfficeCode As String) As String

        'Sales Office Code has to be an integer. ex :7000 , 7100 ...
        Dim _parseresult As Integer = -1
        If Not Integer.TryParse(SalesOfficeCode, _parseresult) Then Return Nothing
        If _parseresult <= 0 Then Return Nothing

        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        Dim cmd As New SqlClient.SqlCommand(" Select distinct ORG_ID From SAP_COMPANY_LOV where SALESOFFICE=@SalesFfficeCode", conn)
        With cmd.Parameters
            .AddWithValue("SalesFfficeCode", SalesOfficeCode)
        End With
        ' Try
        conn.Open()

        Dim myReader As SqlClient.SqlDataReader = cmd.ExecuteReader()

        Dim _dt As New DataTable
        _dt.Load(myReader)
        conn.Close()

        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then Return _dt.Rows(0).Item("ORG_ID").ToString

        Return Nothing

    End Function

    Shared Function shipCode2Txt(ByVal shipCode As String) As String
        Dim ret = ""
        Select Case shipCode.Trim()
            Case "01"
                ret = "Truck / Sea"
            Case "02"
                ret = "Pick up by customer"
            Case "03"
                ret = "Fedex"
            Case "04"
                ret = "UPS Economy"
            Case "05"
                ret = "DHL Economy"
            Case "06"
                ret = "By Material"
            Case "07"
                ret = "Air"
            Case "08"
                ret = "Service"
            Case "09"
                ret = "TNT Economy"
            Case "10"
                ret = "TNT Global"
            Case "11"
                ret = "UPS Global"
            Case "12"
                ret = "DHL Air Express"
            Case "13"
                ret = "Hand Carry"
            Case "14"
                ret = "Courier"
            Case "15"
                ret = "UPS Standard"
            Case "16"
                ret = "Cust. Own Forwarder"
            Case "17"
                ret = "TNT Economy Special"
            Case "18"
                ret = "By Sea to AKMC&ADMC"
            Case "19"
                ret = "By Sea/Air (to ACSC)"
            Case "20"
                ret = "UPS Express Saver"
            Case "21"
                ret = "UPS Expres Plus 9:00"
            Case "22"
                ret = "UPS Express 12:00"
            Case "23"
                ret = "DHL Europlus"
        End Select
        Return ret
    End Function
    Shared Function getOrgByErpId(ByVal CompanyId As String) As String
        Dim OrgId As String = ""
        Dim str As String = String.Format("select org_id from SAP_DIMCOMPANY where COMPANY_ID='{0}' and org_id not in " + ConfigurationManager.AppSettings("InvalidOrg"), CompanyId)
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("B2B", str)
        If dt.Rows.Count > 0 Then
            OrgId = dt.Rows(0).Item("org_id")
        End If
        Return OrgId
    End Function
    Shared Function getBTOPrtItem(ByVal PN As String, ByVal RBU As String, ByVal org As String) As DataTable
        Dim WS As New CBOMWS.MyCBOMDAL
        WS.Timeout = -1
        Dim DT As New DataTable
        DT = WS.getBTOParentList(RBU, org, PN)

        Return DT
    End Function





    Shared Sub ADDQuotationUpdatelog(ByVal QuoteID As String, ByVal ActionStr As String)
        Dim _ql As New EQDSTableAdapters.QuotationUpdateLogTableAdapter
        _ql.Insert(QuoteID, Now, ActionStr)
    End Sub

    Public Shared Function GetQuotationNumberType(ByVal QuoteID As String) As EnumSetting.QuotationNumberType

        If String.IsNullOrEmpty(QuoteID) Then Return EnumSetting.QuotationNumberType.NA
        QuoteID = QuoteID.ToUpper

        If QuoteID.StartsWith("AUSQ") Then Return EnumSetting.QuotationNumberType.AUSQ
        If QuoteID.StartsWith("AMXQ") Then Return EnumSetting.QuotationNumberType.AMXQ
        If QuoteID.StartsWith("AJPQ") Then Return EnumSetting.QuotationNumberType.AJPQ
        Return EnumSetting.QuotationNumberType.Other

    End Function
    Shared Function checkSpecialProductForAEU(ByVal QM As IBUS.iDocHeaderLine) As Boolean
        If String.Equals(QM.org, "EU10", StringComparison.InvariantCultureIgnoreCase) Then
            If QM.expiredDate.AddDays(-30) > DateTime.Now Then
                If MyQuoteX.IsHaveBtos(QM.Key) Then Return True
                Dim _QuoteList As List(Of QuoteItem) = MyQuoteX.GetQuoteList(QM.Key)
                If _QuoteList IsNot Nothing Then
                    Dim pn = From p In _QuoteList Select p.partNo
                    Return Advantech.Myadvantech.Business.QuoteBusinessLogic.IsPartIn30ValidDaysLimitation(pn.ToArray)
                End If
            End If
        End If
        Return False
    End Function

    Shared Function isNeedGPControl(ByVal quoteId As String, ByVal org As String) As Boolean
        If Business.isRepeatedOrder(quoteId, COMM.Fixer.eDocType.EQ) = True Then
            Return False
        End If
        If Business.isPriceUpdated(quoteId) = False Then
            Return False
        End If
        If Not (org = "EU10" OrElse Role.IsJPAonlineSales() = True) Then
            Return False
        End If
        Return True
    End Function

    Public Shared Function GetAUSCost(ByVal PartNo As String, ByVal Plant As String, ByVal quoteId As String) As Decimal
        If String.IsNullOrEmpty(PartNo) Then Return 0
        Dim AUSCost As Decimal = 0


        If Role.IsANAAOnlineTeamsByOfficeCode And Pivot.CurrentProfile.SalesOfficeCode.Contains("2110") Then
            AUSCost = QuoteBusinessLogic.GetANAPartGPBySAPGPBlockRFC(quoteId, PartNo, Plant)
            If AUSCost > 0 Then Return AUSCost
        End If

        Dim sql As New StringBuilder
        sql.AppendLine(" select A.KALN1 from SAPRDP.MBEW a ")
        'sql.AppendLine(" where a.mandt='168' and a.bwkey='USH1' ")
        sql.AppendLine(" where a.mandt='168' and a.bwkey='" & Plant & "' ")
        sql.AppendLine(" and a.matnr='" & SAPDAL.SAPDAL.FormatToSAPPartNo(PartNo) & "' ")
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sql.ToString())
        Dim _sql As String = String.Empty
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            sql.Clear()
            sql.AppendLine(" select a.STPRS as StandardPrice ")
            sql.AppendLine(" From saprdp.CKMLCR a ")
            sql.AppendLine(" where a.mandt='168' and a.KALNR='" & dt.Rows(0).Item("KALN1") & "' ")
            sql.AppendLine(" order by a.bdatj desc,a.poper desc,a.curtp desc ")
            dt = OraDbUtil.dbGetDataTable("SAP_PRD", sql.ToString())
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                AUSCost = dt.Rows(0).Item("StandardPrice")
            End If
        End If

        Return AUSCost

    End Function


    Public Shared Function GetITP(ByVal QuoteID As String, ByVal PartNo As String, ByRef ITP As Decimal, ByVal UnitPrice As String) As Boolean
        Dim Role As IBUS.iRole = Pivot.CurrentProfile
        Dim dtQuoteMaster As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(QuoteID, COMM.Fixer.eDocType.EQ)
        Dim COMPANY_ID As String = dtQuoteMaster.AccErpId, RBU As String = dtQuoteMaster.AccOfficeName, ORG_ID As String = dtQuoteMaster.org, Currency As String = dtQuoteMaster.currency
        Dim isFranchiser As Boolean

        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AFC) = Role.CurrDocReg Then
            isFranchiser = True
        End If

        If isFranchiser Then
            COMPANY_ID = Franchise.getSampleERPidForFranchise(dtQuoteMaster.CreatedBy)
        End If

        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AEU) = Role.CurrDocReg Then
            ITP = SAPDAL.SAPDAL.getItp(ORG_ID, PartNo, Currency, COMPANY_ID, SAPDAL.SAPDAL.itpType.EU)
        End If

        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AUS) = Role.CurrDocReg Then
            ITP = GetAUSCost(PartNo, "USH1", QuoteID)
        End If

        If (Role.CurrDocReg And COMM.Fixer.eDocReg.ABR) = Role.CurrDocReg Then
            ITP = GetAUSCost(PartNo, "BRH1", QuoteID)

            ITP = FormatNumber(ITP * CType(get_exchangerate("USD", "BRL").ToString, Decimal), 2)

        End If

        'Frank 2016/04/25
        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AJP) = Role.CurrDocReg Then
            ITP = SAPDAL.SAPDAL.getItp(ORG_ID, PartNo, Currency, COMPANY_ID, SAPDAL.SAPDAL.itpType.JP)
        End If

        'Frank 2015/10/14
        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AKR) = Role.CurrDocReg Then
            'ITP = GetAUSCost(PartNo, "KRH1")
            ITP = SAPDAL.SAPDAL.getItp(ORG_ID, PartNo, Currency, COMPANY_ID, SAPDAL.SAPDAL.itpType.KR)
        End If

        If isFranchiser Then
            ITP = UnitPrice
        End If

        Return True

    End Function
    Public Shared Function GetShipToByQuoteID(ByVal QuoteID As String) As String
        Dim ShipTo As String = ""
        Dim QM As Quote_Master = MyUtil.Current.CurrentDataContext.Quote_Masters.Where(Function(p) p.quoteId = QuoteID).FirstOrDefault()
        If QM IsNot Nothing Then
            Dim shiptoObj As Object = tbOPBase.dbExecuteScalar("EQ", String.Format("select top 1 IsNull(ERPID,'') AS  ShipTo from  [dbo].[EQPARTNER] where QUOTEID='{0}' and TYPE ='S'", QuoteID))
            If shiptoObj IsNot Nothing AndAlso Not String.IsNullOrEmpty(shiptoObj) Then
                ShipTo = shiptoObj
            Else
                ShipTo = QM.quoteToErpId
            End If
        End If
        Return ShipTo
    End Function
    Public Shared Function Add2Cart(ByVal quoteId As String, ByVal partNo As String, ByVal QTY As Integer, ByVal ewFlag As Integer,
                               ByVal HigherLevel As Integer, ByVal itemType As COMM.Fixer.eItemType, ByVal category As String, ByVal isSyncPrice As Integer,
                               ByVal isSyncATP As Integer, ByVal ReqDate As Date, ByVal ParentLineNo As Integer, ByVal plantID As String,
                               ByRef LineNo As Integer, ByRef strErrorMessage As String, ByVal Role As IBUS.iRole, ByVal oType As COMM.Fixer.eDocType) As Boolean

        '!!!!!!!!Please do not use session in this function!!!!!!!!!!!!!
        Dim aptQuoteDetail As New EQDSTableAdapters.QuotationDetailTableAdapter
        Dim dtQuoteMaster As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(quoteId, oType)
        If IsNothing(dtQuoteMaster) Then
            strErrorMessage = "Invalid Quote Id!" : Return False
        End If




        Dim SoldTo As String = dtQuoteMaster.AccErpId, RBU As String = dtQuoteMaster.AccOfficeName, ORG_ID As String = dtQuoteMaster.org, Currency As String = dtQuoteMaster.currency


        'Ming 20150602 目前只针对美国将ShipTo传入Bapi
        Dim ShipTo As String = GetShipToByQuoteID(quoteId)
        Dim divPlant As String = ""
        partNo = partNo.ToUpper()
        If isSyncATP = 1 Then
            Dim strStatusCode As String = "", strStatusDesc As String = "", decATP As Decimal = 0
            If Business.isInvalidPhaseOutV2(partNo, ORG_ID, strStatusCode, strStatusDesc, decATP) Then
                If String.IsNullOrEmpty(strStatusCode) AndAlso String.IsNullOrEmpty(strStatusDesc) Then
                    strErrorMessage = partNo & " cannot be found" : Return False
                End If
                strErrorMessage = String.Format("Product status of {0} is {1} ({2}),<br/> PLM Notice:{3}", partNo, strStatusCode, strStatusDesc, Business.getPLMNote(partNo, ORG_ID))
                Select Case strStatusCode
                    Case "O", "S"
                        strErrorMessage += " and has no inventory" : Return False
                    Case "I", "M1", "O1", "S1", "S2", "S5", "T", "V"
                        Return False
                    Case Else
                        Return False
                End Select

            End If
        End If

        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AUS) = Role.CurrDocReg AndAlso Not String.IsNullOrEmpty(plantID) Then divPlant = plantID
        Dim pf As IBUS.iProdF = Pivot.FactProd
        Dim p As IBUS.iProd = pf.getProdByPartNo(partNo, ORG_ID, divPlant)
        If p.type = COMM.Fixer.eProdType.Dummy Then
            Return True
        End If
        If p.type = COMM.Fixer.eProdType.ItemBTO AndAlso itemType <> COMM.Fixer.eItemType.Parent Then
            strErrorMessage = "BTO Item cannot be added as component." : Return False
        End If

        'Frank 20161011
        If ORG_ID = "EU10" AndAlso HigherLevel = COMM.Fixer.StartLine Then
            Dim _parts As New List(Of String)
            _parts.Add(partNo)
            Dim InvalidList As List(Of String) = Advantech.Myadvantech.Business.PartBusinessLogic.isMSSWParts(_parts, ORG_ID)
            If InvalidList IsNot Nothing AndAlso InvalidList.Count > 0 Then
                'strErrorMessage = "Invalid Quote Id!" : Return False
                strErrorMessage = "Invalid Parts : " & String.Join(", ", InvalidList.ToArray()) + ", MS Software items can only be added under a BTOS/CTOS."
                Return False
            End If
        End If


        'If ORG_ID = "EU10" AndAlso HigherLevel = COMM.Fixer.StartLine AndAlso p.type = COMM.Fixer.eProdType.HardDrive Then
        '    strErrorMessage = "Hard drive cannot be placed as component order" : Return False
        'End If



        '!!!!!!!!Please do not use session in this function!!!!!!!!!!!!!
        'Frank 2012/09/25:If quoteid starts with "AUSQ"(ANA user), then replace delivery_plant by input parameter Plant
        'If quoteId.StartsWith("AUSQ", StringComparison.InvariantCultureIgnoreCase) AndAlso Not String.IsNullOrEmpty(plantID) Then delivery_plant = plantID

        Dim req_Date As Date = Now.Date.ToShortDateString
        Dim unitPrice As Decimal = 0
        Dim listPrice As Decimal = 0
        Dim itp As Decimal = 0
        Dim RecyclingFee As Decimal = 0

        QTY = CInt(QTY)
        If IsDate(ReqDate) AndAlso ReqDate <> #12:00:00 AM# Then
            req_Date = ReqDate
        End If

        Dim isFranchiser As Boolean
        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AFC) = Role.CurrDocReg Then
            isFranchiser = True
        End If
        If isFranchiser Then
            SoldTo = Franchise.getSampleERPidForFranchise(dtQuoteMaster.CreatedBy)
        End If
        If isSyncPrice = 1 And itemType <> COMM.Fixer.eItemType.Parent Then
            Dim dtPrice As New DataTable
            Business.GetPriceBiz(SoldTo, ShipTo, ORG_ID, Currency, "", RBU, partNo, dtQuoteMaster.CreatedBy, dtPrice, Role.CurrDocReg)
            If Not IsNothing(dtPrice) AndAlso dtPrice.Rows.Count > 0 Then
                unitPrice = dtPrice.Rows(0).Item("Netwr")
                listPrice = dtPrice.Rows(0).Item("Kzwi1")
                If String.Equals(ORG_ID, "US01") Then
                    Dim _RecycleFee As Decimal = 0
                    If Not IsDBNull(dtPrice.Rows(0).Item("RECYCLE_FEE")) AndAlso Decimal.TryParse(dtPrice.Rows(0).Item("RECYCLE_FEE").ToString(), _RecycleFee) Then
                        RecyclingFee = _RecycleFee
                    End If
                    If RecyclingFee > 0 Then
                        unitPrice = unitPrice - RecyclingFee
                        listPrice = listPrice - RecyclingFee
                    End If
                End If
                '20150529 Ming copy TC'code: When org=US01 get recycling fee from eQuotation.dbo.SAP_PRODUCT_PRICE_COND, which is updated daily by MyLocal SSIS
                'If String.Equals(ORG_ID, "US01") Then
                '    If dtPrice.Rows(0).Item("RECYCLE_FEE") IsNot Nothing AndAlso Decimal.TryParse(dtPrice.Rows(0).Item("RECYCLE_FEE").ToString(), 0) Then
                '        RecyclingFee = dtPrice.Rows(0).Item("RECYCLE_FEE")
                '    End If
                '    If RecyclingFee = 0 Then
                '        Dim sqlRecycFee As String = _
                '            " select PART_NO, cast(CONDITION_AMOUNT_PERCENT/CONDITION_PRICE_UNIT as numeric(10,2)) as COND_VALUE " + _
                '            " from eQuotation.dbo.SAP_PRODUCT_PRICE_COND  " + _
                '            " where ORG_ID='US01' and SD_DOC_CURRENCY='USD' and CONDITION_TYPE='ZHB0' and '" + Now.ToString("yyyyMMdd") + "'  " + _
                '            " between VALID_FROM_DATE and VALID_TO_DATE  " + _
                '            " and PART_NO ='" + partNo + "'" + _
                '            " order by PART_NO  "
                '        Dim dtRecycFee As New DataTable
                '        Dim aptRecycFee As New SqlClient.SqlDataAdapter(sqlRecycFee, ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
                '        aptRecycFee.Fill(dtRecycFee)
                '        aptRecycFee.SelectCommand.Connection.Close()
                '        Dim rs() As DataRow = dtRecycFee.Select("PART_NO='" + partNo + "'")
                '        If rs.Length > 0 Then
                '            If rs(0).Item("COND_VALUE") IsNot Nothing AndAlso Decimal.TryParse(rs(0).Item("COND_VALUE"), 0) Then
                '                RecyclingFee = rs(0).Item("COND_VALUE")
                '            End If
                '        End If
                '    End If
                'End If

            End If
        End If
        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AJP) = Role.CurrDocReg And itemType <> COMM.Fixer.eItemType.Parent Then
            itp = SAPDAL.SAPDAL.getItp(ORG_ID, partNo, Currency, SoldTo, SAPDAL.SAPDAL.itpType.JP)
            If Currency <> "JPY" Then
                itp = FormatNumber(itp * CType(get_exchangerate("JPY", Currency).ToString, Decimal), 2)
            End If
        End If
        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AEU) = Role.CurrDocReg And itemType <> COMM.Fixer.eItemType.Parent Then
            itp = SAPDAL.SAPDAL.getItp(ORG_ID, partNo, Currency, SoldTo, SAPDAL.SAPDAL.itpType.EU)
        End If

        'Frank 2014/12/29 use standard price of SAP accounting1 view to be the part's ITP
        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AUS) = Role.CurrDocReg And itemType <> COMM.Fixer.eItemType.Parent Then
            itp = GetAUSCost(partNo, "USH1", quoteId)
        End If


        If isFranchiser Then
            itp = unitPrice
        End If

        If (Role.CurrDocReg And COMM.Fixer.eDocReg.AUS) = Role.CurrDocReg Then
            If ewFlag <> 0 AndAlso Business.isWarrantable(partNo, Role) Then
            Else
                ewFlag = 0
            End If
        End If
        Dim cF As iCartF = Pivot.FactCart
        Dim CART As iCart(Of iCartLine) = Nothing
        CART = cF.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, quoteId, ORG_ID)
        Dim CARTLINE As iCartLine = Pivot.NewLineCart(quoteId, 0, HigherLevel, p, unitPrice, listPrice, itp, QTY, ewFlag, req_Date, itemType, category)

        'Frank 2015/02/09
        'Virtual Part No.預設帶出Part No.
        If (Role.CurrDocReg And COMM.Fixer.eDocReg.ATW) = Role.CurrDocReg Then
            CARTLINE.VirtualPartNo.Value = partNo
        End If
        CARTLINE.RecyclingFee.Value = RecyclingFee
        LineNo = CART.Add(CARTLINE, oType)
        strErrorMessage = strErrorMessage & COMM.errMsg.getErrMsg(CART.errCode)
        'Business.RefreshPartInventory(quoteId, LineNo, COMM.Fixer.eDocType.EQ)
        MyQuoteX.RefreshPartInventory(quoteId, LineNo)
        'If HigherLevel > COMM.Fixer.StartLine Then Business.RefreshBTOSMainitemAvailableDate(quoteId)
        Return True
    End Function
    Shared Function ADD2QUOTE_V2_1(ByVal quoteId As String, ByVal partNo As String, ByVal QTY As Integer, ByVal ewFlag As Integer,
                             ByVal itemType As COMM.Fixer.eItemType, ByVal category As String, ByVal isSyncPrice As Integer,
                             ByVal isSyncATP As Integer, ByVal ReqDate As Date, ByVal ParentLineNo As Integer, ByVal plantID As String,
                             ByRef LineNo As Integer, ByRef strErrorMessage As String, ByVal Prof As IBUS.iRole, ByVal oType As COMM.Fixer.eDocType) As Boolean

        '!!!!!!!!Please do not use session in this function!!!!!!!!!!!!!
        Return Add2Cart(quoteId, partNo, QTY, ewFlag, ParentLineNo, itemType, category, isSyncPrice, isSyncATP, ReqDate, ParentLineNo, plantID, LineNo, strErrorMessage, Prof, oType)
    End Function
    Shared Function GetPriceBiz(ByVal SholdTo As String, ByVal ShipTo As String, ByVal Org_Id As String, ByVal Currency As String _
                                , ByVal QuoteType As String, ByVal RBU As String, ByVal sPartNo As String, ByVal createdBy As String, ByRef dtPrice As DataTable, ByVal DocReg As COMM.Fixer.eDocReg, Optional ByRef errmsg As String = "") As Boolean
        Dim _SAPDAL As New SAPDAL.SAPDAL
        If String.IsNullOrEmpty(SholdTo) Then

            'If Role.IsEUSales OrElse Role.IsJPAonlineSales OrElse Role.IsTWAonlineSales() OrElse Role.IsCNAonlineSales() Then
            '    dtPrice = getListPriceByTable(sPartNo, Org_Id, Currency)
            'Else
            '    'If Role.IsANASales(dtQuoteMaster(0).createdBy) Then
            '    If Role.IsUsaUser Then
            '        'SAPTools.getSAPPriceByTable(sPartNo, "US01", "UEPP5001", "UEPP5001", Currency, dtPrice, errmsg)
            '        _SAPDAL.getSAPPriceByTable(sPartNo, "US01", "UEPP5001", "UEPP5001", Currency, dtPrice, errmsg)
            '    End If
            'End If
            If Role.IsUsaUser Then
                _SAPDAL.getSAPPriceByTable(sPartNo, "US01", "UEPP5001", "UEPP5001", Currency, "", dtPrice, errmsg)
            ElseIf Role.IsKRAonlineSales Then
                _SAPDAL.getSAPPriceByTable(sPartNo, Org_Id, "AKRC00485", "AKRC00485", Currency, "", dtPrice, errmsg)
            Else
                dtPrice = getListPriceByTable(sPartNo, Org_Id, Currency)
            End If


            If IsNothing(dtPrice) OrElse dtPrice.Rows.Count = 0 Then
                SAPTools.getEpricerPrice(sPartNo, "", "", RBU, "", "", Currency, Org_Id, dtPrice)
                'Frank 20140331: Line 1125 will use the column MATNR but it doesn's exist in the dtPrice that returns by getEpricerPrice
                ', therefore I rename the column PART_NO to MATNR for preventing the system throws the "System.ArgumentException: Column 'MATNR' does not belong to table at line 1125"
                If dtPrice IsNot Nothing Then dtPrice.Columns("PART_NO").ColumnName = "MATNR"
            End If

            If Not IsNothing(dtPrice) Then
                For Each r As DataRow In dtPrice.Rows
                    r.Item("Kzwi1") = FormatNumber(r.Item("Kzwi1"), 2).Replace(",", "")
                    r.Item("Netwr") = r.Item("Kzwi1")
                Next
            End If
        Else
            'Quote-to account is with ERPID
            'If Role.IsUsaUser Then
            '    company_Id = "UEPP5001"
            'End If
            'SAPTools.getSAPPriceByTable(sPartNo, Org_Id, SholdTo, ShipTo, Currency, dtPrice, errmsg)

            Dim _ABROrderType As String = ""

            'If Role.IsABRSales Then
            '    Dim _QME As Quote_Master_Extension = MyQuoteX.GetMasterExtension()
            'End If

            _SAPDAL.getSAPPriceByTable(sPartNo, Org_Id, SholdTo, ShipTo, Currency, QuoteType, dtPrice, errmsg)
            If dtPrice.Rows.Count > 0 Then
                For Each r As DataRow In dtPrice.Rows
                    r.Item("Kzwi1") = FormatNumber(r.Item("Kzwi1"), 2).Replace(",", "")
                    r.Item("Netwr") = FormatNumber(r.Item("Netwr"), 2).Replace(",", "")
                Next
            End If
        End If
        If dtPrice.Rows.Count > 0 Then

            Dim _ListPrice As Decimal = 0, _UnitPrice As Decimal = 0
            For Each r As DataRow In dtPrice.Rows
                'Frank 2013/02/18:Can not compare unit price and list price in string format because string length is not the same.
                'Transfer price format to decimal
                If IsNumeric(r.Item("Netwr")) AndAlso IsNumeric(r.Item("Kzwi1")) Then
                    _UnitPrice = Decimal.Parse(r.Item("Netwr")) : _ListPrice = Decimal.Parse(r.Item("Kzwi1"))
                    If _ListPrice < _UnitPrice Then
                        r.Item("Kzwi1") = r.Item("Netwr")
                    End If
                End If
                'If r.Item("Kzwi1") < r.Item("Netwr") Then
                '    r.Item("Kzwi1") = r.Item("Netwr")
                'End If

            Next
        End If
        'If Role.IsUsaUser AndAlso Role.IsANAAOnlineTeamsByOfficeCode() Then
        If Role.IsAonlineUsa Then
            'Dim ws As New Relics.SAPDAL
            If Not IsNothing(dtPrice) AndAlso dtPrice.Rows.Count > 0 Then
                For i As Integer = 0 To dtPrice.Rows.Count - 1
                    Dim gpPrice As Decimal = 0
                    Dim upricetemp As Decimal = 0
                    If IsNumeric(dtPrice.Rows(i).Item("Netwr")) Then
                        upricetemp = dtPrice.Rows(i).Item("Netwr")
                    End If
                    'Relics.CommonLogic.isANAPnBelowGP(dtPrice.Rows(i).Item("MATNR"), upricetemp, gpPrice, errmsg) 'ws.getPriceByProdLine(dtPrice.Rows(i).Item("MATNR"), ws.getProdPricingGrp(dtPrice.Rows(i).Item("MATNR")), errmsg)
                    'Dim _sapdal As New SAPDAL.SAPDAL
                    '_sapdal.isNeedANAGPControl( )
                    'Relics.CommonLogic.isANAPnBelowGP(dtPrice.Rows(i).Item("MATNR"), upricetemp, gpPrice, errmsg)

                    isANAPnBelowGPV2(SholdTo, dtPrice.Rows(i).Item("MATNR"), upricetemp, gpPrice, errmsg)

                    If gpPrice <> 0 AndAlso gpPrice > dtPrice.Rows(i).Item("Netwr") Then
                        dtPrice.Rows(i).Item("Netwr") = gpPrice
                    End If
                    dtPrice.AcceptChanges()
                Next
            End If
        End If
        Return True
    End Function




    Shared Function IseStoreCBom(ByVal DisplayPartno As String) As Boolean
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)

        Dim storeid As String = "AUS"



        If Role.IsJPAonlineSales() Then storeid = "AJP"

        If Role.IsUsaUser() Then storeid = "AUS"

        Dim cmd As New SqlClient.SqlCommand(" Select COUNT(*) from ESTORE_BTOS_CATEGORY where DisplayPartno=@PN and StoreID=@storeid ", conn)
        With cmd.Parameters
            .AddWithValue("PN", DisplayPartno) : .AddWithValue("storeid", storeid)
        End With
        ' Try
        conn.Open()

        Dim myReader As SqlClient.SqlDataReader = cmd.ExecuteReader()

        Dim _dt As New DataTable
        _dt.Load(myReader)
        Dim _count As Integer = _dt.Rows(0).Item(0)
        conn.Close()

        If _count > 0 Then Return True

        Return False

    End Function





    Shared Function get_exchangerate(ByVal C_FROM As String, ByVal C_TO As String) As String
        If C_FROM = C_TO Then
            Return 1
        End If
        Dim temp As Object = Nothing
        temp = tbOPBase.dbExecuteScalar("b2b", "select top 1 UKURS from SAP_EXCHANGERATE" &
                                                     " where fCURR='" & C_FROM & "' and TCURR='" & C_TO & "' order by exch_date desc")

        If temp IsNot Nothing AndAlso temp.ToString <> "" Then
            Return temp
        End If
        Return "0.0"
    End Function
    Shared Function getEWItemByMonth(ByVal month As Integer, ByVal DocReg As COMM.Fixer.eDocReg) As String
        Return Pivot.NewObjEWUtil.getEWItemByMonth(month, DocReg)

    End Function

    Shared Function getRateByEWItem(ByVal itemNo As String, ByVal DocReg As COMM.Fixer.eDocReg) As Double
        Return Pivot.NewObjEWUtil.getRateByEWItem(itemNo, DocReg)

    End Function

    Shared Function isWarrantable(ByVal PN As String, ByVal r As IBUS.iRole) As [Boolean]
        Return Pivot.FactProd.getProdByPartNo(PN, r.getCurrOrg).isWarrantable(PN, r.CurrDocReg)
    End Function





    Shared Function getMonthByEWItem(ByVal itemNo As String) As Double
        Select Case itemNo.ToUpper.Trim()
            Case "AGS-EW-03"
                Return 3
            Case "AGS-EW-06"
                Return 6
            Case "AGS-EW-09"
                Return 9
            Case "AGS-EW-12"
                Return 12
            Case "AGS-EW-15"
                Return 15
            Case "AGS-EW-21"
                Return 21
            Case "AGS-EW-24"
                Return 24
            Case "AGS-EW-36"
                Return 36
            Case "AGS-EW-AD"
                Return 99
            Case "AGS-EW/DOA-03"
                Return 999
        End Select
        Return 0
    End Function
    Shared Function isANA(ByVal RBU As String) As Boolean
        If RBU = "ANADMF" Or RBU = "AAC" Or RBU = "AACIAG" Or RBU = "AENC" Or RBU = "ANA" Then
            Return True
        End If
        Return False
    End Function
    Shared Function getSiebelPayTerms() As DataTable
        Dim mySiebelPayterms As New SIEBEL_PAYTERMS("B2B", "SIEBEL_PAYTERMS")
        Dim Dt As DataTable = mySiebelPayterms.GetDTbySelectStr(String.Format("select name from {0} order by NAME", mySiebelPayterms.tb))
        Return Dt
    End Function

    Shared Function getSiebelShipTerms() As DataTable
        Dim mySiebelShipTerms As New SIEBEL_SHIPTERMS("B2B", "SIEBEL_SHIPTERMS")
        Dim Dt As DataTable = mySiebelShipTerms.GetDTbySelectStr(String.Format("select name from {0} order by NAME", mySiebelShipTerms.tb))
        Return Dt
    End Function

    Shared Function FormatTel(ByVal str As String) As String
        If str.Trim = "" Then
            Return str
        End If
        Dim c As Char() = str.ToCharArray
        str = ""
        For i As Integer = 0 To c.Length - 1
            If IsNumeric(c(i)) Or c(i) = "+" Or c(i) = "#" Or c(i) = " " Then
                str &= c(i)
            Else
                Exit For
            End If
        Next
        Return str
    End Function

    Shared Function transQuote2Siebel(ByVal UID As String) As Boolean

        Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
        Dim dt As EQDS.optyQuoteDataTable = myOptyQuote.GetOptyQuoteByOuoteID(UID)
        Dim optyId As String = "", optyName As String = "", optyStage As String = "", siebelQuoteId As String = ""

        If dt.Rows.Count > 0 Then
            Dim _row As EQDS.optyQuoteRow = dt.Rows(0)
            optyId = _row.optyId : optyName = _row.optyName : optyStage = _row.optyStage
        End If

        If optyId.ToUpper = "NEW ID" Then
            'Nada revised.................................
            ' Dim MyDC As New MyEQDataContext
            ' Dim optyQuote1 As optyQuote = MyDC.optyQuotes.Where(Function(p) p.quoteId = UID).FirstOrDefault()
            Dim M As IBUS.iDocHeader = Pivot.NewObjDocHeader
            Dim _dtQM As IBUS.iDocHeaderLine = M.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
            Dim optyQuote As IBUS.iOptyQuote = Pivot.NewObjOptyQuote()
            Dim optyQuote1 As IBUS.iOptyQuoteLine = optyQuote.GetListByQuoteID(UID).FirstOrDefault
            '/Nada revised.................................
            'Frank 2012/08/28 Setting optyName to QuoteID,optyStage to 25% if they are empty
            If String.IsNullOrEmpty(optyName) Then
                Dim QuoteDt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
                optyName = QuoteDt.quoteNo
                If Not IsNothing(QuoteDt.Revision_Number) AndAlso QuoteDt.Revision_Number > 0 Then
                    optyName = optyName & " V" & QuoteDt.Revision_Number
                End If
            End If

            If String.IsNullOrEmpty(optyStage) Then optyStage = "25% Proposing/Quoting"

            'Frank 2012/07/14 Creating opportunity with opportunity-Stage by CreateOptyByQuoteId_V2
            'optyId = SiebelTools.CreateOptyByQuoteId(UID, optyName)
            'Ming 20150401 call IC's API,create opty
            'Ming 20150423 get Owner by email(Copy Frank API)
            Dim strowner As String = String.Empty
            Dim salesemail As String = _dtQM.salesEmail, creatoremail = _dtQM.CreatedBy, siebelAccountRowId = _dtQM.AccRowId
            Dim OptyOwnerEmail As String = optyQuote1.Opty_Owner_Email
            'ICC 2015/6/9 To prevent someone's eQuotation ID is not equal to SIEBEL ID, we get strowner by Frank's three rules directly.
            'If (Not IsNothing(OptyOwnerEmail)) AndAlso (Not String.IsNullOrEmpty(OptyOwnerEmail.Trim)) Then
            'strowner = OptyOwnerEmail
            'Else

            'ICC 2015/6/24 Check QuotationMaster org column first. Make sure it is not null or empty
            Dim org As String = String.Empty
            If Not _dtQM.org Is Nothing AndAlso Not String.IsNullOrEmpty(_dtQM.org) AndAlso _dtQM.org.Trim().Length >= 2 Then
                org = _dtQM.org.Trim().Substring(0, 2)
            End If
            'ICC 2015/6/24 For TW and CN quotes do not have to comply with US rule. It only have to use OptyOwnerEmail.
            If org.Equals("TW", StringComparison.OrdinalIgnoreCase) OrElse org.Equals("CN", StringComparison.OrdinalIgnoreCase) Then
                strowner = OptyOwnerEmail
            ElseIf org.Equals("KR", StringComparison.OrdinalIgnoreCase) Then
                strowner = creatoremail

                If Not String.IsNullOrEmpty(salesemail) Then
                    strowner = salesemail
                End If

            Else
                'ICC 2015/6/24 For US & EU quotes still use rule 1.
                'Frank 2012/08/29:Below 3 step was confirmed with Jay
                '1. check owner email by Prepared For:(Internal Sales) <--QuotationMaster.salesEmail
                If Not String.IsNullOrEmpty(salesemail) Then
                    strowner = SiebelTools.TransferOpportunityOwnerEmail(salesemail)
                End If
            End If

            'ICC 2015/6/24 將原本的3條規則，拆分成第一條給US & EU使用，後面兩條給所有quote使用
            '2. check owner email by quotation creator  <--QuotationMaster.createdBy
            If String.IsNullOrEmpty(strowner) AndAlso (Not String.IsNullOrEmpty(creatoremail)) Then
                strowner = SiebelTools.TransferOpportunityOwnerEmail(creatoremail)
            End If
            '3. check owner email by siebel account primary sales email <--QuotationMaster.quoteToRowId
            If String.IsNullOrEmpty(strowner) AndAlso (Not String.IsNullOrEmpty(siebelAccountRowId)) Then
                strowner = SiebelTools.getPriSalesEmailByAccountROWID(siebelAccountRowId)
            End If
            ''4. If PriSalesEmailByAccountROWID is still empty, then just let strowner to be empty
            'If String.IsNullOrEmpty(strowner) Then
            '    '? TBD
            'End If
            'End If
            Dim activeopty As SiebelActive = New SiebelActive(UID, optyName, optyStage, HttpContext.Current.User.Identity.Name, strowner)
            Dim retopty As Boolean = SiebelBusinessLogic.CreateOpportunityCommand(activeopty)
            If Not retopty Then Util.InsertMyErrLog(activeopty.FailedLog, Util.GetCurrentUserID())

            'Try
            '    optyId = SiebelTools.CreateOptyByQuoteId_V2(UID, optyName, optyStage, COMM.Fixer.eDocType.EQ, optyQuote1.Opty_Owner_Email)
            'Catch ex As Exception
            '    Dim errormsg As String = ex.ToString + vbCrLf + "Business..vb:line:1325"
            '    Util.InsertMyErrLog(errormsg, Util.GetCurrentUserID())
            'End Try

            'Frank 2012/08/24 We assume that creating new oppurtunity failed because of return value optyid is null
            'If String.IsNullOrEmpty(optyId) Then
            '    optyId = "NEW ID"
            'End If
            'Frank 2012/07/16
            'myOptyQuote.Update(String.Format("quoteId='{0}'", UID), String.Format("optyId='{0}'", optyId))
            'myOptyQuote.UpdateOptyIdByQouteID(optyId, UID)
            'myOptyQuote.UpdateOptyByQuoteID(optyId, optyName, optyStage, UID)
            'optyQuote1.optyId = optyId
            'optyQuote1.optyName = optyName
            'optyQuote1.quoteId = UID
            'optyQuote1.optyStage = optyStage
            ''Nada revised.................................
            'optyQuote.DeleteByQuoteID(UID) : optyQuote.Add(optyQuote1)
            'MyDC.SubmitChanges()
            '/Nada revised.................................
        End If
        'Ming 20150401 call IC's API, create sibele Quote
        Dim activequote As SiebelActive = New SiebelActive(UID, HttpContext.Current.User.Identity.Name)
        'Frank 20150414 If new quote associates with an existing opty, then specify the opty row id to the one
        If Not String.IsNullOrEmpty(optyId) _
            AndAlso Not optyId.Equals("NULL", StringComparison.InvariantCultureIgnoreCase) _
            AndAlso Not optyId.Equals("NEW ID", StringComparison.InvariantCultureIgnoreCase) Then
            activequote.OptyID = optyId
        End If
        Dim retquote As Boolean = SiebelBusinessLogic.CreateQuoteCommand(activequote)
        If Not retquote Then Util.InsertMyErrLog(activequote.FailedLog, Util.GetCurrentUserID())
        'Dim myQuoteSiebelQuote As New quoteSiebelQuote("EQ", "quoteSiebelQuote")
        'myQuoteSiebelQuote.Delete(String.Format("quoteId='{0}'", UID))
        'If Not optyId.Equals("NEW ID", StringComparison.InvariantCultureIgnoreCase) Then
        '    siebelQuoteId = SiebelTools.CreateQuotationbyQuoteid(UID, optyId, COMM.Fixer.eDocType.EQ)
        '    If Not String.IsNullOrEmpty(siebelQuoteId) Then
        '        myQuoteSiebelQuote.Add(UID, siebelQuoteId)
        '    End If
        'End If
        Pivot.NewObjDocHeader.ChangeDocStatus(UID, CInt(COMM.Fixer.eDocStatus.QFINISH), COMM.Fixer.eDocType.EQ)
        Return (True)
    End Function
    Shared Function IsAccountOwner(ByVal SalesUserid As String, ByVal AccountRowID As String, ByVal ErpID As String) As Boolean
        Dim sb As New StringBuilder
        sb.AppendFormat("SELECT DISTINCT ACCOUNT_ROW_ID FROM SIEBEL_ACCOUNT_OWNER_EMAIL AS z WHERE (ACCOUNT_ROW_ID IS NOT NULL) AND ACCOUNT_ROW_ID ='{0}' AND (EMAIL_ADDRESS = '{1}') ", AccountRowID, SalesUserid)
        sb.Append(" UNION  ")
        sb.AppendFormat(" SELECT DISTINCT COMPANY_ID FROM SAP_COMPANY_OWNER_EMAIL AS z WHERE (COMPANY_ID IS NOT NULL) AND COMPANY_ID='{0}' AND (email = '{1}')  ", ErpID, SalesUserid)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", sb.ToString())
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Shared Function isEidtableQuote(ByVal UID As String, Optional ByRef QuoteDt As IBUS.iDocHeaderLine = Nothing) As Boolean
        If IsNothing(QuoteDt) Then
            QuoteDt = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        End If
        Select Case UCase(QuoteDt.org)
            Case "EU10"
                'If String.Equals(Pivot.CurrentProfile.UserId, "chiara.geroli@advantech.it", StringComparison.CurrentCultureIgnoreCase) OrElse
                '            String.Equals(Pivot.CurrentProfile.UserId, "emilie.azevdeo@advantech.fr", StringComparison.CurrentCultureIgnoreCase) Then
                '    If QuoteDt.qStatus = CInt(COMM.Fixer.eDocStatus.QDRAFT) AndAlso String.Equals(QuoteDt.CreatedBy, "agnes.iglesias@advantech.fr") Then
                '        Return True
                '    End If
                'End If
                If QuoteDt.qStatus = CInt(COMM.Fixer.eDocStatus.QDRAFT) AndAlso
                    String.Equals(QuoteDt.CreatedBy, Pivot.CurrentProfile.UserId, StringComparison.CurrentCultureIgnoreCase) Then
                    'If Not GPControl.isInApproval(UID) Then
                    Return True
                    'Else
                    'Return True
                    ' End If
                End If
                If QuoteDt.qStatus = CInt(COMM.Fixer.eDocStatus.QDRAFT) Then
                    If IsAccountOwner(Pivot.CurrentProfile.UserId, QuoteDt.AccRowId, QuoteDt.AccErpId) Then Return True
                End If
            Case "US01"
                If Role.IsUsaUser Then
                    Dim a As IBUS.iRole = New PROF.Prof
                    If Not IsNothing(QuoteDt.CreatedBy) AndAlso Role.IsUsaUser(QuoteDt.CreatedBy) Then
                        Return True
                    End If
                    If Not IsNothing(QuoteDt.siebelRBU) AndAlso QuoteDt.siebelRBU.Equals("ANADMF", StringComparison.InvariantCultureIgnoreCase) Then
                        Return True
                    End If
                End If
            Case Else
                Return True
        End Select

        Return False
    End Function

    Shared Function isEidtableQuote(ByVal UID As String, ByVal org As String, ByVal qstatus As String, ByVal CreatedBy As String) As Boolean

        If String.IsNullOrEmpty(UID) Or String.IsNullOrEmpty(org) Or String.IsNullOrEmpty(qstatus) Or String.IsNullOrEmpty(CreatedBy) Then
            Return False
        Else
            Select Case UCase(org)
                Case "EU10"

                    'If qstatus = CInt(COMM.Fixer.eDocStatus.QDRAFT) AndAlso String.Equals(CreatedBy, "agnes.iglesias@advantech.fr") Then
                    '    If String.Equals(Pivot.CurrentProfile.UserId, "chiara.geroli@advantech.it", StringComparison.CurrentCultureIgnoreCase) OrElse
                    '                String.Equals(Pivot.CurrentProfile.UserId, "emilie.azevdeo@advantech.fr", StringComparison.CurrentCultureIgnoreCase) Then
                    '        Return True
                    '    End If
                    'End If

                    If qstatus = CInt(COMM.Fixer.eDocStatus.QDRAFT) AndAlso
                        String.Equals(CreatedBy, Pivot.CurrentProfile.UserId, StringComparison.CurrentCultureIgnoreCase) Then
                        Return True
                        'If Not GPControl.isInApproval(UID) Then
                        '    Return True
                        'Else
                        '    Return True
                        'End If
                    End If
                    If qstatus = CInt(COMM.Fixer.eDocStatus.QDRAFT) Then
                        Dim QM As Quote_Master = MyQuoteX.GetQuoteMaster(UID)
                        If QM IsNot Nothing Then
                            If IsAccountOwner(Pivot.CurrentProfile.UserId, QM.quoteToRowId, QM.quoteToErpId) Then Return True
                        End If
                    End If
                Case "US01"
                    If Role.IsUsaUser() Then
                        Return True
                    Else
                        Return False
                    End If
                Case "TW01"
                    If Role.IsTWAonlineSales OrElse Role.IsHQDCSales Then
                        Return True
                    Else
                        Return False
                    End If
                Case Else
                    Return True
            End Select
        End If
        Return False
    End Function
    Shared Function getProductStatus(ByVal partNo As String, ByVal org_id As String) As String
        Dim str As String = String.Format("select top 1 * from sap_product_status where PART_NO='{0}' and sales_org='{1}'", partNo, org_id)

        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("B2B", str)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("product_status").ToString
        End If
        Return ""
    End Function
    Shared Function isValidQuote(ByVal UID As String, ByVal oType As COMM.Fixer.eDocType, Optional ByRef DT As DataTable = Nothing) As Boolean
        Dim Doc As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, oType)
        If IsNothing(Doc) Then
            Return False
        End If
        If Doc.CreatedBy.Equals(Pivot.CurrentProfile.UserId, StringComparison.InvariantCultureIgnoreCase) Or
            GPControl.isApprover(Pivot.CurrentProfile.UserId, UID) Or isTeamOwner(UID, Pivot.CurrentProfile.UserId) Then
            Return True
        End If
        'If UID.StartsWith("AUSQ", StringComparison.CurrentCultureIgnoreCase) Then
        If Role.IsUsaUser() OrElse Role.IsTWAonlineSales() OrElse Role.IsKRAonlineSales OrElse Role.IsJPAonlineSales Then
            Return isEidtableQuote(UID)
        End If
        Return False
    End Function
    Shared Function getOrgByOrgID(ByVal ID As String) As String
        Return Left(ID, 2)
    End Function
    Shared Function getPlantByOrgID(ByVal ID As String) As String
        Return Left(ID, 2) & "H1"
    End Function
    Shared Function getCBOMorg(ByVal UID As String, ByVal oType As COMM.Fixer.eDocType) As String
        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, oType)
        If IsNothing(dt) Then
            Return ""
        End If
        If Role.IsFranchiser() Then
            Return "TW"
        End If
        Return getOrgByOrgID(dt.org)
    End Function

    Shared Function getUnitPriceCache(ByVal CompanyId As String, ByVal OrgId As String, ByVal PartNo As String) As Decimal
        Dim objUp As Object = Nothing
        Dim cmd As New SqlClient.SqlCommand(
            "select top 1 UNIT_PRICE from SAP_PRICE_CACHE where COMPANY_ID=@CID and ORG=@ORG and PART_NO=@PN and DATEDIFF(hh,CACHE_DATE,getdate())<=4 order by CACHE_DATE desc",
            New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString))
        cmd.Parameters.AddWithValue("ORG", OrgId) : cmd.Parameters.AddWithValue("CID", CompanyId) : cmd.Parameters.AddWithValue("PN", PartNo)
        cmd.Connection.Open()
        objUp = cmd.ExecuteScalar()
        cmd.Connection.Close()
        If objUp IsNot Nothing Then
            Return DirectCast(objUp, Decimal)
        End If
        Return -1
    End Function
    Shared Function getListPriceByTable(ByVal PartNoStr As String, ByVal org As String, ByVal curr As String) As DataTable
        Dim part_noArr() As String = PartNoStr.Trim().Trim("|").Split("|")
        Dim dt As New DataTable
        'ICC 20170815 Add columns to prevent column error
        dt.Columns.Add("MATNR") : dt.Columns.Add("Kzwi1") : dt.Columns.Add("Netwr") : dt.Columns.Add("RECYCLE_FEE") : dt.Columns.Add("TAX") : dt.Columns.Add("ZMIP") : dt.Columns.Add("VPRS")
        For Each p As String In part_noArr
            Dim l As Decimal = getListPrice(p, org, curr)
            Dim r As DataRow = dt.NewRow
            r.Item("MATNR") = p
            r.Item("Kzwi1") = l
            r.Item("Netwr") = l
            dt.Rows.Add(r)
        Next
        Return dt
    End Function
    'Ming 20150612 
    ''' <summary>
    ''' 附加参数RecyclingFee，用来返回RecyclingFee的值
    ''' </summary>
    ''' <param name="partno"></param>
    ''' <param name="org"></param>
    ''' <param name="CURR"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getListPriceV2(ByVal partno As String, ByVal org As String, ByVal CURR As String, ByRef RecyclingFee As Decimal) As Decimal
        Dim ListPrice As Decimal = 0, objLp As Object = Nothing
        If objLp IsNot Nothing Then
            Return CType(objLp, Decimal)
        Else
            Dim spd As New SAPDAL.SAPDAL
            If org = "US01" Then
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, "UEPP5001", "UEPP5001", "USD", "", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Dim _RecycleFee As Decimal = 0
                    If Not IsDBNull(LpDt.Rows(0).Item("RECYCLE_FEE")) AndAlso Decimal.TryParse(LpDt.Rows(0).Item("RECYCLE_FEE").ToString(), _RecycleFee) Then
                        RecyclingFee = _RecycleFee
                    End If
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If org = "JP01" Then
                Dim LpDt As DataTable = Nothing
                If CURR = "JPY" Then
                    spd.getSAPPriceByTable(partno, org, "JJCBOM", "JJCBOM", CURR, "", LpDt)
                ElseIf CURR = "USD" Then
                    spd.getSAPPriceByTable(partno, org, "UUAASC", "UUAASC", CURR, "", LpDt)
                End If
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If String.Equals(org, "EU10", StringComparison.CurrentCultureIgnoreCase) Then
                Dim dummyERPID As String = "EDDEVI07"
                If String.Equals(CURR, "GBP", StringComparison.CurrentCultureIgnoreCase) Then
                    dummyERPID = "EKGBEC02"
                ElseIf String.Equals(CURR, "USD", StringComparison.CurrentCultureIgnoreCase) Then
                    dummyERPID = "EFESAL01"
                End If
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, dummyERPID, dummyERPID, CURR, "", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If String.Equals(org, "TW01", StringComparison.CurrentCultureIgnoreCase) Then
                Dim dummyERPID As String = "2NC00001"
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, dummyERPID, dummyERPID, CURR, "", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If String.Equals(org, "CN10", StringComparison.CurrentCultureIgnoreCase) Then
                Dim dummyERPID As String = "C100077"
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, dummyERPID, dummyERPID, CURR, "", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
        End If
        Return 0


    End Function
    Shared Function getListPrice(ByVal partno As String, ByVal org As String, ByVal CURR As String) As Decimal

        'Return Relics.CommonLogic.getListPrice(partno, org, CURR)

        Dim ListPrice As Decimal = 0, objLp As Object = Nothing

        If objLp IsNot Nothing Then
            Return CType(objLp, Decimal)
        Else
            Dim spd As New SAPDAL.SAPDAL
            If org = "US01" Then
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, "UEPP5001", "UEPP5001", "USD", "", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If org = "JP01" Then
                Dim LpDt As DataTable = Nothing
                If CURR = "JPY" Then
                    spd.getSAPPriceByTable(partno, org, "JJCBOM", "JJCBOM", CURR, "", LpDt)
                ElseIf CURR = "USD" Then
                    spd.getSAPPriceByTable(partno, org, "UUAASC", "UUAASC", CURR, "", LpDt)
                End If
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If String.Equals(org, "EU10", StringComparison.CurrentCultureIgnoreCase) Then
                Dim dummyERPID As String = "EDDEVI07"
                If String.Equals(CURR, "GBP", StringComparison.CurrentCultureIgnoreCase) Then
                    dummyERPID = "EKGBEC02"
                ElseIf String.Equals(CURR, "USD", StringComparison.CurrentCultureIgnoreCase) Then
                    dummyERPID = "EFESAL01"
                End If
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, dummyERPID, dummyERPID, CURR, "", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If String.Equals(org, "TW01", StringComparison.CurrentCultureIgnoreCase) Then
                'Intercon目前面臨問題：
                '1）使用2NC00001作為默認erpID, 之後將獲取的price用當前匯率轉換成USD
                '2) 找一個默認是USD的erpID 替換2NC00001,減少幣別的換算步驟. -> 20160216 Ryan 當為USD時將ERPID設為AILR001
                Dim dummyERPID As String = "2NC00001"
                If CURR = "USD" Then
                    dummyERPID = "AILR001"
                End If

                'Ryan 20160912 Add default ERPID logic for intercon
                If Role.IsHQDCSales() Then
                    If Role.IsInterconIA() Then
                        dummyERPID = "AILG002"
                    Else
                        dummyERPID = "AILP001"
                    End If
                End If

                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, dummyERPID, dummyERPID, CURR, "", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If String.Equals(org, "CN10", StringComparison.CurrentCultureIgnoreCase) Then
                Dim dummyERPID As String = "C100077"
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, dummyERPID, dummyERPID, CURR, "", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If String.Equals(org, "KR01", StringComparison.CurrentCultureIgnoreCase) Then
                Dim dummyERPID As String = "AKRCE0416"
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, dummyERPID, dummyERPID, "KRW", "", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
        End If
        Return 0


    End Function
    Public Shared Function IsOrderable(ByVal strPartNo As String, ByVal strSAPOrg As String) As Boolean
        Dim strPNs() As String = Split(strPartNo, "|")
        If strPNs.Length = 0 Then Return False
        For Each pn As String In strPNs
            pn = Trim(pn).Replace("'", "''")
            'If String.Equals(pn, "No Need", StringComparison.CurrentCultureIgnoreCase) Then Continue For
            If String.Equals(pn, System.MyExtension.BuildIn, StringComparison.CurrentCultureIgnoreCase) Then Continue For
            Dim objCount As Object = Nothing
            Dim strSql As String = String.Format(
              " select count(part_no) as c " +
              " from SAP_PRODUCT_STATUS_ORDERABLE " +
              " where product_status in " + ConfigurationManager.AppSettings("CanOrderProdStatus") +
              " and part_no =@PN and sales_org=@SAPORG ", pn, strSAPOrg)
            Dim cmd As New SqlClient.SqlCommand(strSql, New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
            cmd.Parameters.AddWithValue("PN", pn) : cmd.Parameters.AddWithValue("SAPORG", strSAPOrg)
            cmd.Connection.Open() : objCount = cmd.ExecuteScalar() : cmd.Connection.Close()
            If objCount Is Nothing OrElse CInt(objCount) = 0 Then
                Return False
            End If
        Next
        'If strPartNo.StartsWith("IPC-610") Then Return False
        Return True
    End Function

    Public Shared Function GetPrice(ByVal PartNo As String, ByVal UID As String) As Decimal
        'If PartNo = "No Need" OrElse PartNo = "None" Then Return 0
        If PartNo.ToLower = System.MyExtension.BuildIn.ToLower OrElse PartNo = "None" Then Return 0
        If PartNo.ToUpper.StartsWith("AGS-EW") Then
            Return Business.getRateByEWItem(PartNo, Pivot.CurrentProfile.CurrDocReg) * 100
        End If
        If Not IsNothing(UID) AndAlso Not String.IsNullOrEmpty(UID) Then
            Dim _SAPDAL As New SAPDAL.SAPDAL
            Dim MasterRef As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
            Dim quote_to_company As String = MasterRef.AccErpId
            Dim unit_price As Decimal = 0
            Dim hdSAPOrg As String = Pivot.CurrentProfile.getCurrOrg
            If String.Equals(hdSAPOrg, "US01") Then
                If String.IsNullOrEmpty(quote_to_company) OrElse String.Equals(quote_to_company, "UEPP5001", StringComparison.CurrentCultureIgnoreCase) Then
                    quote_to_company = "UEPP5001"
                    If Not PartNo.Contains("|") Then
                        unit_price = Business.getListPrice(PartNo, "US01", "USD")
                    Else
                        Dim strPNs() As String = Split(PartNo, "|")
                        For Each strPN As String In strPNs
                            unit_price += Business.getListPrice(strPN, "US01", "USD")
                        Next
                    End If
                Else
                    If Not PartNo.Contains("|") Then
                        unit_price = Business.getUnitPriceCache(quote_to_company, hdSAPOrg, PartNo)
                        If unit_price < 0 Then
                            unit_price = 0
                            Dim dtPrice As New DataTable, org As String = hdSAPOrg
                            ' SAPTools.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", dtPrice)
                            _SAPDAL.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", "", dtPrice)
                            If dtPrice.Rows.Count > 0 Then
                                For Each rPrice As DataRow In dtPrice.Rows
                                    unit_price += rPrice.Item("Netwr")
                                Next
                            End If
                        End If
                    Else
                        Dim dtPrice As New DataTable, org As String = hdSAPOrg
                        'SAPTools.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", dtPrice)
                        _SAPDAL.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", "", dtPrice)
                        If dtPrice.Rows.Count > 0 Then
                            For Each rPrice As DataRow In dtPrice.Rows
                                unit_price += rPrice.Item("Netwr")
                            Next
                        End If
                    End If
                End If
            Else
                If String.IsNullOrEmpty(quote_to_company) Then
                    If Not PartNo.Contains("|") Then
                        unit_price = Business.getListPrice(PartNo, hdSAPOrg, COMM.Util.getCurrSign(MasterRef.currency))
                    Else
                        Dim strPNs() As String = Split(PartNo, "|")
                        For Each strPN As String In strPNs
                            unit_price += Business.getListPrice(strPN, hdSAPOrg, COMM.Util.getCurrSign(MasterRef.currency))
                        Next
                    End If
                Else
                    Dim dtPrice As New DataTable, org As String = hdSAPOrg
                    'SAPTools.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", dtPrice)
                    _SAPDAL.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", "", dtPrice)
                    If dtPrice.Rows.Count > 0 Then
                        For Each rPrice As DataRow In dtPrice.Rows
                            unit_price += rPrice.Item("Netwr")
                        Next
                    End If
                End If

            End If
            If unit_price = 0 Then Return 0
            Return unit_price
        End If
        Return 0
    End Function

    Public Shared Sub GetPriceV2(ByVal PartNo As String, ByVal UID As String, ByVal ORG_ID As String, ByRef ListPrice As Decimal, ByRef UnitPrice As Decimal, Optional ByRef RecyclingFee As Decimal = 0)

        If PartNo.Equals(System.MyExtension.BuildIn, StringComparison.InvariantCultureIgnoreCase) _
            OrElse PartNo.Equals("None", StringComparison.InvariantCultureIgnoreCase) Then
            ListPrice = 0 : UnitPrice = 0 : Exit Sub
        End If

        If PartNo.ToUpper.StartsWith("AGS-EW") Then
            'Return Business.getRateByEWItem(PartNo, Pivot.CurrentProfile.CurrDocReg) * 100
            ListPrice = Business.getRateByEWItem(PartNo, Pivot.CurrentProfile.CurrDocReg) * 100 : UnitPrice = ListPrice : Exit Sub
        End If
        If Not IsNothing(UID) AndAlso Not String.IsNullOrEmpty(UID) Then
            Dim _SAPDAL As New SAPDAL.SAPDAL
            Dim MasterRef As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
            Dim quote_to_company As String = MasterRef.AccErpId

            If String.Equals(ORG_ID, "US01") Then
                If String.IsNullOrEmpty(quote_to_company) OrElse String.Equals(quote_to_company, "UEPP5001", StringComparison.CurrentCultureIgnoreCase) Then
                    quote_to_company = "UEPP5001"
                    If Not PartNo.Contains("|") Then
                        UnitPrice = Business.getListPriceV2(PartNo, "US01", "USD", RecyclingFee)
                    Else
                        Dim strPNs() As String = Split(PartNo, "|")
                        For Each strPN As String In strPNs
                            UnitPrice += Business.getListPriceV2(strPN, "US01", "USD", RecyclingFee)
                        Next
                    End If
                    ListPrice = UnitPrice
                Else
                    If Not PartNo.Contains("|") Then
                        UnitPrice = Business.getUnitPriceCache(quote_to_company, ORG_ID, PartNo)
                        If UnitPrice < 0 Then
                            UnitPrice = 0 : ListPrice = 0 : RecyclingFee = 0
                            Dim dtPrice As New DataTable, org As String = ORG_ID
                            'SAPTools.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", dtPrice)
                            _SAPDAL.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", "", dtPrice)
                            If dtPrice.Rows.Count > 0 Then
                                For Each rPrice As DataRow In dtPrice.Rows
                                    UnitPrice += rPrice.Item("Netwr")
                                    ListPrice += rPrice.Item("Kzwi1")
                                    If String.Equals(org, "US01") Then
                                        Dim _RecycleFee As Decimal = 0
                                        If Not IsDBNull(dtPrice.Rows(0).Item("RECYCLE_FEE")) AndAlso Decimal.TryParse(dtPrice.Rows(0).Item("RECYCLE_FEE").ToString(), _RecycleFee) Then
                                            RecyclingFee = _RecycleFee
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Else
                        Dim dtPrice As New DataTable
                        'SAPTools.getSAPPriceByTable(PartNo, ORG_ID, quote_to_company, quote_to_company, "", dtPrice)
                        _SAPDAL.getSAPPriceByTable(PartNo, ORG_ID, quote_to_company, quote_to_company, "", "", dtPrice)
                        If dtPrice.Rows.Count > 0 Then
                            For Each rPrice As DataRow In dtPrice.Rows
                                UnitPrice += rPrice.Item("Netwr")
                                ListPrice += rPrice.Item("Kzwi1")
                                If String.Equals(ORG_ID, "US01") Then
                                    Dim _RecycleFee As Decimal = 0
                                    If Not IsDBNull(dtPrice.Rows(0).Item("RECYCLE_FEE")) AndAlso Decimal.TryParse(dtPrice.Rows(0).Item("RECYCLE_FEE").ToString(), _RecycleFee) Then
                                        RecyclingFee = _RecycleFee
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
            Else
                If String.IsNullOrEmpty(quote_to_company) Then
                    If Not PartNo.Contains("|") Then
                        UnitPrice = Business.getListPrice(PartNo, ORG_ID, MasterRef.currency)
                    Else
                        Dim strPNs() As String = Split(PartNo, "|")
                        For Each strPN As String In strPNs
                            UnitPrice += Business.getListPrice(strPN, ORG_ID, MasterRef.currency)
                        Next
                    End If
                    ListPrice = UnitPrice
                Else
                    Dim dtPrice As New DataTable, org As String = ORG_ID
                    Dim _currency As String = ""
                    If MasterRef IsNot Nothing AndAlso Not String.IsNullOrEmpty(MasterRef.currency) Then
                        _currency = MasterRef.currency
                    End If
                    'SAPTools.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", dtPrice)
                    '_SAPDAL.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, "", "", dtPrice)
                    _SAPDAL.getSAPPriceByTable(PartNo, org, quote_to_company, quote_to_company, _currency, "", dtPrice)
                    If dtPrice.Rows.Count > 0 Then
                        For Each rPrice As DataRow In dtPrice.Rows
                            UnitPrice += rPrice.Item("Netwr")
                            ListPrice += rPrice.Item("Kzwi1")
                        Next
                    End If
                End If

            End If
            If UnitPrice = 0 Then
                UnitPrice = 0 : ListPrice = 0 : Exit Sub
            End If
            'Return UnitPrice
            Exit Sub
        End If
        UnitPrice = 0 : ListPrice = 0
    End Sub


    Shared Function getCatalogByUID(ByVal UID As String, ByVal oType As COMM.Fixer.eDocType) As CBOMWS.CBOMDS.CBOM_CATALOGDataTable

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, oType)
        If IsNothing(dt) Then
            Util.showMessage("Invalid UID!") : Return Nothing
        End If
        Dim ws As New CBOMWS.MyCBOMDAL
        ws.Timeout = -1
        Dim BtosDT As CBOMWS.CBOMDS.CBOM_CATALOGDataTable = ws.getCatalogList(dt.siebelRBU, dt.org)
        Return BtosDT
    End Function


    Shared Function getCatalogByUID_OLD(ByVal UID As String, ByVal oType As COMM.Fixer.eDocType) As DataTable

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, oType)
        If IsNothing(dt) Then
            Util.showMessage("Invalid UID!") : Return Nothing
        End If
        Dim ws As New CBOMWS.MyCBOMDAL
        ws.Timeout = -1
        Dim BtosDT As New DataTable
        BtosDT = ws.getCatalogList(dt.siebelRBU, dt.org)
        ws.Dispose()

        Return BtosDT
    End Function
    Shared Function getAccountStatusByAccountRowID(ByVal rowId As String) As String
        Return "CP"
    End Function
    Shared Function getCbomListByCatalogAndOrg(ByVal catalog As String, ByVal Org As String, ByVal company_id As String, ByVal RBU As String) As DataTable
        catalog = catalog.Trim()

        Dim WS As New CBOMWS.MyCBOMDAL
        WS.Timeout = -1
        If COMM.Util.IsTesting Then
            WS.Url = "http://my.advantech.com:4002/Services/MyCBOMDAL.asmx?wsdl"
        End If

        'Dim T_strselect As String = ""
        'Dim T_strWhere As String = ""

        'If catalog <> "CTOS" Then
        '    If catalog = "Pre-Configuration" Then
        '        T_strselect = " select distinct '' as SNO,last_updated_by as CATALOG_NAME,a.CATALOG_DESC,CATALOG_NAME as IMAGE_ID,'' as QTY ,'CONFIG' as Assembly, '' as COMPANY_ID , a.CREATED " & _
        '                  " from CBOM_CATALOG a " & _
        '                  " where a.Catalog_Org='" & Org.ToString.ToUpper & "' and a.CATALOG_TYPE like '%" & catalog & "'"
        '        T_strWhere = ""
        '    Else
        '        If catalog = "eStoreBTO" Then
        '            T_strselect = " select distinct '' as SNO,a.CATALOG_NAME,a.CATALOG_DESC, a.IMAGE_ID,'' as QTY ,'CONFIG' as Assembly, '' as COMPANY_ID , a.CREATED " & _
        '            " from CBOM_CATALOG a " & _
        '            " where a.Catalog_Org='" & Org.ToString.ToUpper & "' and Created_by='EZ'"
        '            T_strWhere = ""
        '        Else
        '            T_strselect = " select distinct '' as SNO,a.CATALOG_NAME,a.CATALOG_DESC, a.IMAGE_ID,'' as QTY ,'CONFIG' as Assembly, '' as COMPANY_ID , a.CREATED " & _
        '             " from CBOM_CATALOG a " & _
        '             " where a.Catalog_Org='" & Org.ToString.ToUpper & "' and a.CATALOG_TYPE like '%" & catalog & "'"
        '            T_strWhere = ""
        '        End If
        '    End If
        'Else
        '    T_strselect = " select distinct '' as SNO,a.CATALOG_NAME,a.CATALOG_DESC, a.IMAGE_ID,'' as QTY ,'CONFIG' as Assembly,c.COMPANY_ID , a.CREATED" & _
        '                  " from CBOM_CATALOG a " & _
        '                  " inner join PRODUCT_CUSTOMER_DICT c" & _
        '                  " on a.CATALOG_NAME=c.PART_NO " & _
        '                  " where a.Catalog_Org='" & Org.ToString.ToUpper & "' and a.CATALOG_TYPE like '%" & catalog & "' and a.CATALOG_NAME=c.PART_NO"
        '    T_strWhere = " and c.Company_id='" & company_id & "' "


        '    T_strselect = T_strselect & T_strWhere & " Order By c.COMPANY_ID asc,a.CATALOG_NAME asc"
        'End If
        ''HttpContext.Current.Response.Write(T_strselect)
        Dim dt As New DataTable
        dt = WS.getCBOMList(RBU, Org, catalog, company_id)
        Return dt
    End Function

    'Public Shared Function GetQBOMSql(ByVal PCatId As String, ByVal UID As String) As DataTable
    '    Dim myQM As New quotationMaster("EQ", "quotationMaster")
    '    Dim DTqm As DataTable = myQM.GetDT(String.Format("quoteId='{0}'", UID), "")
    '    Dim org As String = DTqm.Rows(0).Item("org")

    '    Dim qsb As New System.Text.StringBuilder
    '    With qsb
    '        .AppendLine(" SELECT a.PARENT_CATEGORY_ID, a.CATEGORY_ID, a.CATEGORY_NAME, a.CATEGORY_TYPE, a.CATEGORY_DESC, ")
    '        .AppendLine(" IsNull(a.DISPLAY_NAME,'') as DISPLAY_NAME, IsNull(a.SEQ_NO,0) as SEQ_NO, IsNull(a.DEFAULT_FLAG,0) as DEFAULT_FLAG, ")
    '        .AppendLine(" IsNull(a.CONFIGURATION_RULE,'') as CONFIGURATION_RULE, IsNull(a.NOT_EXPAND_CATEGORY,'') as NOT_EXPAND_CATEGORY, ")
    '        .AppendLine(" IsNull(a.SHOW_HIDE,0) as SHOW_HIDE, IsNull(a.EZ_FLAG,0) as EZ_FLAG, IsNull(b.STATUS,'') as STATUS, 0 as SHIP_WEIGHT,  ")
    '        .AppendLine(" 0 as NET_WEIGHT, IsNull(b.MATERIAL_GROUP,'') as MATERIAL_GROUP, case RoHS_Flag when 1 then 'y' else 'n' end as RoHS, '' as class,a.UID,a.org ")
    '        .AppendLine(" FROM CBOM_CATALOG_CATEGORY AS a LEFT OUTER JOIN ")
    '        .AppendLine(" SAP_PRODUCT AS b ON a.CATEGORY_ID = b.PART_NO ")
    '        .AppendLine(String.Format(" WHERE a.PARENT_CATEGORY_ID = N'{0}' and a.org='" & Business.getCBOMorg(UID) & "' and a.CATEGORY_ID<>N'{0}' ", PCatId))
    '        .AppendLine(" and (a.CATEGORY_TYPE='Category' or A.CATEGORY_TYPE='Component' or (a.CATEGORY_TYPE='Component' and (a.CATEGORY_ID='No Need' or a.CATEGORY_ID like '%|%'))) ")
    '        .AppendLine(" ORDER BY a.SEQ_NO ")
    '    End With
    '    Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", qsb.ToString())
    '    Dim compArray As New ArrayList
    '    For Each r As DataRow In dt.Rows
    '        If r.Item("CATEGORY_TYPE") = "Component" And r.Item("category_id").ToString.Contains("|") Then
    '            Dim ps() As String = Split(r.Item("category_id").ToString, "|")
    '            For Each p As String In ps
    '                If Not LCase(HttpContext.Current.Request.ServerVariables("URL")).Contains("webcbomeditor") Then
    '                    If CInt(tbOPBase.dbExecuteScalar("B2B", String.Format("select count(part_no) as c from sap_product_org where status in " + ConfigurationManager.AppSettings("CanOrderProdStatus") + " and part_no in ('{0}') and org_id='{1}'", p.ToString, org))) <= 0 Then
    '                        r.Delete()
    '                    End If
    '                End If
    '            Next
    '        ElseIf r.Item("CATEGORY_TYPE") = "Component" And Not r.Item("category_id").ToString.Contains("|") And Not r.Item("category_id").ToString.Contains("No Need") Then
    '            If Not LCase(HttpContext.Current.Request.ServerVariables("URL")).Contains("webcbomeditor") Then
    '                If CInt(tbOPBase.dbExecuteScalar("B2B", String.Format("select count(part_no) as c from sap_product_org where status in " + ConfigurationManager.AppSettings("CanOrderProdStatus") + " and part_no in ('{0}') and org_id='{1}'", r.Item("CATEGORY_ID").ToString, org))) <= 0 Then
    '                    r.Delete()
    '                End If
    '            End If
    '        End If
    '    Next
    '    dt.AcceptChanges()
    '    For Each r As DataRow In dt.Rows
    '        If r.Item("CATEGORY_TYPE") = "Component" Then
    '            If compArray.Contains(r.Item("category_id").ToString()) = False Then
    '                compArray.Add(r.Item("category_id").ToString())
    '            Else
    '                r.Delete()
    '            End If
    '        End If
    '    Next
    '    dt.AcceptChanges()
    '    compArray.Clear()
    '    For Each r As DataRow In dt.Rows
    '        If r.Item("CATEGORY_TYPE") = "Category" Then
    '            If compArray.Contains(r.Item("category_id").ToString()) = False Then
    '                compArray.Add(r.Item("category_id").ToString())
    '            Else
    '                r.Delete()
    '            End If
    '        End If
    '    Next
    '    dt.AcceptChanges()
    '    If (PCatId.ToUpper().EndsWith("-BTO") Or PCatId.ToUpper().StartsWith("C-CTOS-")) AndAlso _
    '        CInt( _
    '            tbOPBase.dbExecuteScalar("B2B", _
    '            String.Format("select count(category_id) as c FROM CBOM_CATALOG_CATEGORY where org='" & Business.getCBOMorg(UID) & "' and parent_category_id='Root' and category_id='{0}'", Replace(PCatId, "'", "''")))) > 0 Then
    '        Dim r As DataRow = dt.NewRow()
    '        With r
    '            .Item("CATEGORY_ID") = "Extended Warranty for " + PCatId.ToUpper()
    '            .Item("CATEGORY_NAME") = "Extended Warranty for " + PCatId.ToUpper()
    '            .Item("CATEGORY_TYPE") = "Category"
    '            .Item("CATEGORY_DESC") = "Extended Warranty for " + PCatId.ToUpper()
    '            .Item("DISPLAY_NAME") = "Extended Warranty for " + PCatId.ToUpper()
    '            .Item("SEQ_NO") = 99 : .Item("DEFAULT_FLAG") = "" : .Item("CONFIGURATION_RULE") = ""
    '            .Item("NOT_EXPAND_CATEGORY") = "" : .Item("SHOW_HIDE") = 1 : .Item("EZ_FLAG") = 0
    '            .Item("STATUS") = "" : .Item("SHIP_WEIGHT") = 0 : .Item("NET_WEIGHT") = 0
    '            .Item("MATERIAL_GROUP") = "" : .Item("RoHS") = "n" : .Item("class") = ""
    '        End With
    '        dt.Rows.Add(r)
    '        If tbOPBase.dbGetDataTable("B2B", String.Format("select category_name from cbom_catalog_category where org='" & Business.getCBOMorg(UID) & "' and category_id not like '%-CTOS%' and category_id not like '%SYS-%' and category_id='{0}' and isnull(EZ_Flag,'0')<>'2'", Replace(PCatId, "'", "''"))).Rows.Count > 0 Then
    '            Dim r2 As DataRow = dt.NewRow()
    '            With r2
    '                .Item("CATEGORY_ID") = "CTOS note for " + PCatId.ToUpper()
    '                .Item("CATEGORY_NAME") = "CTOS note for " + PCatId.ToUpper()
    '                .Item("CATEGORY_TYPE") = "Category"
    '                .Item("CATEGORY_DESC") = "CTOS note for " + PCatId.ToUpper()
    '                .Item("DISPLAY_NAME") = "CTOS note for " + PCatId.ToUpper()
    '                .Item("SEQ_NO") = 100 : .Item("DEFAULT_FLAG") = "" : .Item("CONFIGURATION_RULE") = ""
    '                .Item("NOT_EXPAND_CATEGORY") = "" : .Item("SHOW_HIDE") = 1 : .Item("EZ_FLAG") = 0
    '                .Item("STATUS") = "" : .Item("SHIP_WEIGHT") = 0 : .Item("NET_WEIGHT") = 0
    '                .Item("MATERIAL_GROUP") = "" : .Item("RoHS") = "n" : .Item("class") = ""
    '            End With
    '            dt.Rows.Add(r2)
    '        End If
    '    Else
    '        If PCatId.ToUpper().StartsWith("EXTENDED WARRANTY FOR") Then
    '            qsb = New System.Text.StringBuilder
    '            With qsb
    '                .AppendLine(" SELECT PART_NO as CATEGORY_ID, PART_NO as CATEGORY_NAME, 'Component' as CATEGORY_TYPE, ")
    '                .AppendLine(" PRODUCT_DESC as CATEGORY_DESC, PRODUCT_DESC as DISPLAY_NAME, 0 as SEQ_NO, 0 as DEFAULT_FLAG, ")
    '                .AppendLine(" '' as CONFIGURATION_RULE, '' as NOT_EXPAND_CATEGORY, 1 as SHOW_HIDE, 0 as EZ_FLAG, IsNull(STATUS,'') as STATUS, ")
    '                .AppendLine(" 0 as SHIP_WEIGHT, 0 as NET_WEIGHT, MATERIAL_GROUP, case RoHS_Flag when 1 then 'y' else 'n' end as RoHS, '' as Class ")
    '                .AppendLine(" FROM  SAP_PRODUCT ")
    '                .AppendLine(" WHERE PART_NO LIKE 'AGS-EW%' order by PART_NO ")
    '            End With
    '            dt = tbOPBase.dbGetDataTable("b2b", qsb.ToString())
    '        Else
    '            If PCatId.ToUpper().StartsWith("CTOS NOTE FOR") Then
    '                qsb = New System.Text.StringBuilder
    '                With qsb
    '                    .AppendLine(" SELECT distinct a.PART_NO as CATEGORY_ID, a.PART_NO as CATEGORY_NAME, 'Component' as CATEGORY_TYPE, ")
    '                    .AppendLine(" b.PRODUCT_DESC as CATEGORY_DESC, b.PRODUCT_DESC as DISPLAY_NAME, 0 as SEQ_NO, 0 as DEFAULT_FLAG, ")
    '                    .AppendLine(" '' as CONFIGURATION_RULE, '' as NOT_EXPAND_CATEGORY, 1 as SHOW_HIDE, 0 as EZ_FLAG, IsNull(b.STATUS,'') as STATUS, ")
    '                    .AppendLine(" 0 as SHIP_WEIGHT, 0 as NET_WEIGHT, MATERIAL_GROUP, case RoHS_Flag when 1 then 'y' else 'n' end as RoHS, '' as Class ")
    '                    .AppendLine(" from CBOM_CATEGORY_CTOS_NOTE a left join SAP_PRODUCT b on a.part_no=b.part_no ")
    '                    .AppendLine(" order by a.PART_NO ")
    '                End With
    '                dt = tbOPBase.dbGetDataTable("b2b", qsb.ToString())
    '            End If
    '        End If
    '    End If
    '    Return dt
    'End Function

    Public Shared Function GetConfigOrderCartDt() As DataTable
        Dim dt As New DataTable("ConfigCart")
        With dt.Columns
            .Add("CATEGORY_ID", GetType(String)) : .Add("CATEGORY_NAME", GetType(String))
            .Add("CATEGORY_TYPE")
            .Add("PARENT_CATEGORY_ID", GetType(String)) : .Add("CATALOG_ID", GetType(String))
            .Add("CATALOGCFG_SEQ", GetType(Integer)) : .Add("CATEGORY_DESC", GetType(String))
            .Add("DISPLAY_NAME", GetType(String)) : .Add("IMAGE_ID", GetType(String))
            .Add("EXTENDED_DESC", GetType(String)) : .Add("CREATED", GetType(DateTime))
            .Add("CREATED_BY", GetType(String)) : .Add("LAST_UPDATED", GetType(DateTime))
            .Add("LAST_UPDATED_BY", GetType(String)) : .Add("SEQ_NO", GetType(Integer))
            .Add("PUBLISH_STATUS", GetType(String)) : .Add("CATEGORY_PRICE", GetType(Double))
            .Add("CATEGORY_QTY", GetType(Integer)) : .Add("ParentSeqNo", GetType(Integer)) : .Add("ParentRoot", GetType(String))
        End With
        Return dt
    End Function

    Public Shared Function IsHotSelling(ByVal part_no As String, ByVal Org As String) As Boolean
        If Not Org.StartsWith("US") Then
            Return False
        End If

        Dim dt As DataTable = tbOPBase.dbGetDataTable("b2b", String.Format("select * from MYADVANTECH_PRODUCT_PROMOTION where Part_no='{0}' and active_flag=1", part_no))
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function IsFastDelivery(ByVal part_no As String, ByVal Org As String) As Boolean
        If Not Org.StartsWith("US") Then
            Return False
        End If

        Dim dt As DataTable = tbOPBase.dbGetDataTable("b2b", String.Format("select * from MYADVANTECH_PRODUCT_FAST_DELIVERY where Part_no='{0}' and active_flag=1", part_no))
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Shared Function getBankInfoByRBU(ByVal RBU As String) As String
        Dim STR As String = ""

        Dim BankInfo As New BankInfo("EQ", "BankInfo")

        Dim DT As New DataTable
        DT = BankInfo.GetDT(String.Format("RBU='{0}'", RBU), "")
        If DT.Rows.Count > 0 Then
            STR = DT.Rows(0).Item("INFO")
        End If
        Return STR
    End Function

    'Shared Function GET_Curr_By_Company_ID(ByVal Company_id As String) As String
    '    Dim str As Object = String.Format("select TOP 1 isnull(currency,'EUR') as currency from SAP_DIMcompany  where company_id = '{0}' and company_type ='Z001'", Company_id)
    '    Dim curr = tbOPBase.dbExecuteScalar("B2B", str)
    '    If Not IsNothing(curr) AndAlso curr <> "" Then
    '        Return curr
    '    End If
    '    Return "USD"
    'End Function

    'Shared Function GET_Currency_By_InterconCompany_ID(ByVal Company_id As String, ByVal org_id As String) As String
    '    Dim str As Object = String.Format("select isnull(currency,'EUR') as currency from SAP_DIMcompany  where company_id = '{0}' and company_type ='Z001' and ORG_ID='{1}'", Company_id, org_id)
    '    Dim curr = tbOPBase.dbExecuteScalar("B2B", str)
    '    If Not IsNothing(curr) AndAlso curr <> "" Then
    '        Return curr
    '    End If
    '    Return "USD"
    'End Function

    Shared Function GET_Currency_By_Company_ID(ByVal Company_id As String, ByVal org_id As String) As String
        Dim str As Object = String.Format("select isnull(currency,'EUR') as currency from SAP_DIMcompany  where company_id = '{0}' and company_type ='Z001' and ORG_ID='{1}'", Company_id, org_id)
        Dim curr = tbOPBase.dbExecuteScalar("B2B", str)
        If Not IsNothing(curr) AndAlso curr <> "" Then
            Return curr
        End If
        Return "USD"
    End Function



    Shared Function Correctcurrecy(ByVal siebelCurr As String) As String
        Dim str As String = "USD"
        Select Case siebelCurr.Trim
            Case "EURO", "FRF", "PTE", "EUR"
                str = "EUR"
            Case "ILS", "INR", "USD"
                str = "USD"
            Case "NTD", "TWD"
                str = "TWD"
            Case "RMB(￥)", "RMB", "CNY"
                str = "CNY"
            Case "YEN", "JPY"
                str = "JPY"
            Case ""
                str = "USD"
        End Select
        Return str
    End Function
    Shared Function getPiPage(ByVal UID As String, ByVal DocReg As COMM.Fixer.eDocReg) As String
        If (DocReg And COMM.Fixer.eDocReg.AFC) = DocReg Then
            Return "piAIN.aspx"
        End If
        If (DocReg And COMM.Fixer.eDocReg.ACN) = DocReg Then
            Return "piACN.aspx"
        End If
        Return "piHtml.aspx"
    End Function
    Shared Function getMailTempPage() As String
        Return "MailTemp.aspx"
    End Function
    Shared Function getOrgByQuoteId(ByVal UID As String, ByVal oType As COMM.Fixer.eDocType) As String
        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, oType)
        If IsNothing(dt) Then
            Util.showMessage("Invalid UID!")
            Return Nothing
        End If
        'Dim strSqlCmd As String
        'Dim org As String = getCBOMorg(UID)
        ''If dt.Rows(0).Item("quoteToErpId") <> "" Then
        ''    org = getOrgByOrgID(getOrgByErpId(dt.Rows(0).Item("quoteToErpId")))
        ''End If
        'strSqlCmd = _
        '"select DISTINCT IsNull(Catalog_Type, '') as Catalog_Type from CBOM_Catalog WHERE Catalog_Org='" & org.ToString.ToUpper & "' and Catalog_Type <>'' "
        Return dt.org

    End Function
    Shared Function getOptyIdByQuoteId(ByVal quoteId As String) As String
        Dim optyID As String = String.Empty
        Dim dt As DataTable = Nothing
        'Dim MyOptyQuote As New optyQuote("eq", "optyQuote")
        'dt = MyOptyQuote.GetDT(String.Format("quoteId='{0}'", quoteId), "")
        Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
        dt = myOptyQuote.GetOptyQuoteByOuoteID(quoteId)

        If dt.Rows.Count > 0 Then
            optyID = dt.Rows(0).Item("optyId")
        End If
        dt = Nothing
        myOptyQuote = Nothing
        Return optyID
    End Function

    <Obsolete("This method has been replaced by SAPDAL.syncSingleCompany.syncSingleSAPCustomer")>
    Public Shared Function SyncCompanyIdFromSAP(ByVal companyid As String) As Boolean
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" select kna1.kunnr as Company_Id, "))
            .AppendLine(String.Format(" 	   knvv.vkorg as org_id, "))
            .AppendLine(String.Format("     (select MIN(knvp.kunnr) from saprdp.knvp where knvp.kunn2 = kna1.kunnr and knvp.vkorg=knvv.vkorg AND knvp.parvw='WE') as ParentCompanyId, "))
            .AppendLine(String.Format(" 		kna1.name1 || kna1.name2 as Company_Name, "))
            .AppendLine(String.Format(" 		adrc.street || adrc.str_suppl3 || adrc.location as Address, "))
            .AppendLine(String.Format(" 		kna1.telfx as fax_no, "))
            .AppendLine(String.Format(" 		kna1.telf1 as tel_no, "))
            .AppendLine(String.Format(" 		kna1.ktokd as company_type, "))
            .AppendLine(String.Format(" 		kna1.kdkg1 || kna1.kdkg2 || kna1.kdkg3 || kna1.kdkg4 as price_class,  "))
            .AppendLine(String.Format("     '' as ptrade_price_class, "))
            .AppendLine(String.Format(" 		knvv.waers as Currency, "))
            .AppendLine(String.Format(" 		adrc.country as Country,  "))
            .AppendLine(String.Format("     '' as region, "))
            .AppendLine(String.Format(" 		adrc.post_code1 as Zip_Code, "))
            .AppendLine(String.Format(" 		adrc.city1 as City, "))
            .AppendLine(String.Format(" 		adrc.name_co as Attention, "))
            .AppendLine(String.Format(" 		'0' as Credit_Limit, "))
            .AppendLine(String.Format(" 		knvv.zterm as Credit_Term, "))
            .AppendLine(String.Format(" 		knvv.inco1 || '  ' || knvv.inco2 as Ship_Via, "))
            .AppendLine(String.Format(" 		kna1.knurl as Url,  "))
            .AppendLine(String.Format("     '' as LAST_UPDATED,  "))
            .AppendLine(String.Format("     '' as UPDATED_BY,  "))
            .AppendLine(String.Format(" 		kna1.erdat as CREATED_DATE, "))
            .AppendLine(String.Format(" 		kna1.ernam as Created_By, "))
            .AppendLine(String.Format(" 		knvv.kdgrp as Company_Price_Type,	 "))
            .AppendLine(String.Format("     '' as SALES_USERID,	 "))
            .AppendLine(String.Format(" 		knvv.vsbed as SHIP_CONDITION, "))
            .AppendLine(String.Format(" 		kna1.KATR4 as attribute4, "))
            .AppendLine(String.Format(" 		KNVV.VKBUR as SalesOffice, "))
            .AppendLine(String.Format("     KNVV.VKGRP as SalesGroup, "))
            .AppendLine(String.Format(" (select KNVI.TAXKD from saprdp.KNVI where KNVI.kunnr=kna1.kunnr and KNVI.ALAND = 'NL' and KNVI.TATYP = 'MWST' and KNVI.mandt='168' and rownum=1) as TAX_CLASS "))
            .AppendLine(String.Format(" from saprdp.knvv inner join saprdp.kna1 on knvv.kunnr=kna1.kunnr  "))
            .AppendLine(String.Format(" 	inner join saprdp.adrc on kna1.adrnr=adrc.addrnumber and kna1.land1=adrc.country   "))
            .AppendLine(String.Format(" where rownum=1 and knvv.mandt='168'  and kna1.loevm = ' ' and knvv.kunnr='{0}' ", companyid.Trim.Replace("'", "").ToUpper()))
        End With
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
        If dt.Rows.Count > 0 Then
            For Each Row As DataRow In dt.Rows
                Row.Item("CREATED_DATE") = Date.ParseExact(Row.Item("CREATED_DATE").ToString(), "yyyyMMdd", New System.Globalization.CultureInfo("fr-FR"))
                tbOPBase.dbExecuteNoQuery("my", "delete from sap_dimcompany where company_id='" + companyid.Trim.Replace("'", "").ToUpper() + "'")
                Dim sql As String = " INSERT INTO [SAP_DIMCOMPANY]([UNIQUE_ID],[COMPANY_ID],[ORG_ID],[PARENTCOMPANYID]," +
                    "[COMPANY_NAME],[ADDRESS],[FAX_NO],[TEL_NO],[COMPANY_TYPE],[PRICE_CLASS],[CURRENCY],[COUNTRY],[ZIP_CODE]," +
                    "[CITY],[ATTENTION],[CREDIT_TERM],[SHIP_VIA],[URL],[CREATEDDATE],[CREATED_BY],[COMPANY_PRICE_TYPE],[SHIPCONDITION]," +
                    "[ATTRIBUTE4],[SALESOFFICE],[SALESGROUP])VALUES ("
                sql = sql + "'','" + Row.Item("COMPANY_ID") + "','" + Row.Item("ORG_ID") + "','" + Row.Item("PARENTCOMPANYID") + "'" &
                ",'" + Replace(Row.Item("COMPANY_NAME"), "'", "''") + "','" + Replace(Row.Item("ADDRESS"), "'", "''") +
                "','" + Replace(Row.Item("FAX_NO"), "'", "''") + "','" + Replace(Row.Item("TEL_NO"), "'", "''") + "' " &
                ",'" + Row.Item("COMPANY_TYPE") + "','" + Row.Item("PRICE_CLASS") + "','" + Row.Item("CURRENCY") +
                "','" + Replace(Row.Item("COUNTRY"), "'", "''") + "' " &
                ",'" + Row.Item("ZIP_CODE") + "','" + Row.Item("CITY").ToString().Replace("'", "''") +
                "','" + Replace(Row.Item("ATTENTION"), "'", "''") + "','" + Row.Item("CREDIT_TERM") + "','" + Replace(Row.Item("SHIP_VIA"), "'", "''") + "' " &
                ",'" + Replace(Row.Item("URL"), "'", "''") + "','" + Row.Item("CREATED_DATE") + "','" + Row.Item("CREATED_BY") +
                "','" + Row.Item("COMPANY_PRICE_TYPE") + "','" + Replace(Row.Item("SHIP_CONDITION"), "'", "''") + "' " &
               " ,'" + Row.Item("ATTRIBUTE4") + "','" + Row.Item("SALESOFFICE") + "','" + Row.Item("SALESGROUP") + "')"
                Dim retint As Integer = tbOPBase.dbExecuteNoQuery("my", sql)
                If retint = -1 Then
                    Return False
                End If
            Next
            Dim Sqlupdate As String = " update SAP_DIMCOMPANY " &
                     " set unique_id=dbo.MD5Hash(COMPANY_ID+'|'+ORG_ID+'|'+COMPANY_NAME+'|'+SHIP_VIA+'|'+SALESOFFICE+'|'+SALESGROUP+'|'+CREDIT_TERM) " &
                     " where COMPANY_ID = '" + companyid.Trim.Replace("'", "").ToUpper() + "'"
            Dim retupdateint As Integer = tbOPBase.dbExecuteNoQuery("my", Sqlupdate)
            If retupdateint = -1 Then
                Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com,Mike.Liu@advantech.com,tc.chen@advantech.com.tw,Jay.lee@advantech.com", "", "", "eQuotation Pick Account Sync ERP ID Failed!" + companyid, "", Now.ToString, "")

                Return False
            End If
            dt.AcceptChanges()
            Return True
        End If
        Return False
    End Function

    Shared Function is_Valid_Company_Id(ByVal company_id As String) As Boolean
        If company_id.Trim = "" Then
            Return False
        End If
        Dim Valid_Company As Dictionary(Of String, Boolean) = CType(HttpContext.Current.Cache("Valid_Company"), Dictionary(Of String, Boolean))

        If Valid_Company Is Nothing Then
            Valid_Company = New Dictionary(Of String, Boolean)
            HttpContext.Current.Cache("Valid_Company") = Valid_Company
            HttpContext.Current.Cache.Add("Valid_Company", Valid_Company, Nothing, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If

        If (Valid_Company.ContainsKey(company_id.ToUpper())) = False Then
            Dim str As String = String.Empty
            'Ming 20150414 請開放Siebel RBU=ADLoG的account到AEU eQuotation的account view中，但須檢查ERPID是否對應到SAP org EU10
            If Role.IsEUSales Then
                str = String.Format("select top 1 COMPANY_ID from SAP_DIMCOMPANY where COMPANY_ID='{0}' and ORG_ID='EU10' and COMPANY_Type='Z001'", company_id)
            Else
                str = String.Format("select top 1 COMPANY_ID from SAP_DIMCOMPANY where COMPANY_ID='{0}' and COMPANY_Type='Z001'", company_id)
            End If
            Dim dt As New DataTable
            dt = tbOPBase.dbGetDataTable("B2B", str)
            If dt.Rows.Count = 0 Then
                'Dim ws As New MAMigrationWS.MAMigration ----- Marked Nada 
                'Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Pick Account Sync ERP ID Start : " + company_id, "", Now.ToString, "")
                'If SyncCompanyIdFromSAP(company_id) AndAlso tbOPBase.dbGetDataTable("B2B", str).Rows.Count > 0 Then Return True
                'Ming add 20140404 Cache只记录验证为true的Erpid, 同步customer调用最新的同步方法
                Dim errMsg As String = String.Empty, Erpids As New ArrayList : Erpids.Add(company_id)
                Dim ds As SAPDAL.DimCompanySet = SAPDAL.syncSingleCompany.syncSingleSAPCustomer(Erpids, False, errMsg)
                If Not String.IsNullOrEmpty(errMsg.Trim) Then
                    Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Pick Account Sync ERP ID Error : " + company_id, "", Now.ToString + vbCrLf + errMsg, "")
                    Return False
                End If
                dt = tbOPBase.dbGetDataTable("B2B", str)
            End If
            If dt.Rows.Count > 0 Then Valid_Company.Add(company_id.ToUpper(), True)
        End If
        If Valid_Company.ContainsKey(company_id.ToUpper()) Then
            Return Valid_Company.Item(company_id.ToUpper())
        End If
        Return False
    End Function
    Shared Function getPLMNote(ByVal pn As String, ByVal org As String) As String
        Dim STR As String = String.Format("select * from SAP_PRODUCT_ORDERNOTE where org='{1}' AND part_no='{0}'", pn, org)
        Dim DT As New DataTable
        DT = tbOPBase.dbGetDataTable("b2b", STR)
        'HttpContext.Current.Response.Write(STR)
        If DT.Rows.Count > 0 Then
            Return DT.Rows(0).Item("txt")
        End If
        Return ""
    End Function

    Shared Function getMOQ(ByVal pn As String, ByVal org As String) As String
        Dim STR As String = String.Format("select PART_NO,MIN_ORDER_QTY from SAP_PRODUCT_STATUS where SALES_ORG='{1}' AND part_no='{0}'", pn, org)
        Dim DT As New DataTable
        DT = tbOPBase.dbGetDataTable("b2b", STR)
        If DT.Rows.Count > 0 Then
            Return CInt(DT.Rows(0).Item("MIN_ORDER_QTY"))
        End If
        Return ""
    End Function


    Shared Function getPartIndicator(ByVal pn As String, ByVal plant As String) As String
        Dim STR As String = String.Format("Select PART_NO,PLANT,ABC_INDICATOR from SAP_PRODUCT_ABC where PLANT='{1}' AND part_no='{0}'", pn, plant)
        Dim DT As New DataTable
        DT = tbOPBase.dbGetDataTable("b2b", STR)
        'HttpContext.Current.Response.Write(STR)
        If DT.Rows.Count > 0 Then
            Return DT.Rows(0).Item("ABC_INDICATOR")
        End If
        Return ""
    End Function

    Shared Function getATPdetail(ByVal pn As String, ByVal org As String) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("date") : dt.Columns.Add("qty")
        'Try
        Dim dttemp As New DataTable
        SAPTools.getInventoryAndATPTable(pn, Left(org, 2) & "H1", 0, "", 0, dttemp)
        If dttemp.Rows.Count > 0 Then
            For i As Integer = 0 To dttemp.Rows.Count - 1
                If CInt(dttemp.Rows(i).Item("com_qty")) <> 0 Then
                    Dim r As DataRow = dt.NewRow
                    r.Item("date") = dttemp.Rows(i).Item("com_date") : r.Item("qty") = CInt(dttemp.Rows(i).Item("com_qty")) : dt.Rows.Add(r)
                End If
            Next
        End If
        If dt.Rows.Count = 0 Then
            Dim r As DataRow = dt.NewRow
            r.Item("date") = Now.ToString("yyyyMMdd") : r.Item("qty") = 0 : dt.Rows.Add(r)
        End If
        For Each r As DataRow In dt.Rows
            Dim strATPDate As String = r.Item("date").ToString(), dtATPDate As Date = Date.MaxValue
            If Date.TryParseExact(strATPDate, "yyyyMMdd", New System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, dtATPDate) Then
                r.Item("date") = dtATPDate.ToString("yyyy/MM/dd")
            End If
            'r.Item("date") = Date.ParseExact(r.Item("date"), "yyyyMMdd", New Globalization.CultureInfo("en-US")).ToString("yyyy/MM/dd")
            'r.Item("date") = Date.Parse(r.Item("date")).ToString("yyyy/MM/dd")
        Next
        Return dt
        'Catch ex As Exception

        'End Try

        Return Nothing
    End Function

    Shared Function IsPartInBTOS_CTOSMaterialGroup(ByVal partNo As String) As Boolean
        'Ming 20150415
        '1. material group=CTOS And PN starts with C-CTOS
        '2. material group=BTOS And PN ends with -BTO
        Dim _RecordCount As Integer = tbOPBase.dbExecuteScalar("B2B", String.Format("select count(material_group) from sap_product where part_no='{0}' and ( (material_group='CTOS' And part_no LIKE  'C-CTOS%') OR (material_group='BTOS' And part_no LIKE  '%-BTO'))", partNo))
        If _RecordCount > 0 Then Return True
        Return False
    End Function


    Shared Function isEWable(ByVal partNo As String) As Boolean
        Dim f As Boolean = True
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", String.Format("select * from sap_product where part_no='{0}' and material_group in ('ZHD0','ZSPC','ZINS')", partNo))
        If dt.Rows.Count > 0 Then
            Return False
        End If
        Return f
    End Function

    Shared Function isValidSalesEmail(ByVal email As String) As Boolean
        If email = "" Then Return True
        Dim F As Boolean = False
        Dim DT As New DataTable
        DT = getContact(email)
        If DT.Rows.Count > 0 Then
            F = True
        End If
        Return F
    End Function



    Shared Function checkProduct(ByVal org As String, ByVal partNo As String, ByVal Desc As String) As DataTable
        Dim str As String = String.Format(
           " select distinct TOP 100 a.Part_no, b.model_no, b.product_desc,a.product_status " +
           " from SAP_PRODUCT_STATUS a inner join sap_product b ON A.PART_NO=B.PART_NO " +
           " where a.part_no = '{0}' and b.PRODUCT_DESC like '%{2}%' and a.sales_org='{1}' " +
           " and a.part_no not like '%-bto' and a.PRODUCT_STATUS in " + ConfigurationManager.AppSettings("CanOrderProdStatus"), partNo.Trim, org, Desc)
        'and b.PRODUCT_HIERARCHY!='EAPC-INNO-DPX'
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("b2b", str)
        'HttpContext.Current.Response.Write(str)
        Return dt
    End Function
    Shared Function getProduct(ByVal org As String, ByVal partNo As String, ByVal Desc As String, Optional ByVal Btos As String = "0") As DataTable

        Dim str As String = String.Empty
        If org.Equals("TW01", StringComparison.InvariantCultureIgnoreCase) _
            OrElse org.Equals("CN01", StringComparison.InvariantCultureIgnoreCase) _
            OrElse org.Equals("KR01", StringComparison.InvariantCultureIgnoreCase) _
            OrElse org.Equals("JP01", StringComparison.InvariantCultureIgnoreCase) _
            OrElse org.Equals("BR01", StringComparison.InvariantCultureIgnoreCase) Then

            'ICC 2015/6/11 Add sql condition for ATW & ACN to change BTOS part no
            Dim btosSql As String = String.Empty
            If Btos = "1" Then
                btosSql = " and a.part_no like '%-bto' "
            End If
            str = String.Format(
               " select distinct TOP 15 a.Part_no, b.model_no, b.product_desc,a.product_status " +
               " from SAP_PRODUCT_STATUS a inner join sap_product b ON A.PART_NO=B.PART_NO " +
               " where a.part_no like '{0}%' and b.PRODUCT_DESC like '%{2}%' and a.sales_org='{1}'  {3} " +
               " order by  a.product_status , a.Part_no ", partNo.Trim, org, Desc, btosSql)
        ElseIf (org.Equals("US01", StringComparison.InvariantCultureIgnoreCase) And Btos.Equals("1")) Then
            'Frank 20160721: Aonline EP is allowed to pick bto part number
            str = String.Format(
               " select distinct TOP 15 a.Part_no, b.model_no, b.product_desc,a.product_status " +
               " from SAP_PRODUCT_STATUS a inner join sap_product b ON A.PART_NO=B.PART_NO " +
               " where a.part_no like '{1}%' and b.PRODUCT_DESC like '%{3}%' and a.sales_org='{2}' " +
               " and a.PRODUCT_STATUS in {0}" +
               " order by  a.product_status , a.Part_no ", ConfigurationManager.AppSettings("CanOrderProdStatus"), partNo.Trim, org, Desc)
        Else
            str = String.Format(
               " select distinct TOP 15 a.Part_no, b.model_no, b.product_desc,a.product_status " +
               " from SAP_PRODUCT_STATUS a inner join sap_product b ON A.PART_NO=B.PART_NO " +
               " where a.part_no like '{1}%' and b.PRODUCT_DESC like '%{3}%' and a.sales_org='{2}' " +
               " and a.part_no not like '%-bto' and a.PRODUCT_STATUS in {0}" +
               " order by  a.product_status , a.Part_no ", ConfigurationManager.AppSettings("CanOrderProdStatus"), partNo.Trim, org, Desc)
        End If



        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("b2b", str)
        'HttpContext.Current.Response.Write(str)
        Return dt
    End Function
    Shared Function getProductBTO(ByVal org As String, ByVal partNo As String, ByVal Desc As String) As DataTable
        Dim str As String = String.Format(
           " select distinct TOP 100 a.Part_no, b.model_no, b.product_desc,a.product_status " +
           " from SAP_PRODUCT_STATUS a inner join sap_product b ON A.PART_NO=B.PART_NO " +
           " where a.part_no like '{0}%' and b.PRODUCT_DESC like '%{2}%' and a.sales_org='{1}' " +
           " and a.part_no like '%-bto' and a.PRODUCT_STATUS in " + ConfigurationManager.AppSettings("CanOrderProdStatus"), partNo.Trim, org, Desc)
        'and b.PRODUCT_HIERARCHY!='EAPC-INNO-DPX'
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("b2b", str)
        'HttpContext.Current.Response.Write(str)
        Return dt
    End Function
    Shared Function getContact(ByVal emailPat As String) As DataTable
        Dim str As String = String.Format(
           "SELECT DISTINCT TOP 10 A.EMAIL_ADDR from S_USER B LEFT JOIN S_CONTACT A ON A.ROW_ID=B.ROW_ID " &
           "WHERE A.EMAIL_ADDR like '%@%.%' AND UPPER(A.EMAIL_ADDR) like '{0}%' AND UPPER(B.LOGIN) NOT LIKE 'DELETE%' ORDER BY A.EMAIL_ADDR ", emailPat.Replace("''", "'").ToUpper)
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("CRM", str)
        Return dt
    End Function

    Shared Function getDefaultEmployeeEMail(ByVal ERPID As String) As String
        Dim str As String = String.Format(
           " Select b.EMAIL from MyAdvantechGlobal.dbo.SAP_COMPANY_PARTNERS a " &
           " left join MyAdvantechGlobal.dbo.SAP_EMPLOYEE b on a.SALES_CODE=b.SALES_CODE " &
           " WHERE a.COMPANY_ID='{0}' and a.PARTNER_FUNCTION='ZM'", ERPID)
        Dim _email As Object = tbOPBase.dbExecuteScalar("MY", str)
        If _email IsNot Nothing Then
            Return _email
        Else
            Return ""
        End If
    End Function


    Shared Function getEmployeeList(ByVal emailPat As String) As DataTable
        Dim str As String = String.Format(
           "SELECT DISTINCT TOP 10 A.EMAIL_ADDR from EZ_EMPLOYEE A " &
           "WHERE A.EMAIL_ADDR like '%@%.%' AND UPPER(A.EMAIL_ADDR) like '{0}%' ORDER BY A.EMAIL_ADDR ", emailPat.Replace("''", "'").ToUpper)
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("MY", str)
        Return dt
    End Function


    Shared Function getANAGPPercByPN(ByVal pn As String, ByVal div As String) As Decimal
        Dim aptSapDs As New SAPDSTableAdapters.SAP_PRODUCTTableAdapter, pLine As String = aptSapDs.GetProductLineByPN(pn)
        If String.IsNullOrEmpty(pLine) Then pLine = "Other"
        Dim strSql As String = String.Format("select top 1 PPerc From ANAProductGP where CDiv=@DIV and PHrc=@PLINE", div)
        Dim sqlCmd As New SqlClient.SqlCommand(strSql, New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString))
        sqlCmd.Parameters.AddWithValue("DIV", div) : sqlCmd.Parameters.AddWithValue("PLINE", pLine)
        sqlCmd.Connection.Open()
        Dim objPercentage As Object = sqlCmd.ExecuteScalar()
        sqlCmd.Connection.Close()
        If objPercentage IsNot Nothing Then
            Return CType(objPercentage, Decimal) / 100
        Else
            Select Case div
                Case 10
                    Return 0.2
                Case 20
                    Return 0.16
                Case Else
                    Return 0
            End Select
        End If
    End Function

    '<Obsolete("Replaced by isANAPnBelowGPV2 (Frank)")>
    'Shared Function isANAPnBelowGP(ByVal PN As String, ByVal unitPrice As Decimal, ByRef gpPrice As Decimal, ByRef errmsg As String) As Boolean
    '    Return Relics.CommonLogic.isANAPnBelowGP(PN, unitPrice, gpPrice, errmsg)
    'End Function

    Shared Function isANAPnBelowGPV2(ByVal ERPID As String, ByVal PN As String, ByVal unitPrice As Decimal, ByRef gpPrice As Decimal, ByRef errmsg As String) As Boolean

        If is_AUSNoGPBlock_Company_Id(ERPID) Then Return False


        If Not SAPDAL.SAPDAL.isNeedANAGPControl(PN) Then
            Return False
        End If

        Dim _SAPDAL As New SAPDAL.SAPDAL
        gpPrice = _SAPDAL.getPriceByProdLine(PN, _SAPDAL.getProdPricingGrp(PN), errmsg)

        If gpPrice > unitPrice Then
            Dim listprice As Decimal = 0
            listprice = getListPrice(PN, "US01", "USD")
            If listprice < gpPrice Then
                gpPrice = listprice
            End If
        End If

        If gpPrice > 0 AndAlso unitPrice < gpPrice Then
            Return True
        End If
        'PPerc = getANAGPPercByPN(PN, div)
        'Dim Pcost As Decimal = getCostForANAPn(PN, Plant)
        'If (unitPrice - Pcost) < (PPerc * Pcost) Then
        '    Return True
        'Else
        '    Return False
        'End If
        Return False


        'Return Relics.CommonLogic.isANAPnBelowGP(PN, unitPrice, gpPrice, errmsg)

    End Function


    Shared Function is_AUSNoGPBlock_Company_Id(ByVal company_id As String) As Boolean
        If company_id.Trim = "" Then
            Return False
        End If
        Dim Valid_Company As Dictionary(Of String, Boolean) = CType(HttpContext.Current.Cache("AUSNoGPBlock_Company"), Dictionary(Of String, Boolean))

        If Valid_Company Is Nothing Then
            Valid_Company = New Dictionary(Of String, Boolean)
            HttpContext.Current.Cache("AUSNoGPBlock_Company") = Valid_Company
            HttpContext.Current.Cache.Add("AUSNoGPBlock_Company", Valid_Company, Nothing, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If

        If (Valid_Company.ContainsKey(company_id.ToUpper())) = False Then
            Dim str As String = String.Format("Select ERPID from AUSNoGPBlockCompany where ERPID='{0}'", company_id)
            Dim dt As New DataTable
            dt = tbOPBase.dbGetDataTable("EQ", str)

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return False

            Valid_Company.Add(company_id.ToUpper(), dt.Rows.Count > 0)
        End If
        Return Valid_Company.Item(company_id.ToUpper())
    End Function



    Shared Function isOrderable(ByVal UID As String) As Boolean

        Dim DT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(DT) Then
            If Franchise.isFranchiser() Then
                Return True
            End If
            If is_Valid_Company_Id(DT.AccErpId) And DT.qStatus = CInt(COMM.Fixer.eDocStatus.QFINISH) Then
                Return True
            End If
        End If
        Return False
    End Function

    'Shared Function isOrderable(ByVal siebelRBU As String, ByVal quoteToErpId As String, ByVal qStatus As String, ByVal quoteID As String, ByVal docReg As COMM.Fixer.eDocReg) As Boolean
    Shared Function isOrderable(ByVal siebelRBU As String, ByVal quoteToErpId As String, ByVal qStatus As String, ByVal quoteID As String) As Boolean
        'Frank 2012/09/14 If quote id start with AUSQ (means ANA) and siebelRBU is empty then set ANADMF as default value for siebelRBU
        'If String.IsNullOrEmpty(siebelRBU) AndAlso quoteID.StartsWith("AUSQ", StringComparison.InvariantCultureIgnoreCase) Then

        If String.IsNullOrEmpty(siebelRBU) AndAlso Role.IsUsaUser Then
            siebelRBU = "ANADMF"
        End If

        If String.IsNullOrEmpty(siebelRBU) Or String.IsNullOrEmpty(quoteToErpId) Or String.IsNullOrEmpty(qStatus) Then
            Return False
        Else
            If Role.IsFranchiser Then
                Return False
            End If
            If is_Valid_Company_Id(quoteToErpId) And qStatus = CInt(COMM.Fixer.eDocStatus.QFINISH) Then
                Return True
            End If
        End If
        Return False
    End Function

    Shared Function isCanInsert2Siebel(ByVal UID As String) As Boolean
        Return False
    End Function

    Shared Function GetAJPPTradePrice(ByVal UnitPrice As Decimal, ByVal ITP As Decimal) As Decimal
        If UnitPrice < CType((ITP / 0.75), Decimal) Then
            Return Decimal.Round(CType((ITP / 0.75), Decimal), 0)
        Else
            Return UnitPrice
        End If
    End Function

    Shared Function getMyQuoteRecordV2(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                   ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                                   ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date) As String
        Dim sbSQLSTR As New System.Text.StringBuilder
        With sbSQLSTR
            .AppendLine(" select top 200 a.quoteId, quoteNo, Revision_Number, quoteToName, quoteDate, customId, a.quoteToErpId, a.siebelRBU, ")
            .AppendLine(" (SELECT optyid FROM optyQuote where optyQuote.quoteId=a.quoteid) as siebelQuoteId, DOCSTATUS, org, expiredDate ")
            .AppendLine(" from quotationMaster a left join eQuotation.dbo.quoteSiebelQuote b on a.quoteid=b.quoteid ")
            .AppendLine(String.Format(" where a.createdBy=N'{0}' ", user))
            .AppendLine(" and a.Active=1 ")
            .AppendLine(" and quoteDate between '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' and '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
            If Not String.IsNullOrEmpty(QuoteNo) Then .AppendLine(String.Format(" and a.quoteNo like '%{0}%' ", QuoteNo))
            If Not String.IsNullOrEmpty(CustomId) Then .AppendLine(String.Format(" and a.CustomId like '%{0}%' ", CustomId))
            If Not String.IsNullOrEmpty(CompanyName) Then .AppendLine(String.Format(" and a.quoteToName like '%{0}%' ", CompanyName))
            If Not String.IsNullOrEmpty(CompanyID) Then .AppendLine(String.Format(" and a.quoteToErpId like '%{0}%' ", CompanyID))
            If Not String.IsNullOrEmpty(Status) Then .AppendLine(String.Format(" and a.DOCSTATUS = '{0}' ", Status))
            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))
            .AppendLine(" order by quoteDate desc ")
        End With
        Return sbSQLSTR.ToString()
    End Function





    'Shared Function getMyQuoteRecord_FilterByQuoteIDList_OLD(ByVal QuoteID As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String, _
    '                                ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String) As String
    '    Dim myQM As New quotationMaster("EQ", "quotationMaster")
    '    Dim SQLSTR As String = String.Format( _
    '        " select top 1000 a.quoteId,quoteToName,quoteDate,customId,a.quoteToErpId,a.siebelRBU,siebelQuoteId,qstatus,org" + _
    '        " from {0} a left join quoteSiebelQuote b on a.quoteid=b.quoteid" + _
    '        " where a.QuoteId like '%{5}%' and CustomId like N'%{1}%' and quoteToName like N'%{2}%' " + _
    '        " and quoteToErpId like N'%{3}%' and qStatus like N'%{4}%' " + _
    '        " and a.QuoteId in ({6}) " + _
    '    IIf(user.StartsWith("tc.chen@advantech"), "", " and createdBy='" + user + "' ") + _
    '        " order by quoteDate desc", _
    '        myQM.tb, CustomId, CompanyName, CompanyID, Status, QuoteID, QuoteIDList)
    '    'Response.Write(SQLSTR)
    '    Return SQLSTR
    '    'Dim dt As New DataTable
    '    'dt = tbOPBase.dbGetDataTable("EQ", SQLSTR)
    '    'Return dt
    'End Function

    Shared Function getMyQuoteRecordV3(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                           ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                           ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal OptyProjectName As String, Optional ByVal datePeriodType As Integer = 0) As String

        Dim _IsTWUser = Role.IsTWAonlineSales()
        Dim _IsCNAonline = Role.IsCNAonlineSales()
        Dim _IsHQDCUser = Role.IsHQDCSales()
        Dim _IsUSAonline = Role.IsAonlineUsa()
        Dim _IsUSAAC = Role.IsUSAACSales()

        Dim sbSQLSTR As New System.Text.StringBuilder
        With sbSQLSTR
            .AppendLine(" select top 300 a.quoteId, a.quoteNo, a.Revision_Number, quoteToName, a.quoteDate, customId, a.quoteToErpId")
            .AppendLine(" , a.siebelRBU, DOCSTATUS, org ,a.expiredDate,a.createdby,a.isRepeatedOrder ")
            If _IsTWUser Or _IsCNAonline Or _IsHQDCUser Or _IsUSAAC Then
                .AppendLine(" , isnull(c.name,b.optyId) as siebelQuoteId ")
            Else
                .AppendLine(" , b.optyId as siebelQuoteId ")
            End If

            'Ryan 20180419 Add fields for ATW
            .AppendLine(" , a.createddate, a.lastupdateddate ")

            'Frank 2012/07/24
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log")
            .AppendLine(" ,docReg,qstatus ")

            'Ryan 20160726 Revised for pipeline status
            If _IsUSAonline Then
                .AppendLine(" ,(SELECT top 1 Probability FROM eQuotation.dbo.pipelineReport where pipelineReport.quoteId=a.Quoteid) as Probability ")
                .AppendLine(" ,case when (SELECT top 1 QuoteId FROM eQuotation.dbo.pipelineReport where pipelineReport.quoteId=a.Quoteid) is null then 0 else 1 end as isPipelined ")
            End If

            .AppendLine(" from quotationMaster a left join optyQuote b on a.quoteid=b.quoteid ")

            If _IsTWUser Or _IsCNAonline Or _IsHQDCUser Or _IsUSAAC Then
                .AppendLine(" left join [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] c on b.optyid=c.row_id ")
            End If

            '.AppendLine(String.Format(" where a.createdBy=N'{0}' ", user))
            .AppendLine(" where")

            If String.Equals(user, "chiara.geroli@advantech.it", StringComparison.CurrentCultureIgnoreCase) OrElse
             String.Equals(user, "emilie.azevdeo@advantech.fr", StringComparison.CurrentCultureIgnoreCase) Then
                .AppendLine(String.Format(" ( a.createdBy=N'{0}' or  a.createdBy='agnes.iglesias@advantech.fr' ) ", user))

            Else
                .AppendLine(String.Format(" a.createdBy=N'{0}' ", user))
            End If
            '   .AppendLine(String.Format(" a.createdBy=N'{0}' ", user))

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            'Ryan 20161020 Add for Joe Neary case
            If user.Equals("josephn@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then
                Dim groupforJoe As String = System.Web.HttpContext.Current.Session("Joe").ToString
                If Not String.IsNullOrEmpty(groupforJoe) Then
                    If groupforJoe.Equals("Joebtn_1", StringComparison.InvariantCultureIgnoreCase) Then
                        .AppendLine(" and a.quoteNo like 'AUSQ%' ")
                    ElseIf groupforJoe.Equals("Joebtn_2", StringComparison.InvariantCultureIgnoreCase) Then
                        .AppendLine(" and a.quoteNo like 'AACQ%' ")
                    End If
                End If
            End If
            'End Ryan 20161020 Add

            '.AppendLine(" and quoteDate between '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' and '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
            'Frank 2012/10/22
            'If QuoteCreateFromDate <> Nothing Then
            '    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
            'End If
            'If QuoteCreateToDate <> Nothing Then
            '    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
            'End If
            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then .AppendLine(String.Format(" and a.quoteNo like '%{0}%' ", QuoteNo))

            'If Not String.IsNullOrEmpty(CustomId) Then .AppendLine(String.Format(" and a.CustomId like '%{0}%' ", CustomId))
            If Not String.IsNullOrEmpty(CustomId) Then
                Dim _substr1 As String = " and a.CustomId like N'%" + CustomId.Replace("'", "''") + "%' "
                .AppendLine(_substr1)
            End If

            'Frank 20150318 I don't know why the N causes compiling error, so I group the string to _substr1 and then append it to sql script
            'If Not String.IsNullOrEmpty(CompanyName) Then .AppendLine(String.Format(" and a.quoteToName like N'%{0}%' ", CompanyName))
            If Not String.IsNullOrEmpty(CompanyName) Then
                Dim _substr2 As String = " and a.quoteToName like N'%" + CompanyName.Replace("'", "''") + "%' "
                .AppendLine(_substr2)
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then .AppendLine(String.Format(" and a.quoteToErpId like '%{0}%' ", CompanyID))
            If Not String.IsNullOrEmpty(Status) Then
                .AppendLine(String.Format(" and a.DOCSTATUS = '{0}' ", Status))
            Else
                .AppendLine(" and a.DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If
            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))

            If Not String.IsNullOrEmpty(OptyProjectName) Then .AppendLine(String.Format(" and c.Name like N'%{0}%' ", OptyProjectName))

            '.AppendLine(" order by quoteDate desc ")
            .AppendLine(" order by a.quoteDate desc, a.quoteNo desc ")

        End With
        Return sbSQLSTR.ToString()
    End Function

    Shared Function getMyQuoteRecordByAccountOwnerUSAOnline(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                    ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                               ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, Optional ByVal datePeriodType As Integer = 0) As String

        Dim _IsUSAonline = Role.IsAonlineUsa()
        Dim _IsUSAAC = Role.IsUSAACSales()

        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) a.quoteId, a.quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE, ")

            If _IsUSAAC Then
                .AppendLine(" (SELECT isnull(optyName, optyid) FROM optyQuote where optyQuote.quoteId=a.Quoteid) as siebelQuoteId, ")
            Else
                .AppendLine(" (SELECT optyid FROM optyQuote where optyQuote.quoteId=a.Quoteid) as siebelQuoteId, ")
            End If

            'Frank 2012/07/24
            .AppendLine(" IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")
            '.AppendLine(" FROM QuotationMaster a where (quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%')")

            'Ryan 20160726 Revised for pipeline status
            If _IsUSAonline Then
                .AppendLine(" ,(SELECT top 1 Probability FROM eQuotation.dbo.pipelineReport where pipelineReport.quoteId=a.Quoteid) as Probability ")
                .AppendLine(" ,case when (SELECT top 1 QuoteId FROM eQuotation.dbo.pipelineReport where pipelineReport.quoteId=a.Quoteid) is null then 0 else 1 end as isPipelined ")
            End If

            'Frank:20141106 The user is belong to SALES.IAG.USA who can only query the quote in AACQXXXXXX 
            .AppendLine(" FROM QuotationMaster a ")

            .AppendLine(" where 1=1 ")
            'If Role.IsUSAACSales() Then
            '    .AppendLine(" FROM QuotationMaster a where (quoteNo like 'AACQ%')")
            'Else
            '    .AppendLine(" FROM QuotationMaster a where (quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%')")
            'End If

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            Dim SB As New StringBuilder
            'SB.AppendLine(" Select DISTINCT b.Email From [ACLSQL6\SQL2008R2].MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK a  ")
            'SB.AppendLine(" Left join [ACLSQL6\SQL2008R2].MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
            'SB.AppendLine(" Inner join [ACLSQL6\SQL2008R2].MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID  ")
            SB.AppendLine(" Select DISTINCT b.ALIAS_EMAIL From MyAdvantechGlobal.dbo.AD_MEMBER a  ")
            SB.AppendLine(" Left join MyAdvantechGlobal.dbo.AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL  ")
            SB.AppendLine(" Inner join MyAdvantechGlobal.dbo.AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL  ")


            SB.AppendLine(" Where 1=1 ")
            'SB.AppendLine(" And b.Email not like '%O365.mail.onmicrosoft.%' ")
            SB.AppendLine(" And b.ALIAS_EMAIL not like '%O365.mail.onmicrosoft.%' ")

            Dim GroupA As New ArrayList, QuoteNoPref As New ArrayList, _QuotePref As String = String.Empty

            If Role.IsAonlineUsa() OrElse Role.IsMexicoAonlineSales() Then
                GroupA.Add("N'Aonline.USA'") : GroupA.Add("N'SALES.AISA.USA'") : GroupA.Add("N'Aonline.Mexico'")
                QuoteNoPref.Add("AUSQ") : QuoteNoPref.Add("AMXQ")
            End If

            If Role.IsAonlineUsaISystem() Then
                GroupA.Add("N'Aonline.USA.iSystem'")
                If Not QuoteNoPref.Contains("AUSQ") Then
                    QuoteNoPref.Add("AUSQ")
                End If
            End If

            'If Role.IsAonlineUsaIag() Then
            '    GroupA.Add("N'Aonline.USA.IAG'")
            '    If Not QuoteNoPref.Contains("AUSQ") Then
            '        QuoteNoPref.Add("AUSQ")
            '    End If
            'End If

            'If Role.IsUSAACSales() Then
            '    GroupA.Add("N'SALES.IAG.USA'")
            '    If Not QuoteNoPref.Contains("AACQ") Then
            '        QuoteNoPref.Add("AACQ")
            '    End If
            'End If

            If Role.IsAonlineUsaIag() Or Role.IsUSAACSales() Then
                GroupA.Add("N'Aonline.USA.IAG'")
                If Not QuoteNoPref.Contains("AUSQ") Then
                    QuoteNoPref.Add("AUSQ")
                End If
                GroupA.Add("N'SALES.IAG.USA'")
                If Not QuoteNoPref.Contains("AACQ") Then
                    QuoteNoPref.Add("AACQ")
                End If
            End If



            'ICC 2015/2/10 Add new group PAPS.eStore
            If Role.IsPAPSeStore() Then
                GroupA.Add("N'PAPS.eStore'")
                If Not QuoteNoPref.Contains("PUSQ") Then
                    QuoteNoPref.Add("PUSQ")
                End If
            End If


            If GroupA.Count > 0 Then
                'SB.AppendFormat(" and c.Name IN ({0}) ", String.Join(",", GroupA.ToArray()))
                SB.AppendFormat(" and c.Group_Name IN ({0}) ", String.Join(",", GroupA.ToArray()))

                'Frank 20150915 Get sales list from table ExEmployee 
                SB.AppendLine(" union ")
                SB.AppendLine(" Select ExEmployeeMail as Email From ExEmployee ")
                SB.AppendFormat(" Where MailGroup IN ({0}) ", String.Join(",", GroupA.ToArray()))
                SB.AppendLine(" and (LastWorkingDate <= GETDATE() and ExpirationDate >= GETDATE()) ")

                .AppendFormat(" and a.createdBy in ( {0}) ", SB.ToString())

                'Frank 20141127: Append quote prefix number limitation
                If QuoteNoPref.Count > 0 Then
                    For Each _quoteprefitem As String In QuoteNoPref
                        If Not String.IsNullOrEmpty(_QuotePref) Then
                            _QuotePref &= " or"
                        End If
                        _QuotePref &= " a.quoteNo like '" & _quoteprefitem & "%'"
                    Next
                    _QuotePref = " and (" & _QuotePref & ")"
                    .Append(_QuotePref)
                End If

            Else
                .AppendFormat(" and  1=2 ")
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and a.quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and a.quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and a.createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and a.createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If
            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and a.quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(user) Then
                .AppendFormat(" and a.createdBy like N'%{0}%' ", Trim(Replace(Replace(user, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and a.customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and a.quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and a.quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and a.DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and a.DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and a.createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If


            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))

            .AppendLine(" ORDER BY a.quoteDate desc,a.quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function

    ''' <summary>
    ''' For US AENC KA sales team only
    ''' </summary>
    ''' <param name="QuoteNo"></param>
    ''' <param name="user"></param>
    ''' <param name="CustomId"></param>
    ''' <param name="CompanyName"></param>
    ''' <param name="CompanyID"></param>
    ''' <param name="Status"></param>
    ''' <param name="QuoteIDList"></param>
    ''' <param name="QuoteCreateFromDate"></param>
    ''' <param name="QuoteCreateToDate"></param>
    ''' <param name="CreatedBy"></param>
    ''' <param name="OptyProjectName"></param>
    ''' <param name="datePeriodType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getMyQuoteRecordByAccountOwnerUSAENC(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                           ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, ByVal OptyProjectName As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) a.quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")
            .AppendLine(" ,isnull(c.NAME,b.optyId) as siebelQuoteId")
            '.AppendLine(" ,(SELECT isnull(so.NAME,qo.optyid) FROM optyQuote qo left join [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] so on qo.optyid=so.ROW_ID ")
            ' .AppendLine("  where qo.quoteId=a.Quoteid) as siebelQuoteId")
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a LEFT JOIN optyQuote b on a.quoteId=b.quoteId ")
            .AppendLine(" LEFT JOIN [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] C ON b.optyId=c.ROW_ID ")
            .AppendLine(" Where a.quoteNo like 'AENQ%' ")

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            Dim emailgroup As String = String.Empty, userlist As New List(Of String)
            Dim SB As New StringBuilder
            'SB.AppendLine(" select DISTINCT a.PrimarySmtpAddress from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
            'SB.AppendLine(" inner join ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID  ")
            'SB.AppendLine(" left join EAI_IDMAP f on b.Email=f.id_email  ")
            SB.AppendLine(" Select DISTINCT a.PrimarySmtpAddress From AD_MEMBER a  ")
            SB.AppendLine(" Left join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL  ")
            SB.AppendLine(" Inner join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL  ")
            SB.AppendLine(" left join EAI_IDMAP f on b.ALIAS_EMAIL=f.id_email  ")



            SB.AppendLine(" where 1=1 ")
            'SB.AppendLine(" and b.Email not like '%O365.mail.onmicrosoft.' ")
            SB.AppendLine(" and b.ALIAS_EMAIL not like '%O365.mail.onmicrosoft.' ")

            Dim _groupsetting As String = String.Empty
            If Pivot.CurrentProfile.GroupBelTo.Contains(("IS.AENC.USA").ToUpper) Then
                _groupsetting &= "N'IS.AENC.USA',N'Channel.AENC.USA',N'Sales.ECAC.Central.USA',N'Sales.ECAC.West.USA',N'Sales.NCIS.USA',N'IoT.Sales.USA'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Channel.AENC.USA").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Channel.AENC.USA'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ECAC.Central.USA").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ECAC.Central.USA'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ECAC.West.USA").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ECAC.West.USA'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.NCIS.USA").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.NCIS.USA'"
            End If

            If Not String.IsNullOrEmpty(_groupsetting) Then
                'SB.Append(" and c.Name IN (" & _groupsetting & ") ")
                SB.Append(" and c.Group_Name IN (" & _groupsetting & ") ")
            Else
                'Frank 2013/03/18:If user does not belong to any group then just search itself's quotes
                SB.Append(" and a.PrimarySmtpAddress = N'" & user & "' ")
            End If

            Dim DT As DataTable = tbOPBase.dbGetDataTable("B2B", SB.ToString())
            If DT.Rows.Count > 0 Then
                .AppendLine(" and a.createdBy in ( ")
                For i As Integer = 0 To DT.Rows.Count - 1
                    If i = DT.Rows.Count - 1 Then
                        .AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
                    Else
                        .AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
                    End If
                Next

                .AppendLine(" ) ")
            Else
                .AppendLine(String.Format(" and a.createdBy=N'{0}' ", user))
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(OptyProjectName) Then
                .AppendFormat(" and c.name like N'%{0}%' ", Trim(Replace(Replace(OptyProjectName, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then
                .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))
            End If

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function


    Shared Function getMyQuoteRecordByAccountOwnerCAPS(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                           ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, ByVal OptyProjectName As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) a.quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")
            .AppendLine(" ,isnull(c.NAME,b.optyId) as siebelQuoteId")
            '.AppendLine(" ,(SELECT isnull(so.NAME,qo.optyid) FROM optyQuote qo left join [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] so on qo.optyid=so.ROW_ID ")
            ' .AppendLine("  where qo.quoteId=a.Quoteid) as siebelQuoteId")
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a LEFT JOIN optyQuote b on a.quoteId=b.quoteId ")
            .AppendLine(" LEFT JOIN [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] C ON b.optyId=c.ROW_ID ")
            .AppendLine(" Where a.quoteNo like 'CAPQ%' ")

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            Dim emailgroup As String = String.Empty, userlist As New List(Of String)
            Dim SB As New StringBuilder
            'SB.AppendLine(" select DISTINCT a.PrimarySmtpAddress from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
            'SB.AppendLine(" inner join ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID  ")
            'SB.AppendLine(" left join EAI_IDMAP f on b.Email=f.id_email  ")

            SB.AppendLine(" Select DISTINCT a.PrimarySmtpAddress From AD_MEMBER a  ")
            SB.AppendLine(" Left join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL  ")
            SB.AppendLine(" Inner join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL  ")
            SB.AppendLine(" left join EAI_IDMAP f on b.ALIAS_EMAIL=f.id_email  ")


            SB.AppendLine(" where 1=1 ")
            'SB.AppendLine(" and b.Email not like '%O365.mail.onmicrosoft.' ")
            SB.AppendLine(" and b.ALIAS_EMAIL not like '%O365.mail.onmicrosoft.' ")

            Dim _groupsetting As String = String.Empty
            If Pivot.CurrentProfile.GroupBelTo.Contains(("CAPS.BD").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'CAPS.BD'"
            End If

            If Not String.IsNullOrEmpty(_groupsetting) Then
                'SB.Append(" and c.Name IN (" & _groupsetting & ") ")
                SB.Append(" and c.Group_Name IN (" & _groupsetting & ") ")
            Else
                'Frank 2013/03/18:If user does not belong to any group then just search itself's quotes
                SB.Append(" and a.PrimarySmtpAddress = N'" & user & "' ")
            End If

            Dim DT As DataTable = tbOPBase.dbGetDataTable("B2B", SB.ToString())
            If DT.Rows.Count > 0 Then
                .AppendLine(" and a.createdBy in ( ")
                For i As Integer = 0 To DT.Rows.Count - 1
                    If i = DT.Rows.Count - 1 Then
                        .AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
                    Else
                        .AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
                    End If
                Next

                .AppendLine(" ) ")
            Else
                .AppendLine(String.Format(" and a.createdBy=N'{0}' ", user))
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(OptyProjectName) Then
                .AppendFormat(" and c.name like N'%{0}%' ", Trim(Replace(Replace(OptyProjectName, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then
                .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))
            End If

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function

    Shared Function getMyQuoteRecordByAccountOwnerATW(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                           ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, ByVal OptyProjectName As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) a.quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")
            .AppendLine(" ,isnull(c.NAME,b.optyId) as siebelQuoteId")
            '.AppendLine(" ,(SELECT isnull(so.NAME,qo.optyid) FROM optyQuote qo left join [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] so on qo.optyid=so.ROW_ID ")
            ' .AppendLine("  where qo.quoteId=a.Quoteid) as siebelQuoteId")
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a LEFT JOIN optyQuote b on a.quoteId=b.quoteId ")
            .AppendLine(" LEFT JOIN [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] C ON b.optyId=c.ROW_ID ")
            .AppendLine(" Where a.quoteNo like 'TWQ%' ")

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            Dim emailgroup As String = String.Empty, userlist As New List(Of String)
            Dim SB As New StringBuilder
            'SB.AppendLine(" select DISTINCT a.PrimarySmtpAddress from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
            'SB.AppendLine(" inner join ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID  ")
            'SB.AppendLine(" left join EAI_IDMAP f on b.Email=f.id_email  ")
            SB.AppendLine(" Select DISTINCT a.PrimarySmtpAddress From AD_MEMBER a  ")
            SB.AppendLine(" Left join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL  ")
            SB.AppendLine(" Inner join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL  ")
            SB.AppendLine(" left join EAI_IDMAP f on b.ALIAS_EMAIL=f.id_email  ")



            SB.AppendLine(" where 1=1 ")
            'SB.AppendLine(" and b.Email not like '%.com' ")
            SB.AppendLine(" and b.ALIAS_EMAIL not like '%.com' ")

            Dim _groupsetting As String = String.Empty
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.KA.ES-KA1").ToUpper) OrElse
               Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.KA.ES-KA2").ToUpper) Then
                _groupsetting &= "N'Sales.ATW.KA.ES-KA1',N'Sales.ATW.KA.ES-KA2'"
            End If
            'If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.AOL-eP").ToUpper) Then
            '    If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
            '    _groupsetting &= "N'Sales.ATW.AOL-eP'"
            'End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.AOL-Neihu(IIoT)").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ATW.AOL-Neihu(IIoT)'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.AOL-EC").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ATW.AOL-EC'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("CallCenter.IA.ACL").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'CallCenter.IA.ACL'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.Emb'Core").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ATW.Emb''Core'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.KA.ES-KA3").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ATW.KA.ES-KA3'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.KA.ES-KA4").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ATW.KA.ES-KA4'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.KA.IAKA").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ATW.KA.IAKA'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("ATP.IA.ACL").ToUpper) OrElse
               Pivot.CurrentProfile.GroupBelTo.Contains(("MA.IA.ACL").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'ATP.IA.ACL','MA.IA.ACL'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("ACL.Sourcer").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'ACL.Sourcer'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.AOL-ATC(IIoT)").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ATW.AOL-ATC(IIoT)'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.AiS").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ATW.AiS'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.Ucity").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'Sales.ATW.Ucity'"
            End If
            If Not String.IsNullOrEmpty(_groupsetting) Then
                'SB.Append(" and c.Name IN (" & _groupsetting & ") ")
                SB.Append(" and c.Group_Name IN (" & _groupsetting & ") ")
            Else
                'Frank 2013/03/18:If user does not belong to any group then just search itself's quotes
                SB.Append(" and a.PrimarySmtpAddress = N'" & user & "' ")
            End If

            Dim DT As DataTable = tbOPBase.dbGetDataTable("B2B", SB.ToString())
            If DT.Rows.Count > 0 Then


                'Ryan 20170220 Comment below code out due to Dora Wu's multi group case
                '日後Dora Wu特例取消時，把下列註解程式碼回復，並把特例語法刪除即可      
                '.AppendLine(" and a.createdBy in ( ")
                'For i As Integer = 0 To DT.Rows.Count - 1
                '    If i = DT.Rows.Count - 1 Then
                '        .AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
                '    Else
                '        .AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
                '    End If
                'Next
                '.AppendLine(" ) ")
                'End Ryan 20170220 Comment out


                'Ryan 20170220 特例語法FOR Dora wu 共40行左右
                If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.AOL-ATC(IIoT)").ToUpper) OrElse
                    Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.AOL-EC").ToUpper) Then

                    Dim MailGroupforSelect As String = String.Empty
                    If Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.AOL-ATC(IIoT)").ToUpper) Then
                        MailGroupforSelect = "Sales.ATW.AOL-ATC(IIoT)"
                    ElseIf Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.ATW.AOL-EC").ToUpper) Then
                        MailGroupforSelect = "Sales.ATW.AOL-EC"
                    End If

                    .AppendLine(" And (")
                    .AppendLine(" a.createdBy in ( ")
                    For i As Integer = 0 To DT.Rows.Count - 1
                        If DT.Rows(i).Item("PrimarySmtpAddress").ToString.Equals("Dora.Wu@advantech.com.tw", StringComparison.OrdinalIgnoreCase) Then
                            Continue For
                        End If

                        If i = DT.Rows.Count - 1 Then
                            .AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
                        Else
                            .AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
                        End If
                    Next

                    .AppendLine(" ) ")
                    .AppendFormat(" Or (a.createdBy = N'Dora.Wu@advantech.com.tw' and a.KEYPERSON = '{0}')", MailGroupforSelect)
                    .AppendLine(" ) ")
                Else
                    .AppendLine(" and a.createdBy in ( ")
                    For i As Integer = 0 To DT.Rows.Count - 1
                        If i = DT.Rows.Count - 1 Then
                            .AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
                        Else
                            .AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
                        End If
                    Next
                    .AppendLine(" ) ")
                End If
                'End Ryan 20170220 特例語法FOR Dora wu

            Else
                .AppendLine(String.Format(" and a.createdBy=N'{0}' ", user))
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(OptyProjectName) Then
                .AppendFormat(" and c.name like N'%{0}%' ", Trim(Replace(Replace(OptyProjectName, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then
                .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))
            End If

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function


    Shared Function getMyQuoteRecordByAccountOwnerAKR(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                            ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                       ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, ByVal OptyProjectName As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) a.quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy,  ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")
            .AppendLine(" ,isnull(c.NAME,b.optyId) as siebelQuoteId")
            '.AppendLine(" ,(SELECT isnull(so.NAME,qo.optyid) FROM optyQuote qo left join [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] so on qo.optyid=so.ROW_ID ")
            ' .AppendLine("  where qo.quoteId=a.Quoteid) as siebelQuoteId")
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a LEFT JOIN optyQuote b on a.quoteId=b.quoteId ")
            .AppendLine(" LEFT JOIN [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] C ON b.optyId=c.ROW_ID ")
            .AppendLine(" Where a.quoteNo like 'AKRQ%' ")

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            Dim emailgroup As String = String.Empty, userlist As New List(Of String)
            Dim SB As New StringBuilder

            'SB.AppendLine(" Select DISTINCT a.PrimarySmtpAddress From AD_MEMBER a  ")
            'SB.AppendLine(" Left join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL  ")
            'SB.AppendLine(" Inner join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL  ")
            'SB.AppendLine(" left join EAI_IDMAP f on b.ALIAS_EMAIL=f.id_email  ")


            'SB.AppendLine(" where 1=1 ")
            'SB.AppendLine(" and b.ALIAS_EMAIL not like '%.com' ")

            'Dim _groupsetting As String = String.Empty
            'If Pivot.CurrentProfile.GroupBelTo.Contains(("SALES.AKR").ToUpper) Then
            '    If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
            '    _groupsetting &= "N'SALES.AKR'"
            'End If
            'If Not String.IsNullOrEmpty(_groupsetting) Then
            '    'SB.Append(" and c.Name IN (" & _groupsetting & ") ")
            '    SB.Append(" and c.Group_Name IN (" & _groupsetting & ") ")
            'Else
            '    'Frank 2013/03/18:If user does not belong to any group then just search itself's quotes
            '    SB.Append(" and a.PrimarySmtpAddress = N'" & user & "' ")
            'End If

            SB.AppendLine(" SELECT Sales_email as PrimarySmtpAddress FROM AKR_Sales_List ")
            SB.AppendLine(" Where Report_to=(select Report_to from AKR_Sales_List where Sales_email='" & user & "')")
            SB.AppendLine(" or Report_to='" & user & "'")
            SB.AppendLine(" union ")
            SB.AppendLine(" SELECT Report_to FROM AKR_Sales_List ")
            SB.AppendLine(" where Sales_email='" & user & "' or Report_to='" & user & "' ")

            Dim DT As DataTable = tbOPBase.dbGetDataTable("EQ", SB.ToString())
            If user.Equals("Nadia.Kim@advantech.co.kr", StringComparison.OrdinalIgnoreCase) Then
                ' Ryan 20170803 Nadia is able to see all AKR quotations, no need to add any conditions to created by

            ElseIf DT.Rows.Count > 0 Then
                .AppendLine(" and a.createdBy in ( ")
                For i As Integer = 0 To DT.Rows.Count - 1
                    If i = DT.Rows.Count - 1 Then
                        .AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
                    Else
                        .AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
                    End If
                Next

                .AppendLine(" ) ")
            Else
                .AppendLine(String.Format(" and a.createdBy=N'{0}' ", user))
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(OptyProjectName) Then
                .AppendFormat(" and c.name like N'%{0}%' ", Trim(Replace(Replace(OptyProjectName, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then
                .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))
            End If

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function

    Shared Function getMyQuoteRecordByAccountOwnerAKRV2(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                            ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                       ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, ByVal OptyProjectName As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) a.quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")
            .AppendLine(" ,isnull(c.NAME,b.optyId) as siebelQuoteId")
            '.AppendLine(" ,(SELECT isnull(so.NAME,qo.optyid) FROM optyQuote qo left join [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] so on qo.optyid=so.ROW_ID ")
            ' .AppendLine("  where qo.quoteId=a.Quoteid) as siebelQuoteId")
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a LEFT JOIN optyQuote b on a.quoteId=b.quoteId ")
            .AppendLine(" LEFT JOIN [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] C ON b.optyId=c.ROW_ID ")
            .AppendLine(" Where a.quoteNo like 'AKRQ%' ")

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            Dim emailgroup As String = String.Empty, userlist As New List(Of String)

            'Ryan 20170822 AKR will use new table to get team's quotation visibility
            Dim SB As New StringBuilder
            SB.AppendLine(" select distinct (select EMAIL from MyAdvantechGlobal.dbo.SAP_EMPLOYEE where SALES_CODE = b.SalesID) as EMAIL ")
            SB.AppendLine(" From MyAdvantechGlobal.dbo.SAP_EMPLOYEE a inner join MyAdvantechGlobal.dbo.AJP_SalesMapping b on a.SALES_CODE = b.InsideSalesID or a.SALES_CODE = b.SalesID where EMAIL = '" + user + "' ")
            SB.AppendLine(" UNION ")
            SB.AppendLine(" select distinct (select EMAIL from MyAdvantechGlobal.dbo.SAP_EMPLOYEE where SALES_CODE = b.InsideSalesID) as EMAIL ")
            SB.AppendLine(" From MyAdvantechGlobal.dbo.SAP_EMPLOYEE a inner join MyAdvantechGlobal.dbo.AJP_SalesMapping b on a.SALES_CODE = b.InsideSalesID or a.SALES_CODE = b.SalesID where EMAIL = '" + user + "' ")
            SB.AppendLine(" UNION ")
            SB.AppendLine(" SELECT Sales_email as EMAIL FROM AKR_Sales_List ")
            SB.AppendLine(" Where Report_to=(select Report_to from AKR_Sales_List where Sales_email='" & user & "')")
            SB.AppendLine(" or Report_to='" & user & "'")
            SB.AppendLine(" UNION ")
            SB.AppendLine(" SELECT Report_to as EMAIL FROM AKR_Sales_List ")
            SB.AppendLine(" where Sales_email='" & user & "' or Report_to='" & user & "' ")
            Dim DT As DataTable = tbOPBase.dbGetDataTable("EQ", SB.ToString())

            If user.Equals("Nadia.Kim@advantech.co.kr", StringComparison.OrdinalIgnoreCase) Then
                ' Ryan 20170803 Nadia is able to see all AKR quotations, no need to add any conditions to created by

            ElseIf DT.Rows.Count > 0 Then
                .AppendLine(" and ( a.createdBy in ( ")
                .AppendFormat("N'{0}'", user)
                For i As Integer = 0 To DT.Rows.Count - 1
                    .AppendFormat(", N'{0}'", DT.Rows(i).Item("EMAIL"))
                Next
                .AppendFormat(" ) or a.salesEmail=N'{0}' ) ", user)
            Else
                .AppendLine(String.Format(" and a.createdBy=N'{0}' ", user))
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(OptyProjectName) Then
                .AppendFormat(" and c.name like N'%{0}%' ", Trim(Replace(Replace(OptyProjectName, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then
                .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))
            End If

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function


    Shared Function getMyQuoteRecordByAccountOwnerHQDC(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                        ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                   ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, ByVal OptyProjectName As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) a.quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")
            .AppendLine(" ,isnull(c.NAME,b.optyId) as siebelQuoteId")
            '.AppendLine(" ,(SELECT isnull(so.NAME,qo.optyid) FROM optyQuote qo left join [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] so on qo.optyid=so.ROW_ID ")
            ' .AppendLine("  where qo.quoteId=a.Quoteid) as siebelQuoteId")
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a LEFT JOIN optyQuote b on a.quoteId=b.quoteId ")
            .AppendLine(" LEFT JOIN [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] C ON b.optyId=c.ROW_ID ")
            .AppendLine(" Where (a.quoteNo like 'AIAQ%' or a.quoteNo like 'AIEQ%') ")

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            Dim emailgroup As String = String.Empty, userlist As New List(Of String)
            Dim SB As New StringBuilder
            'SB.AppendLine(" select DISTINCT a.PrimarySmtpAddress from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
            'SB.AppendLine(" inner join ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID  ")
            'SB.AppendLine(" left join EAI_IDMAP f on b.Email=f.id_email  ")
            SB.AppendLine(" Select DISTINCT a.PrimarySmtpAddress From AD_MEMBER a  ")
            SB.AppendLine(" Left join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL  ")
            SB.AppendLine(" Inner join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL  ")
            SB.AppendLine(" left join EAI_IDMAP f on b.ALIAS_EMAIL=f.id_email  ")


            SB.AppendLine(" where 1=1 ")

            Dim _groupsetting As String = String.Empty
            If Pivot.CurrentProfile.GroupBelTo.Contains(("IA.eSales").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'IA.eSales'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("InterCon.Embedded").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'InterCon.Embedded'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("InterCon.iService").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'InterCon.iService'"
            End If
            If Pivot.CurrentProfile.GroupBelTo.Contains(("InterCon.iLogistic").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'InterCon.iLogistic'"
            End If
            If Not String.IsNullOrEmpty(_groupsetting) Then
                'SB.Append(" and c.Name IN (" & _groupsetting & ") ")
                SB.Append(" and c.Group_Name IN (" & _groupsetting & ") ")
            Else
                'Frank 2013/03/18:If user does not belong to any group then just search itself's quotes
                SB.Append(" and a.PrimarySmtpAddress = N'" & user & "' ")
            End If

            Dim DT As DataTable = tbOPBase.dbGetDataTable("B2B", SB.ToString())
            If DT.Rows.Count > 0 Then
                .AppendLine(" and a.createdBy in ( ")
                For i As Integer = 0 To DT.Rows.Count - 1
                    If i = DT.Rows.Count - 1 Then
                        .AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
                    Else
                        .AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
                    End If
                Next

                .AppendLine(" ) ")
            Else
                .AppendLine(String.Format(" and a.createdBy=N'{0}' ", user))
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(OptyProjectName) Then
                .AppendFormat(" and c.name like N'%{0}%' ", Trim(Replace(Replace(OptyProjectName, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then
                .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))
            End If

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function


    Shared Function getMyQuoteRecordByAccountOwnerABR(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                        ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                   ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, ByVal OptyProjectName As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) a.quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")
            .AppendLine(" ,isnull(c.NAME,b.optyId) as siebelQuoteId")
            '.AppendLine(" ,(SELECT isnull(so.NAME,qo.optyid) FROM optyQuote qo left join [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] so on qo.optyid=so.ROW_ID ")
            ' .AppendLine("  where qo.quoteId=a.Quoteid) as siebelQuoteId")
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a LEFT JOIN optyQuote b on a.quoteId=b.quoteId ")
            .AppendLine(" LEFT JOIN [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] C ON b.optyId=c.ROW_ID ")
            .AppendLine(" Where a.quoteNo like 'ABRQ%' ")

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            'Frank 這是暫時的解決方式，先用.com.br來判斷是否為ABR的員工，因為在登入時也只有判斷ez_employee中的RBU Name是否
            '為ABR，沒有通過mail group來判斷，所以這邊只認creatdby like '%@advantech.com.br'即可
            .AppendLine(String.Format(" and a.createdBy like N'{0}' ", "%@advantech.com.br"))
            'Dim emailgroup As String = String.Empty, userlist As New List(Of String)
            'Dim SB As New StringBuilder
            'SB.AppendLine(" select DISTINCT a.PrimarySmtpAddress from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
            'SB.AppendLine(" inner join ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID  ")
            'SB.AppendLine(" left join EAI_IDMAP f on b.Email=f.id_email  ")

            'SB.AppendLine(" where 1=1 ")
            'SB.AppendLine(" and b.Email not like '%.com' ")

            'Dim _groupsetting As String = String.Empty
            'If Pivot.CurrentProfile.GroupBelTo.Contains(("IA.eSales").ToUpper) Then
            '    If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
            '    _groupsetting &= "N'IA.eSales'"
            'End If
            'If Not String.IsNullOrEmpty(_groupsetting) Then
            '    SB.Append(" and c.Name IN (" & _groupsetting & ") ")
            'Else
            '    'Frank 2013/03/18:If user does not belong to any group then just search itself's quotes
            '    SB.Append(" and a.PrimarySmtpAddress = N'" & user & "' ")
            'End If

            'Dim DT As DataTable = tbOPBase.dbGetDataTable("B2B", SB.ToString())
            'If DT.Rows.Count > 0 Then
            '    .AppendLine(" and a.createdBy in ( ")
            '    For i As Integer = 0 To DT.Rows.Count - 1
            '        If i = DT.Rows.Count - 1 Then
            '            .AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
            '        Else
            '            .AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
            '        End If
            '    Next

            '    .AppendLine(" ) ")
            'Else
            '    .AppendLine(String.Format(" and a.createdBy=N'{0}' ", user))
            'End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(OptyProjectName) Then
                .AppendFormat(" and c.name like N'%{0}%' ", Trim(Replace(Replace(OptyProjectName, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then
                .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))
            End If

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function

    Shared Function getMyQuoteRecordByAccountOwnerAAU(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                    ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
               ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, ByVal OptyProjectName As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) a.quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")
            .AppendLine(" ,isnull(c.NAME,b.optyId) as siebelQuoteId")
            '.AppendLine(" ,(SELECT isnull(so.NAME,qo.optyid) FROM optyQuote qo left join [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] so on qo.optyid=so.ROW_ID ")
            ' .AppendLine("  where qo.quoteId=a.Quoteid) as siebelQuoteId")
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a LEFT JOIN optyQuote b on a.quoteId=b.quoteId ")
            .AppendLine(" LEFT JOIN [MyAdvantechGlobal].[dbo].[SIEBEL_OPPORTUNITY] C ON b.optyId=c.ROW_ID ")
            .AppendLine(" Where a.quoteNo like 'AAUQ%' ")

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            Dim emailgroup As String = String.Empty, userlist As New List(Of String)
            Dim SB As New StringBuilder
            'SB.AppendLine(" select DISTINCT a.PrimarySmtpAddress from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
            'SB.AppendLine(" select DISTINCT b.Email from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
            'SB.AppendLine(" inner join ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID  ")
            'SB.AppendLine(" left join EAI_IDMAP f on b.Email=f.id_email  ")
            SB.AppendLine(" Select DISTINCT b.ALIAS_EMAIL From AD_MEMBER a  ")
            SB.AppendLine(" Left join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL  ")
            SB.AppendLine(" Inner join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL  ")
            SB.AppendLine(" left join EAI_IDMAP f on b.ALIAS_EMAIL=f.id_email  ")


            SB.AppendLine(" where 1=1 ")
            'SB.AppendLine(" and b.Email not like '%.com' ")
            'SB.AppendLine(" and b.Email not like '%@advantechO365%' ")
            SB.AppendLine(" and b.ALIAS_EMAIL not like '%@advantechO365%' ")

            Dim _groupsetting As String = String.Empty
            If Pivot.CurrentProfile.GroupBelTo.Contains(("SALES.AAU").ToUpper) Then
                If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                _groupsetting &= "N'SALES.AAU'"
            End If
            If Not String.IsNullOrEmpty(_groupsetting) Then
                'SB.Append(" and c.Name IN (" & _groupsetting & ") ")
                SB.Append(" and c.Group_Name IN (" & _groupsetting & ") ")
            Else
                'Frank 2013/03/18:If user does not belong to any group then just search itself's quotes
                SB.Append(" and a.PrimarySmtpAddress = N'" & user & "' ")
            End If

            Dim DT As DataTable = tbOPBase.dbGetDataTable("B2B", SB.ToString())
            If DT.Rows.Count > 0 Then
                .AppendLine(" and a.createdBy in ( ")
                For i As Integer = 0 To DT.Rows.Count - 1
                    If i = DT.Rows.Count - 1 Then
                        '.AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
                        .AppendFormat("N'{0}'", DT.Rows(i).Item("Email"))
                    Else
                        '.AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
                        .AppendFormat("N'{0}',", DT.Rows(i).Item("Email"))
                    End If
                Next

                .AppendLine(" ) ")
            Else
                .AppendLine(String.Format(" and a.createdBy=N'{0}' ", user))
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(OptyProjectName) Then
                .AppendFormat(" and c.name like N'%{0}%' ", Trim(Replace(Replace(OptyProjectName, "'", ""), "*", "%")))
            End If

            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then
                .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))
            End If

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function

    Shared Function getMyQuoteRecordByAccountOwnerAJP(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                    ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                               ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy,  ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE,(SELECT optyid FROM optyQuote where optyQuote.quoteId=a.Quoteid) as siebelQuoteId,")
            .AppendLine(" IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a ")
            .AppendLine(" Where a.quoteNo like 'AJPQ%' ")

            'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
            .AppendLine(" and a.Active=1 ")

            Dim emailgroup As String = String.Empty, userlist As New List(Of String)
            Dim SB As New StringBuilder
            'SB.Append(" select DISTINCT  a.PrimarySmtpAddress from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
            'SB.Append(" inner join ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID  ")

            SB.AppendLine(" Select DISTINCT a.PrimarySmtpAddress From AD_MEMBER a  ")
            SB.AppendLine(" Left join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL  ")
            SB.AppendLine(" Inner join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL  ")


            SB.Append(" where 1=1 ")

            If (Pivot.CurrentProfile.GroupBelTo.Contains(("ajp_callcenter").ToUpper) Or Role.IsJPAonlineSales()) Then
                'SB.Append(" and c.Name IN (N'AJP_EC',N'AJP_ES', N'AJP_IA', N'ajp_callcenter') ")
                SB.Append(" and c.Group_Name IN (N'AJP_EC',N'AJP_ES', N'AJP_IA', N'ajp_callcenter') ")
            Else

                Dim _groupsetting As String = String.Empty
                If Pivot.CurrentProfile.GroupBelTo.Contains(("AJP_EC").ToUpper) Then
                    _groupsetting &= "N'AJP_EC'"
                End If
                If Pivot.CurrentProfile.GroupBelTo.Contains(("AJP_ES").ToUpper) Then
                    If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                    _groupsetting &= "N'AJP_ES'"
                End If
                If Pivot.CurrentProfile.GroupBelTo.Contains(("AJP_IA").ToUpper) Then
                    If Not String.IsNullOrEmpty(_groupsetting) Then _groupsetting &= ", "
                    _groupsetting &= "N'AJP_IA'"
                End If

                If Not String.IsNullOrEmpty(_groupsetting) Then
                    'SB.Append(" and c.Name IN (" & _groupsetting & ") ")
                    SB.Append(" and c.Group_Name IN (" & _groupsetting & ") ")
                Else
                    'Frank 2013/03/18:If user is not under 'AJP_EC',N'AJP_ES', N'AJP_IA', N'ajp_callcenter'
                    ', then just search itself
                    SB.Append(" and a.PrimarySmtpAddress = N'" & user & "' ")
                End If

            End If



            'If (Role.IsInMailGroup("ajp_callcenter", user) Or Role.IsJPPowerUser(user)) Then
            '    SB.Append(" and c.Name IN (N'AJP_EC',N'AJP_ES', N'AJP_IA', N'ajp_callcenter') ")
            'ElseIf Role.IsInMailGroup("AJP_EC", user) Then
            '    SB.Append(" and c.Name = N'AJP_EC' ")
            'ElseIf Role.IsInMailGroup("AJP_ES", user) Then
            '    SB.Append(" and c.Name = N'AJP_ES' ")
            'ElseIf Role.IsInMailGroup("AJP_IA", user) Then
            '    SB.Append(" and c.Name = N'AJP_IA' ")
            'Else
            '    'Frank 2013/03/18:If user is not under 'AJP_EC',N'AJP_ES', N'AJP_IA', N'ajp_callcenter'
            '    ', then just search itself
            '    SB.Append(" and a.PrimarySmtpAddress = N'" & user & "' ")
            'End If

            Dim DT As DataTable = tbOPBase.dbGetDataTable("B2B", SB.ToString())
            If DT.Rows.Count > 0 Then
                .AppendLine(" and a.createdBy in ( ")
                For i As Integer = 0 To DT.Rows.Count - 1
                    If i = DT.Rows.Count - 1 Then
                        .AppendFormat("N'{0}'", DT.Rows(i).Item("PrimarySmtpAddress"))
                    Else
                        .AppendFormat("N'{0}',", DT.Rows(i).Item("PrimarySmtpAddress"))
                    End If
                Next

                If Role.IsJPAonlineSales() Then
                    If DT.Rows.Count > 0 Then .Append(",")
                    .AppendFormat("N'{0}',", "Tanya.Lin@advantech.com.tw")
                    .AppendFormat("N'{0}',", "Mary.Huang@advantech.com.tw")
                    .AppendFormat("N'{0}'", "manami.doi@advantech.com")
                End If

                .AppendLine(" ) ")
            Else
                .AppendLine(String.Format(" and a.createdBy=N'{0}' ", user))
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            'If Not String.IsNullOrEmpty(user) Then
            '    .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(user, "'", "''"), "*", "%")))
            'End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If


            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function

    Shared Function getMyQuoteRecordByAccountOwnerAJPV2(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                    ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                               ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, Optional ByVal datePeriodType As Integer = 0) As String
        Dim sqlSb As New System.Text.StringBuilder
        With sqlSb
            .AppendLine(" SELECT TOP (600) quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
            .AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
            .AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
            .AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
            .AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE,(SELECT optyid FROM optyQuote where optyQuote.quoteId=a.Quoteid) as siebelQuoteId,")
            .AppendLine(" IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log ")
            .AppendLine(" ,DocReg,qstatus ")

            .AppendLine(" FROM QuotationMaster a ")
            .AppendLine(" Where a.quoteNo like 'AJPQ%' ")
            .AppendLine(" and a.Active=1 ")

            Dim emailgroup As String = String.Empty, userlist As New List(Of String)
            Dim SB As New StringBuilder
            SB.AppendLine(" select distinct (select EMAIL from SAP_EMPLOYEE where SALES_CODE = b.SalesID) as EMAIL ")
            SB.AppendLine(" From SAP_EMPLOYEE a inner join AJP_SalesMapping b on a.SALES_CODE = b.InsideSalesID or a.SALES_CODE = b.SalesID where EMAIL = '" + user + "' ")
            SB.AppendLine(" UNION ")
            SB.AppendLine(" select distinct (select EMAIL from SAP_EMPLOYEE where SALES_CODE = b.InsideSalesID) as EMAIL ")
            SB.AppendLine(" From SAP_EMPLOYEE a inner join AJP_SalesMapping b on a.SALES_CODE = b.InsideSalesID or a.SALES_CODE = b.SalesID where EMAIL = '" + user + "' ")

            Dim DT As DataTable = tbOPBase.dbGetDataTable("B2B", SB.ToString())
            If user.Equals("yc.liu@advantech.com", StringComparison.OrdinalIgnoreCase) Then
                ' Ryan 20170615 YC is able to see all AJP quotations, no need to add any conditions to created by

            ElseIf DT.Rows.Count > 0 Then
                .AppendLine(" and ( a.createdBy in ( ")
                .AppendFormat("N'{0}'", user)
                For i As Integer = 0 To DT.Rows.Count - 1
                    .AppendFormat(", N'{0}'", DT.Rows(i).Item("EMAIL"))
                Next
                .AppendFormat(" ) or a.salesEmail=N'{0}' ) ", user)
            Else
                .AppendLine(String.Format("and ( a.createdBy=N'{0}' or a.salesEmail=N'{0}' ) ", user))
            End If

            If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            Else
                If QuoteCreateFromDate <> Nothing Then
                    .AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
                End If
                If QuoteCreateToDate <> Nothing Then
                    .AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
                End If
            End If


            If Not String.IsNullOrEmpty(QuoteNo) Then
                .AppendFormat(" and quoteNo like N'%{0}%' ", Trim(Replace(Replace(QuoteNo, "'", ""), "*", "%")))
            End If
            'If Not String.IsNullOrEmpty(user) Then
            '    .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(user, "'", "''"), "*", "%")))
            'End If
            If Not String.IsNullOrEmpty(CustomId) Then
                .AppendFormat(" and customId like N'%{0}%' ", Trim(Replace(Replace(CustomId, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyName) Then
                .AppendFormat(" and quoteToName like N'%{0}%' ", Trim(Replace(Replace(CompanyName, "'", "''"), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(CompanyID) Then
                .AppendFormat(" and quoteToErpId like N'%{0}%' ", Trim(Replace(Replace(CompanyID, "'", ""), "*", "%")))
            End If
            If Not String.IsNullOrEmpty(Status) Then
                .AppendFormat(" and DOCSTATUS =N'{0}' ", Trim(Replace(Replace(Status, "'", ""), "*", "")))
            Else
                .AppendLine(" and DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "'")
            End If

            If Not String.IsNullOrEmpty(CreatedBy) Then
                .AppendFormat(" and createdBy like N'%{0}%' ", Trim(Replace(Replace(CreatedBy, "'", ""), "*", "%")))
            End If


            If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then .AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))

            .AppendLine(" ORDER BY quoteDate desc,quoteNo DESC ")
        End With
        Return sqlSb.ToString()
    End Function

    Shared Function getMyQuoteRecordByAccountOwner(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                    ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                               ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, Optional ByVal datePeriodType As Integer = 0) As String

        '     Dim SQLSTR As String = String.Format(" SELECT top 20 *,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log FROM {0} AS a WHERE (createdBy='{6}' " & _
        '" OR (quoteToRowId IN ( SELECT DISTINCT ACCOUNT_ROW_ID FROM SIEBEL_ACCOUNT_OWNER_EMAIL AS z WHERE (ACCOUNT_ROW_ID IS NOT NULL) AND (EMAIL_ADDRESS = '{6}') ) ) " & _
        '" OR ( quoteToErpId IN ( SELECT DISTINCT COMPANY_ID FROM SAP_COMPANY_OWNER_EMAIL AS z WHERE (COMPANY_ID IS NOT NULL) AND (email = '{6}') ) )) " & _
        '" and a.CustomId like N'%{1}%' and a.quoteToName like N'%{2}%' and a.quoteToErpId like N'%{3}%' and a.qStatus like N'%{4}%' and a.quoteId like '%{5}%'" & _
        '" ORDER BY quoteDate DESC, quoteToRowId ", myQM.tb, CustomId, CompanyName, CompanyID, Status, QuoteID, user)

        Dim _sql As New StringBuilder
        '_sql.Append("SELECT top 20 *")

        _sql.AppendLine(" SELECT TOP (600) quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
        _sql.AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
        _sql.AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy, LastUpdatedDate, ")
        _sql.AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
        _sql.AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")


        _sql.Append(",IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log")
        _sql.Append(",(SELECT optyid FROM optyQuote where optyQuote.quoteId=a.Quoteid) as siebelQuoteId,DocReg,qstatus ")
        _sql.Append(String.Format(" FROM {0} AS a ", "QuotationMaster"))
        _sql.Append(String.Format(" WHERE (createdBy='{0}' ", user))
        _sql.Append(String.Format(" OR (quoteToRowId IN ( SELECT DISTINCT ACCOUNT_ROW_ID FROM SIEBEL_ACCOUNT_OWNER_EMAIL AS z WHERE (ACCOUNT_ROW_ID IS NOT NULL) AND (EMAIL_ADDRESS = '{0}') ) ) ", user))
        _sql.Append(String.Format(" OR ( quoteToErpId IN ( SELECT DISTINCT COMPANY_ID FROM SAP_COMPANY_OWNER_EMAIL AS z WHERE (COMPANY_ID IS NOT NULL) AND (email = '{0}') ) )) ", user))

        'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
        _sql.AppendLine(" and Active=1 ")

        'Frank 2012/08/29
        '_sql.AppendLine(" and a.quoteDate between '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' and '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
        'Frank 2012/10/22
        'If QuoteCreateFromDate <> Nothing Then
        '    _sql.AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
        'End If
        'If QuoteCreateToDate <> Nothing Then
        '    _sql.AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
        'End If
        If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
            If QuoteCreateFromDate <> Nothing Then
                _sql.AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
            End If
            If QuoteCreateToDate <> Nothing Then
                _sql.AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
            End If
        Else
            If QuoteCreateFromDate <> Nothing Then
                _sql.AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
            End If
            If QuoteCreateToDate <> Nothing Then
                _sql.AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
            End If
        End If


        If Not String.IsNullOrEmpty(CustomId) Then
            _sql.Append(String.Format(" and a.CustomId like N'%{0}%'", CustomId))
        End If
        If Not String.IsNullOrEmpty(CompanyName) Then
            _sql.Append(String.Format(" and a.quoteToName like N'%{0}%'", CompanyName))
        End If
        If Not String.IsNullOrEmpty(CompanyID) Then
            _sql.Append(String.Format(" and a.quoteToErpId like N'%{0}%'", CompanyID))
        End If
        If Not String.IsNullOrEmpty(Status) Then
            _sql.Append(String.Format(" and a.DOCSTATUS = N'{0}'", Status))
        Else
            _sql.Append(String.Format(" and a.DOCSTATUS <> N'{0}'", CInt(COMM.Fixer.eDocStatus.QDELETED)))
        End If

        If Not String.IsNullOrEmpty(CreatedBy) Then
            _sql.AppendFormat(String.Format(" and createdBy like N'%{0}%' ", CreatedBy))
        End If


        If Not String.IsNullOrEmpty(QuoteNo) Then
            _sql.Append(String.Format(" and a.quoteNo like '%{0}%'", QuoteNo))
        End If
        If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then _sql.AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))

        _sql.AppendLine(" order by a.quoteDate desc,a.quoteNo desc ")

        'Return SQLSTR
        Return _sql.ToString

    End Function
    Shared Function getQuotationAll(ByVal QuoteID As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                   ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                              ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, Optional ByVal datePeriodType As Integer = 0) As String

        '     Dim SQLSTR As String = String.Format(" SELECT top 20 *,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log FROM {0} AS a WHERE (createdBy='{6}' " & _
        '" OR (quoteToRowId IN ( SELECT DISTINCT ACCOUNT_ROW_ID FROM SIEBEL_ACCOUNT_OWNER_EMAIL AS z WHERE (ACCOUNT_ROW_ID IS NOT NULL) AND (EMAIL_ADDRESS = '{6}') ) ) " & _
        '" OR ( quoteToErpId IN ( SELECT DISTINCT COMPANY_ID FROM SAP_COMPANY_OWNER_EMAIL AS z WHERE (COMPANY_ID IS NOT NULL) AND (email = '{6}') ) )) " & _
        '" and a.CustomId like N'%{1}%' and a.quoteToName like N'%{2}%' and a.quoteToErpId like N'%{3}%' and a.qStatus like N'%{4}%' and a.quoteId like '%{5}%'" & _
        '" ORDER BY quoteDate DESC, quoteToRowId ", myQM.tb, CustomId, CompanyName, CompanyID, Status, QuoteID, user)

        Dim _sql As New StringBuilder
        '_sql.Append("SELECT top 20 *")

        _sql.AppendLine(" SELECT TOP (600) quoteId, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
        _sql.AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
        _sql.AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy,  ")
        _sql.AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
        _sql.AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")


        _sql.Append(",IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log")
        _sql.Append(",(SELECT optyid FROM optyQuote where optyQuote.quoteId=a.Quoteid) as siebelQuoteId,docReg ")
        _sql.Append(String.Format(" FROM {0} AS a ", "QuotationMaster"))
        _sql.Append(String.Format(" WHERE 1=1 ", user))

        'Frank 2012/08/29
        '_sql.AppendLine(" and a.quoteDate between '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' and '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
        'Frank 2012/10/22
        'If QuoteCreateFromDate <> Nothing Then
        '    _sql.AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
        'End If
        'If QuoteCreateToDate <> Nothing Then
        '    _sql.AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
        'End If
        If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
            If QuoteCreateFromDate <> Nothing Then
                _sql.AppendLine(" and quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
            End If
            If QuoteCreateToDate <> Nothing Then
                _sql.AppendLine(" and quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
            End If
        Else
            If QuoteCreateFromDate <> Nothing Then
                _sql.AppendLine(" and createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
            End If
            If QuoteCreateToDate <> Nothing Then
                _sql.AppendLine(" and createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
            End If
        End If


        If Not String.IsNullOrEmpty(CustomId) Then
            _sql.Append(String.Format(" and a.CustomId like N'%{0}%'", CustomId))
        End If
        If Not String.IsNullOrEmpty(CompanyName) Then
            _sql.Append(String.Format(" and a.quoteToName like N'%{0}%'", CompanyName))
        End If
        If Not String.IsNullOrEmpty(CompanyID) Then
            _sql.Append(String.Format(" and a.quoteToErpId like N'%{0}%'", CompanyID))
        End If
        If Not String.IsNullOrEmpty(Status) Then
            _sql.Append(String.Format(" and a.DOCSTATUS = '{0}'", Status))
        Else
            _sql.Append(String.Format(" and a.DOCSTATUS <> N'{0}'", CInt(COMM.Fixer.eDocStatus.QDELETED)))
        End If

        If Not String.IsNullOrEmpty(CreatedBy) Then
            _sql.AppendFormat(String.Format(" and createdBy like N'%{0}%' ", CreatedBy))
        End If


        If Not String.IsNullOrEmpty(QuoteID) Then
            _sql.Append(String.Format(" and a.quoteNo like '%{0}%'", QuoteID))
        End If
        If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then _sql.AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))

        _sql.AppendLine(" order by a.quoteDate desc,a.quoteid desc ")

        'Return SQLSTR
        Return _sql.ToString

    End Function
    Shared Function SSOIsExists(ByVal email As String) As Boolean
        Dim ws1 As New SSO.MembershipWebservice
        ws1.Timeout = -1
        If ws1.isExist(LCase(email.Trim), "My") = True Then
            ws1.Dispose()
            Return True
        End If
        ws1.Dispose()
        Return False
    End Function
    Shared Function SSORegister(ByVal Company As String, ByVal email As String, ByVal password As String, ByVal msg As String) As Boolean
        Dim ws1 As New SSO.MembershipWebservice

        Dim p As New SSO.SSOUSER
        With p
            p.company_id = Company : p.erpid = Company
            p.email_addr = email.Trim
            p.login_password = password.Trim 'Util.GetMD5Checksum(LCase(email.Text.Trim) + "|" + password.Text.Trim)
            'p.AccountID = account_row_id : p.company_name = account_name
            'p.first_name = FirstName.Text.Replace("'", "").Trim : p.last_name = LastName.Text.Replace("'", "").Trim
            'p.tel_no = txtPhone.Text.Replace("'", "").Trim
            'p.country = ddlCountry.SelectedValue
            p.source = "My"
            'p.city = txtCity.Text.Replace("'", "").Trim : p.state = state : p.zip = zip : p.address = address
            'p.business_application_area = ddlBAA.SelectedValue : p.in_product = ddlInterestedProd.SelectedValue
        End With
        ws1.register("My", p)
        ws1.Dispose()
        If CInt(tbOPBase.dbExecuteScalar("MYLOCAL", String.Format("select count(*) from SSO_MEMBER where EMAIL_ADDR = '{0}'", email))) = 0 Then
            tbOPBase.dbExecuteNoQuery("MYLOCAL", String.Format("insert into SSO_MEMBER (EMAIL_ADDR,USER_STATUS) values ('{0}',1)", email))
        End If
        'Threading.Thread.Sleep(10000)
        'Util.SyncContactFromSiebelByEmail(email.Text.Trim)

        Return True
    End Function
    Shared Function getMyQuoteRecordByAccountOwnerCN(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                 ByVal CompanyID As String, ByVal Status As String, ByVal QuoteIDList As String,
                               ByVal QuoteCreateFromDate As Date, ByVal QuoteCreateToDate As Date, ByVal CreatedBy As String, Optional ByVal datePeriodType As Integer = 0) As String

        '     Dim SQLSTR As String = String.Format(" SELECT top 20 *,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log FROM {0} AS a WHERE (createdBy IN (select DISTINCT SID from ManagerSalesMap WHERE MID='{6}') OR createdby='{6}' ) " & _
        '" and a.CustomId like N'%{1}%' and a.quoteToName like N'%{2}%' and a.quoteToErpId like N'%{3}%' and a.qStatus like N'%{4}%' and a.quoteId like '%{5}%'" & _
        '" ORDER BY quoteDate DESC, quoteToRowId ", myQM.tb, CustomId, CompanyName, CompanyID, Status, QuoteID, user)

        Dim _sql As New StringBuilder
        '_sql.Append("SELECT top 20 *")


        _sql.AppendLine(" SELECT TOP (600) quoteId, quoteNo, Revision_Number, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency, salesEmail, salesRowId,  ")
        _sql.AppendLine(" directPhone, attentionRowId, attentionEmail, bankInfo, quoteDate, deliveryDate, expiredDate, shipTerm,  ")
        _sql.AppendLine(" paymentTerm, freight, insurance, specialCharge, tax, quoteNote, relatedInfo, createdBy, createdDate, preparedBy,  ")
        _sql.AppendLine(" DOCSTATUS, isShowListPrice, isShowDiscount, isShowDueDate, isLumpSumOnly, isRepeatedOrder, ogroup, DelDateFlag,  ")
        _sql.AppendLine(" org, siebelRBU, DIST_CHAN, DIVISION, SALESGROUP, SALESOFFICE")

        _sql.Append(",IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log")
        _sql.Append(",(SELECT optyid FROM optyQuote where optyQuote.quoteId=a.Quoteid) as siebelQuoteId")
        _sql.Append(String.Format(" ,Docreg FROM {0} AS a ", "QuotationMaster"))
        _sql.Append(String.Format(" WHERE (createdBy IN (select DISTINCT SID from ManagerSalesMap WHERE MID='{0}') OR createdby='{0}' ) ", user))

        'Frank 20130626 For re-vision function: Only allow the Active=1 quote records
        _sql.AppendLine(" and a.Active=1 ")

        'Frank 2012/10/22
        If IsNumeric(datePeriodType) AndAlso datePeriodType = 1 Then
            If QuoteCreateFromDate <> Nothing Then
                _sql.AppendLine(" and a.quoteDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
            End If
            If QuoteCreateToDate <> Nothing Then
                _sql.AppendLine(" and a.quoteDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
            End If
        Else
            If QuoteCreateFromDate <> Nothing Then
                _sql.AppendLine(" and a.createdDate >= '" + QuoteCreateFromDate.ToString("yyyy-MM-dd") + "' ")
            End If
            If QuoteCreateToDate <> Nothing Then
                _sql.AppendLine(" and a.createdDate <= '" + QuoteCreateToDate.ToString("yyyy-MM-dd") + "' ")
            End If
        End If

        If Not String.IsNullOrEmpty(CustomId) Then
            _sql.Append(String.Format(" and a.CustomId like N'%{0}%'", CustomId))
        End If
        If Not String.IsNullOrEmpty(CompanyName) Then
            _sql.Append(String.Format(" and a.quoteToName like N'%{0}%'", CompanyName))
        End If
        If Not String.IsNullOrEmpty(CompanyID) Then
            _sql.Append(String.Format(" and a.quoteToErpId like N'%{0}%'", CompanyID))
        End If
        If Not String.IsNullOrEmpty(Status) Then
            _sql.Append(String.Format(" and a.DOCSTATUS = N'{0}'", Status))
        Else
            _sql.Append(String.Format(" and a.DOCSTATUS <> N'{0}'", CInt(COMM.Fixer.eDocStatus.QDELETED)))
        End If

        If Not String.IsNullOrEmpty(CreatedBy) Then
            _sql.AppendFormat(String.Format(" and createdBy like N'%{0}%' ", CreatedBy))
        End If


        If Not String.IsNullOrEmpty(QuoteNo) Then
            _sql.Append(String.Format(" and a.quoteNo like '%{0}%'", QuoteNo))
        End If
        If Not String.IsNullOrEmpty(QuoteIDList) AndAlso QuoteIDList.Length >= 1 Then _sql.AppendLine(String.Format(" and a.QuoteId in ({0}) ", QuoteIDList))

        _sql.AppendLine(" order by a.quoteDate desc,a.quoteNo desc ")

        'Return SQLSTR
        Return _sql.ToString

    End Function


    'Shared Function getQuoteToOrderLog(ByVal QuoteID As String, ByVal IsReturnTop1Record As Boolean) As String

    '    If QuoteID Is Nothing Then QuoteID = ""

    '    Dim SQLSTR As New StringBuilder
    '    'IsReturnTop1Record means return is return newest order record
    '    If IsReturnTop1Record Then
    '        SQLSTR.Append("SELECT Top 1 c.ORDER_NO, c.PO_NO,SOLDTO_ID, c.SHIPTO_ID, c.BILLTO_ID")
    '    Else
    '        SQLSTR.Append("SELECT c.ORDER_NO, c.PO_NO,SOLDTO_ID, c.SHIPTO_ID, c.BILLTO_ID")
    '    End If
    '    SQLSTR.Append(", c.ORDER_DATE, SUM(b.unit_price*b.qty) as AMOUNT, c.ORDER_STATUS")
    '    SQLSTR.Append(", c.DUE_DATE")

    '    SQLSTR.Append(" FROM ORDER_DETAIL b Left join ORDER_MASTER c on b.ORDER_ID=c.ORDER_ID")
    '    SQLSTR.Append(String.Format(" Where b.OptyID = '{0}' and c.ORDER_NO <>''", QuoteID.Replace("'", "''")))
    '    SQLSTR.Append(" Group by c.ORDER_NO, c.PO_NO, c.SOLDTO_ID, c.SHIPTO_ID, c.BILLTO_ID, c.ORDER_DATE, c.ORDER_STATUS, c.DUE_DATE")

    '    If IsReturnTop1Record Then
    '        SQLSTR.Append(" Order by c.ORDER_DATE Desc")
    '    Else
    '        SQLSTR.Append(" Order by c.ORDER_DATE")
    '    End If

    '    Return SQLSTR.ToString

    'End Function

    Shared Function getQuoteToOrderLog(ByVal QuoteID As String, ByVal IsReturnTop1Record As Boolean) As String


        If QuoteID Is Nothing Then QuoteID = ""

        Dim SQLSTR As New StringBuilder
        With SQLSTR
            'IsReturnTop1Record means return is return newest order record
            .Append("SELECT")
            If IsReturnTop1Record Then
                .Append(" Top 1")
            End If
            'SQLSTR.Append(" b.SO_NO, b.PO_NO,b.SOLDTO_ID, b.SHIPTO_ID, b.BILLTO_ID")
            SQLSTR.Append(" b.SO_NO, b.PO_NO,'' as SOLDTO_ID,'' as SHIPTO_ID,'' as BILLTO_ID")

            'SQLSTR.Append(", b.ORDER_DATE, SUM(a.unit_price*a.qty) as AMOUNT, b.ORDER_STATUS, b.DUE_DATE")
            SQLSTR.Append(", b.ORDER_DATE, SUM(a.unitPrice * a.qty) as AMOUNT, '' as ORDER_STATUS, '' as DUE_DATE")

            SQLSTR.Append(" FROM QuotationDetail a Left join QUOTE_TO_ORDER_LOG b on a.quoteId=b.QUOTEID")
            SQLSTR.Append(String.Format(" Where a.QUOTEID = '{0}' And b.SO_NO<>''", QuoteID.Replace("'", "''")))
            'SQLSTR.Append(" Group by b.SO_NO, b.PO_NO, b.SOLDTO_ID, b.SHIPTO_ID, b.BILLTO_ID, b.ORDER_DATE, b.ORDER_STATUS, b.DUE_DATE")
            SQLSTR.Append(" Group by b.SO_NO, b.PO_NO, b.ORDER_DATE")

            If IsReturnTop1Record Then
                SQLSTR.Append(" Order by b.ORDER_DATE Desc")
            Else
                SQLSTR.Append(" Order by b.ORDER_DATE")
            End If
            '.Append(" M.ORDER_ID as SO_NO, M.PO_NO,  ")
            '.Append("  M.ORDER_DATE, SUM(D.unit_Price * D.qty) as AMOUNT, M.ORDER_STATUS, M.DUE_DATE, ")
            '.Append("  (select top 1 ISNULL(erpid,'') as SDerpid from ORDER_PARTNERS where order_id=  M.ORDER_ID and TYPE='SOLDTO')  as  SOLDTO_ID, ")
            '.Append("  (select top 1 ISNULL(erpid,'') as Serpid from ORDER_PARTNERS where order_id=  M.ORDER_ID and TYPE='S')  as  SHIPTO_ID, ")
            '.Append(" (select top 1 ISNULL(erpid,'') as Berpid from ORDER_PARTNERS where order_id=  M.ORDER_ID and TYPE='B')  as  BILLTO_ID ")
            '.Append(" FROM ORDER_DETAIL D Left join CreateSAPQuoteLog L on L.ORDERID=D.ORDER_ID ")
            '.Append("  LEFT JOIN ORDER_MASTER M on M.ORDER_ID = D.ORDER_ID  ")
            '.Append(String.Format(" Where L.QUOTEID = '{0}' And M.ORDER_ID<>'' ", QuoteID))
            '.Append("  Group by M.ORDER_ID ,  M.PO_NO, M.ORDER_DATE, M.DUE_DATE,M.ORDER_STATUS,M.SOLDTO_ID ")
            '.Append("  order by M.ORDER_DATE desc ")
        End With
        Return SQLSTR.ToString

    End Function

    Shared Function getQuoteIDListByeQuotationSOPO(ByVal SO As String, ByVal PO As String) As String

        Dim _SQL As String = Business.getQuoteListBySOPO_V2(SO, PO), _QuoteIdIN As String = String.Empty
        If String.IsNullOrEmpty(_SQL) = False Then
            'Dim dt1 As DataTable = tbOPBase.dbGetDataTable("B2B", _SQL)
            Dim dt1 As DataTable = tbOPBase.dbGetDataTable("EQ", _SQL)
            If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then
                For Each _row As DataRow In dt1.Rows
                    _QuoteIdIN &= "'" & _row.Item("QuoteID").ToString.Replace("'", "''") & "',"
                Next
                _QuoteIdIN = _QuoteIdIN.TrimEnd(",")
            End If
        End If

        Return _QuoteIdIN

    End Function

    Shared Function getQuoteListBySOPO_V2(ByVal SO As String, ByVal PO As String) As String

        If String.IsNullOrEmpty(SO) AndAlso String.IsNullOrEmpty(PO) Then
            Return Nothing
        End If

        Dim SQLSTR As New StringBuilder
        SQLSTR.Append("SELECT QUOTEID ")

        SQLSTR.Append(" FROM QUOTE_TO_ORDER_LOG a ")
        SQLSTR.Append(" Where a.QUOTEID <>''")
        If String.IsNullOrEmpty(SO) = False Then
            SQLSTR.Append(String.Format(" And a.SO_NO like '%{0}%'", SO.Replace("'", "''")))
        End If
        If String.IsNullOrEmpty(PO) = False Then
            SQLSTR.Append(String.Format(" And a.PO_NO like '%{0}%'", PO.Replace("'", "''")))
        End If

        SQLSTR.Append(" Group by a.QUOTEID")
        SQLSTR.Append(" Order by a.QUOTEID")

        Return SQLSTR.ToString

    End Function


    Shared Function getQuoteListBySOPO(ByVal SO As String, ByVal PO As String) As String

        If String.IsNullOrEmpty(SO) AndAlso String.IsNullOrEmpty(PO) Then
            Return Nothing
        End If

        Dim SQLSTR As New StringBuilder
        SQLSTR.Append("SELECT a.OptyID as QuoteID")

        SQLSTR.Append(" FROM ORDER_DETAIL a Left join ORDER_MASTER b on a.ORDER_ID=b.ORDER_ID")
        SQLSTR.Append(" Where a.OptyID like 'GQ%' and b.ORDER_NO <>''")
        If String.IsNullOrEmpty(SO) = False Then
            SQLSTR.Append(String.Format(" And b.ORDER_NO = '{0}'", SO.Replace("'", "''")))
        End If
        If String.IsNullOrEmpty(PO) = False Then
            SQLSTR.Append(String.Format(" And b.PO_NO = '{0}'", PO.Replace("'", "''")))
        End If

        SQLSTR.Append(" Group by a.OptyID")
        SQLSTR.Append(" Order by a.OptyID")

        Return SQLSTR.ToString

    End Function
    Shared Function getQuoteListBySOPOV2(ByVal SO As String, ByVal PO As String) As String
        If String.IsNullOrEmpty(SO) AndAlso String.IsNullOrEmpty(PO) Then
            Return String.Empty
        End If
        Dim SQLSTR As New StringBuilder
        SQLSTR.Append(" select distinct b.QUOTEID FROM QUOTE_TO_ORDER_LOG b where b.QUOTEID IN ")
        SQLSTR.Append(" (  ")
        SQLSTR.Append("   select quoteId  FROM  QuotationMaster where quoteNo like 'GQ%' ")
        SQLSTR.Append("  ) ")
        If String.IsNullOrEmpty(SO) = False Then
            SQLSTR.Append(String.Format(" And b.SO_NO = '{0}'", SO.Replace("'", "''")))
        End If
        If String.IsNullOrEmpty(PO) = False Then
            SQLSTR.Append(String.Format(" And b.PO_NO = '{0}'", PO.Replace("'", "''")))
        End If
        'SQLSTR.Append(" Group by a.OptyID")
        'SQLSTR.Append(" Order by a.OptyID")
        Return SQLSTR.ToString
    End Function
    Shared Function isTeamOwner(ByVal QuoteId As String, ByVal user As String) As Boolean

        Dim SQLSTR As String = String.Format(" SELECT top 1 quoteid FROM {0} AS a WHERE (createdBy='{2}' " &
   " OR (quoteToRowId IN ( SELECT DISTINCT ACCOUNT_ROW_ID FROM SIEBEL_ACCOUNT_OWNER_EMAIL AS z WHERE (ACCOUNT_ROW_ID IS NOT NULL) AND (EMAIL_ADDRESS = '{2}') ) ) " &
   " OR ( quoteToErpId IN ( SELECT DISTINCT COMPANY_ID FROM SAP_COMPANY_OWNER_EMAIL AS z WHERE (COMPANY_ID IS NOT NULL) AND (email = '{2}') ) )) " &
   " and a.quoteId  like  '%{1}%'" &
   " ORDER BY quoteDate DESC, quoteToRowId ", "QuotationMaster", QuoteId, user)
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", SQLSTR)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Shared Function getMyTeamsQuoteRecord(ByVal QuoteNo As String, ByVal user As String, ByVal CustomId As String, ByVal CompanyName As String,
                                     ByVal CompanyID As String, ByVal Status As String, ByVal state As String, ByVal province As String) As String

        'Dim SQLSTR As String = String.Format( _
        '    " select top 20 * from {0} where active=1 and QuoteNo like '%{6}%' and CustomId like N'%{1}%' and quoteToName like N'%{2}%' " + _
        '    " and quoteToErpId like N'%{3}%' and DOCSTATUS = N'{4}' and quoteID in (select distinct quote_id as quoteid from quotation_approval where approver='{5}') " & _
        '    " and quotetorowid in (select row_id from [ACLSQL6\SQL2008R2].MyAdvantechGlobal.dbo.SIEBEL_ACCOUNT where state like '%{7}%' and province like '%{8}%') " & _
        '    " order by quoteDate desc,quoteid desc", _
        '     "QuotationMaster", CustomId, CompanyName, CompanyID, Status, user, QuoteNo, state, province)

        Dim SQLSTR As String = String.Format(
            " select top 20 * from {0} where active=1 and QuoteNo like '%{5}%' and CustomId like N'%{1}%' and quoteToName like N'%{2}%' " +
            " and quoteToErpId like N'%{3}%' and quoteID in (select distinct quote_id as quoteid from quotation_approval where approver='{4}') " &
            " and quotetorowid in (select row_id from MyAdvantechGlobal.dbo.SIEBEL_ACCOUNT where state like '%{6}%' and province like '%{7}%') " _
            , "QuotationMaster", CustomId, CompanyName, CompanyID, user, QuoteNo, state, province)

        If Not String.IsNullOrEmpty(Status) Then
            SQLSTR &= " and DOCSTATUS='" & Status & "'"
        End If
        SQLSTR &= " order by quoteDate desc,quoteid desc"

        'Response.Write(SQLSTR)
        Return SQLSTR
        'Dim dt As New DataTable
        'dt = tbOPBase.dbGetDataTable("EQ", SQLSTR)
        'Return dt
    End Function

    Shared Function isPhaseOut_OLD(ByVal pn As String, ByVal org As String) As Boolean
        Dim f As Boolean = False
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("B2B", String.Format(
                                     " select part_no from SAP_PRODUCT_STATUS_ORDERABLE where part_no='{0}' and sales_org='{1}' ", pn, org))
        If dt.Rows.Count = 0 Then
            f = True
        Else

        End If
        Return f
    End Function

    Shared Function isInvalidPhaseOutV2(ByVal PartNo As String, ByVal SalesOrg As String, ByRef StatusCode As String, ByRef StatusDesc As String, ByRef ATPQty As Decimal, Optional ByVal IsSyncToLocalDB As Boolean = True) As Boolean
        'Frank 2012/09/03 Move this function to Relics.SAPDAL.isInvalidPhaseOutV2
        'Dim _IsPhaseOut As Boolean = Relics.SAPDAL.isInvalidPhaseOutV2(PartNo, SalesOrg, StatusCode, StatusDesc, ATPQty, IsSyncToLocalDB)
        'Frank
        Dim _IsPhaseOut As Boolean = SAPDAL.SAPDAL.isInvalidPhaseOutV2(PartNo, SalesOrg, StatusCode, StatusDesc, ATPQty, IsSyncToLocalDB)

        If _IsPhaseOut AndAlso SalesOrg.Equals("US01") Then
            Dim _StatusCode As String = String.Empty, _StatusDesc As String = String.Empty
            'If Relics.SAPDAL.IsBlankPartStatus(PartNo, SalesOrg, _StatusCode, _StatusDesc) Then
            If SAPDAL.SAPDAL.IsBlankPartStatus(PartNo, SalesOrg, _StatusCode, _StatusDesc) Then
                StatusCode = _StatusCode : StatusDesc = _StatusDesc : Return False
            End If
        End If

        Return _IsPhaseOut

        'Return Relics.SAPDAL.isInvalidPhaseOutV2(PartNo, SalesOrg, StatusCode, StatusDesc, ATPQty, IsSyncToLocalDB)
    End Function

    'Shared Function isInvalidPhaseOutV2(ByVal PartNo As String, ByVal SalesOrg As String, ByRef StatusCode As String, ByRef StatusDesc As String, ByRef ATPQty As Decimal, Optional ByVal IsSyncToLocalDB As Boolean = True) As Boolean
    '    SalesOrg = Trim(UCase(SalesOrg)) : PartNo = Trim(UCase(PartNo))

    '    'Frank 2012/08/22:Do not check extended warranty partno
    '    If PartNo.StartsWith("AGS-EW") Then Return False
    '    PartNo = replaceCartBTO(PartNo, SalesOrg)
    '    If PartNo.ToUpper.EndsWith("-BTO") Then Return False
    '    Dim IsNumericPn As Boolean = False
    '    For i As Integer = 0 To PartNo.Length - 1
    '        If IsNumeric(PartNo.Substring(i, 1)) Then
    '            IsNumericPn = True
    '        Else
    '            IsNumericPn = False : Exit For
    '        End If
    '    Next
    '    If IsNumericPn Then
    '        Dim intZeros As Integer = 18 - PartNo.Length
    '        For i As Integer = 1 To intZeros
    '            PartNo = "0" + PartNo
    '        Next
    '    End If

    '    Dim strSql As String = String.Empty
    '    'Dim strSql As String = _
    '    '    " select a.vmsta as status_code, b.vmstb as status_desc, c.MMSTA as xy_status_code" + _
    '    '    " from saprdp.MVKE a left join saprdp.TVMST b on a.vmsta=b.vmsta" + _
    '    '    " left join saprdp.MARC c on a.MATNR=c.MATNR" + _
    '    '    " where a.mandt='168' " + _
    '    '    " and b.mandt='168' and a.vkorg='" + SalesOrg + "' and a.matnr='" + PartNo + "' and b.spras='E' and rownum=1"

    '    'Frank 2012/08/24:If part no start with X or Y and org is US01, then check product status from field "saprdp.MARC.MMSTA"
    '    If SalesOrg.Equals("US01", StringComparison.InvariantCultureIgnoreCase) AndAlso ( _
    '        PartNo.StartsWith("X", StringComparison.InvariantCultureIgnoreCase) OrElse _
    '        PartNo.StartsWith("Y", StringComparison.InvariantCultureIgnoreCase)) Then
    '        'c.spras='E' means to get English version product status description.
    '        strSql = _
    '        " select a.MMSTA as status_code, c.vmstb as status_desc" + _
    '        " from saprdp.MARC a left join saprdp.TVMST C on a.MMSTA=c.vmsta" + _
    '        " where a.mandt='168' and a.werks='USH1'" + _
    '        " and a.matnr='" + PartNo + "' and c.spras='E' and rownum=1"
    '    Else
    '        strSql = _
    '        " select a.vmsta as status_code, b.vmstb as status_desc" + _
    '        " from saprdp.MVKE a left join saprdp.TVMST b on a.vmsta=b.vmsta" + _
    '        " where a.mandt='168' " + _
    '        " and b.mandt='168' and a.vkorg='" + SalesOrg + "' and a.matnr='" + PartNo + "' and b.spras='E' and rownum=1"
    '    End If

    '    Dim dtProdStatus As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)

    '    If dtProdStatus.Rows.Count > 0 Then

    '        'Frank 2012/08/22:is Sync real time product data from sap to myadvanglobal
    '        'Put below code here because above codes will add 0 begin with part no if have to do
    '        If IsSyncToLocalDB Then Relics.SAPDAL.SyncSAPProductStatusToMyadvanGlobal(PartNo, SalesOrg)

    '        StatusCode = dtProdStatus.Rows(0).Item("status_code") : StatusDesc = dtProdStatus.Rows(0).Item("status_desc")

    '        Select Case StatusCode
    '            Case "A", "N", "H"
    '                Return False
    '            Case "O", "S"
    '                Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP, intInventory As Integer = -1
    '                Dim atpTb As New GET_MATERIAL_ATP.BAPIWMDVETable, retTb As New GET_MATERIAL_ATP.BAPIWMDVSTable, rOfretTb As New GET_MATERIAL_ATP.BAPIWMDVS
    '                rOfretTb.Req_Date = Now.ToString("yyyyMMdd") : rOfretTb.Req_Qty = 999 : retTb.Add(rOfretTb)
    '                p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
    '                p1.Connection.Open()
    '                p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", PartNo, Left(SalesOrg, 2) + "H1", "", "", "", "", "PC", _
    '                                       "", intInventory, "", "", New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)
    '                p1.Connection.Close()
    '                For i As Integer = 0 To atpTb.Count - 1
    '                    If atpTb(i).Com_Qty > 0 Then
    '                        ATPQty = atpTb(i).Com_Qty : Return False
    '                    End If
    '                Next
    '                ATPQty = 0 : Return True
    '            Case "I"
    '                Return True
    '            Case Else
    '                Return True
    '        End Select
    '    Else
    '        StatusCode = "" : StatusDesc = "" : Return True
    '    End If
    'End Function

    Shared Sub updateQuoteToErpId(ByVal rowId As String, ByVal quoteid As String, ByVal oType As COMM.Fixer.eDocType)

        Dim ERPID As String = ""
        ERPID = SiebelTools.getErpIdFromSiebelByRowId(rowId)
        If ERPID = "" Then
            Util.showMessage("ERP ID is not yet maintained or not maintained correctly on Siebel.")
        Else
            Pivot.NewObjDocHeader.UpdateAccErpId(rowId, ERPID, oType)


            'Frank 2012/07/27
            '還要將ERPID塞入到EQPARTNER裡面, 包含Sold / Bill / Ship - To三條數據, 每一條都用同樣的ADDRESS / ATTENTION / TEL
            '請使用SAPTools.SearchAllSAPCompanySoldBillShipTo來抓出address這些數據, call一次即可
            Dim _dt As DataTable = SAPTools.SearchAllSAPCompanySoldBillShipTo(ERPID, "", "", "", "", "", "", "", "") '有點慢
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter, _row As DataRow = Nothing
                Dim SoldToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(quoteid, "SOLDTO")
                Dim ShipToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(quoteid, "S")
                Dim BillToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(quoteid, "B")
                'insert sold to
                If SoldToTable.Rows.Count > 0 Then
                    apt.DeleteEQPartnerByQIDandType(quoteid, "SOLDTO")
                    tbOPBase.adddblog("DELETE FROM EQPARTNER WHERE (QUOTEID = '" & quoteid & "') AND (TYPE = '" & "SOLDTO" & "')")
                End If
                _row = _dt.Rows(0)
                apt.InsertPartner(quoteid, rowId, ERPID, _row.Item("COMPANY_NAME"), _row.Item("Address"), "SOLDTO", "", _row.Item("TEL_NO"), _row.Item("TEL_NO"), "", "", "", "", "", "", "", "")
                'insert ship to
                If ShipToTable.Rows.Count = 0 Then
                    apt.InsertPartner(quoteid, rowId, ERPID, _row.Item("COMPANY_NAME"), _row.Item("Address"), "S", "", _row.Item("TEL_NO"), _row.Item("TEL_NO"), "", "", "", "", "", "", "", "")
                End If
                'insert bill to
                If BillToTable.Rows.Count = 0 Then
                    apt.InsertPartner(quoteid, rowId, ERPID, _row.Item("COMPANY_NAME"), _row.Item("Address"), "B", "", _row.Item("TEL_NO"), _row.Item("TEL_NO"), "", "", "", "", "", "", "", "")
                End If
            End If
        End If
    End Sub

    Shared Function getMySelfQuote(ByVal user As String, ByVal Desc As String) As String
        Dim str As String = String.Format("select * from quotationMaster where createdBy='{0}' and (customId like '%{1}%' or quoteNo like '%{1}%') and DOCSTATUS <> '" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "' order by quoteDate desc", user, Desc)
        Return str
    End Function
    Shared Function getMySelfQuoteFinish(ByVal user As String, ByVal Desc As String) As String
        Dim str As String = String.Format("select * from quotationMaster where createdBy='{0}' and (customId like '%{1}%' or quoteNo like '%{1}%') and DOCSTATUS='{2}'", user, Desc, CInt(COMM.Fixer.eDocStatus.QFINISH))
        Return str
    End Function
    Shared Function getSiebelQuoteIdByQuoteId(ByVal quoteID As String) As String
        Dim SquoteId As String = ""
        Dim myQuoteSiebelQuote As New quoteSiebelQuote("EQ", "quoteSiebelQuote")
        Dim DT As DataTable = myQuoteSiebelQuote.GetDT(String.Format("quoteId='{0}'", quoteID), "")
        If DT.Rows.Count > 0 Then
            SquoteId = DT.Rows(0).Item("siebelQuoteId")
        End If
        Return SquoteId
    End Function


    Shared Sub send_Quotation_Approval(ByVal quoteId As String, ByVal ApprovalStr As String, ByVal oType As COMM.Fixer.eDocType)

        'Dim myQA As New quotation_approval("EQ", "quotation_approval")
        'Dim DTA As DataTable = myQA.GetDT(String.Format("quote_Id='{0}'", ARGID), "Approval_Level Desc")
        Dim dtM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(quoteId, oType)
        Dim QuoteNo As String = dtM.quoteNo

        If IsNothing(dtM) Then
            'Try
            Dim userid As String = ""

            If Pivot.CurrentProfile.UserId <> "" Then userid = Pivot.CurrentProfile.UserId
            Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Error Massage by " + userid, "", "DTMaster for " + quoteId + "  is no row ", "")
            'Catch ex As Exception
            'End Try
            Exit Sub
        End If
        Dim cc As String = "myadvantech@advantech.com"
        'cc = ""
        Dim subject As String = ""
        Dim mailbody As String = ""
        Dim sendTo As String = ""
        Dim type As String = ""
        Dim typeStr As String = ""

        If GPControl.isApproved(quoteId) Then
            type = "A"
        End If
        If GPControl.isInApproval(quoteId) Then
            type = "I"
        End If
        If GPControl.isRejected(quoteId) Then
            type = "R"
        End If

        Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(quoteId)
        If _QMaster Is Nothing Then Exit Sub
        Dim _ME As QuotationExtension = _QMaster.QuoteExtensionX
        If _ME Is Nothing Then Exit Sub
        Dim myDoc As New System.Xml.XmlDocument
        Dim iRet1 As String = Util.HtmlToXML(String.Format("~/Quote/{1}?UID={0}", quoteId, Business.getPiPage(quoteId, dtM.DocReg)), myDoc)
        Dim fragment As System.Xml.XmlDocumentFragment = myDoc.CreateDocumentFragment()
        Dim temp As String = ""
        Dim _RunTimeSiteUrl As String = GetRuntimeSiteUrl()
        If type = "I" Then
            sendTo = GPControl.getNextApprover(quoteId)
            typeStr = "Request"
            cc = cc & "," & GPControl.Agent(sendTo)
            cc = cc.Trim(",")
            If dtM.org.ToString.StartsWith("US") Then
                'subject = "Quotation " & quoteId & " for: " & dtM.AccErpId.ToString
                subject = "Quotation " & QuoteNo & " for: " & dtM.AccErpId.ToString
            Else
                'subject = "eQuotation Approval request for: " & "(" & quoteId & ")" & " Type: " & typeStr
                subject = "eQuotation Approval request for: " & "(" & QuoteNo & ")" & " Type: " & typeStr
            End If
            Dim level As Integer = GPControl.getNextLevel(quoteId)
            Dim uniqidY As String = GPControl.GP_getMobileYesOrNoUniqCodeByLevel(quoteId, level, "YES")
            Dim uniqidN As String = GPControl.GP_getMobileYesOrNoUniqCodeByLevel(quoteId, level, "NO")

            Dim st As String = "Approved"
            Dim f As Integer = GPControl.getLastApprovalType(quoteId)
            If f < 0 Then
                st = "Rejected"
            End If
            'Dim _GPandExpirationStr As String = "Below GP"
            'If _ME.ApprovalFlowTypeX = eQApprovalFlowType.GPandExpiration Then
            '    _GPandExpirationStr += " and Expiration date is more than 30 days"
            'End If
            'Dim StatusStr As String = String.Format("{0}. (Reason:<font color=""#ff0000""> {1}</font>)", "Request for GP approval", _GPandExpirationStr)
            Dim StatusStr As String = "Request for GP approval"
            Dim ApprovedBy As String = GPControl.getLastProcessor(quoteId)
            If Not ApprovedBy = "System@System" Then
                StatusStr = "<font color=""#333333"">--( " & st & " By " & Left(ApprovedBy, InStr(ApprovedBy, "@") - 1) & " )</font>"
            End If

            'temp = "<table><tr><td><a href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & HttpContext.Current.Request.ServerVariables("SERVER_PORT") & ":" & HttpContext.Current.Request.ServerVariables("SERVER_PORT") & "/Functions/DoApprove.aspx?AC=" & uniqidY & "&uid=" & quoteId & """>Approve</a></td><td> | </td><td><a href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & HttpContext.Current.Request.ServerVariables("SERVER_PORT") & ":" & HttpContext.Current.Request.ServerVariables("SERVER_PORT") & "/Functions/DoApprove.aspx?AC=" & uniqidN & "&uid=" & quoteId & """>Reject</a></td></tr></table>"

            'temp &= "<TABLE width=""100%"" border=""0"">" & _
            '    "<tr><td style=""padding-left:10px;border-bottom:#ffa500 1px dashed;height:20px;background-color:#fff7cb"" align=""left"" valign=""middle"" class=""text"">" & _
            '     "<font color=""#ff0000""><b>Comment</b></font><font color=""#999999"">--( " & st & " By " & Left(ApprovedBy, InStr(ApprovedBy, "@") - 1) & " )</font></td></tr><tr><td style=""padding:1px 15px;background-color:#feffcb"">" & _
            '     ApprovalStr.Replace(Chr(13), "<br/>") & _
            '     "</td></tr>" & _
            '     "<tr><td align=""center""><a style="" font-family:Times New Roman ; font-size:12.0pt"" href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & ":" & HttpContext.Current.Request.ServerVariables("SERVER_PORT") & "/quote/QuoteApproval.aspx?UID=" & quoteId & """ target=""_blank"">Check</a></td></tr></table>"
            temp &= "<TABLE width=""100%"" border=""0"">" &
                "<tr><td style=""padding-left:10px;border-bottom:#ffa500 1px dashed;height:20px;background-color:#fff7cb"" align=""left"" valign=""middle"" class=""text"">" &
                 "<font color=""#1965BF""><b>Comment: </b></font> " & StatusStr & "</td></tr><tr><td style=""padding:1px 15px;background-color:#feffcb"">" &
                 ApprovalStr.Replace(Chr(13), "<br/>") &
                 "</td></tr>" &
                 "<tr><td align=""center""><a style="" font-family:Times New Roman ; font-size:12.0pt"" href=""" & _RunTimeSiteUrl & "/quote/QuoteApproval.aspx?UID=" & quoteId & """ target=""_blank"">Check</a></td></tr></table>"

            Dim mobileMailBody As String = getmobilePage(quoteId, st, ApprovalStr, StatusStr, oType)
            Util.SendEmail("myadvantech@advantech.com", sendTo, cc, "", "Mobile: " & subject, "", mobileMailBody, "")
        ElseIf type = "R" Then
            sendTo = dtM.CreatedBy
            typeStr = "Notice"
            If dtM.org.ToString.StartsWith("US") Then
                'subject = "Quotation " & quoteId & " for: " & dtM.AccErpId.ToString
                subject = "Quotation " & QuoteNo & " for: " & dtM.AccErpId.ToString
            Else
                'subject = "eQuotation Approval request for: " & "(" & quoteId & ")" & " Type: " & typeStr
                subject = "eQuotation Approval request for: " & "(" & QuoteNo & ")" & " Type: " & typeStr
            End If
            Dim reJectedBy As String = GPControl.getLastRejecter(quoteId)
            temp = "<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""padding-left:10px;border-bottom:#ffa500 1px dashed;height:20px;background-color:#fff7cb"" align=""left"" valign=""middle"" class=""text"">" &
                    "<font color=""#ff0000""><b>Comment</b></font><font color=""#999999"">--( Rejected By " & Left(reJectedBy, InStr(reJectedBy, "@") - 1) & " )</font></td></tr><tr><td style=""padding:1px 15px;background-color:#feffcb"">" &
                    ApprovalStr.Replace(Chr(13), "<br/>") &
                    "</td></tr></table>"
        ElseIf type = "A" Then
            sendTo = dtM.CreatedBy
            typeStr = "Notice"
            If dtM.org.ToString.StartsWith("US") Then
                'subject = "Quotation " & quoteId & " for: " & dtM.AccErpId.ToString
                subject = "Quotation " & QuoteNo & " for: " & dtM.AccErpId.ToString
            Else
                'subject = "eQuotation Approval request for: " & "(" & quoteId & ")" & " Type: " & typeStr
                subject = "eQuotation Approval request for: " & "(" & QuoteNo & ")" & " Type: " & typeStr
            End If
            Dim ApprovedBy As String = GPControl.getLastApprover(quoteId)
            temp = "<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""padding-left:10px;border-bottom:#ffa500 1px dashed;height:20px;background-color:#fff7cb"" align=""left"" valign=""middle"" class=""text"">" &
                    "<font color=""#ff0000""><b>Comment</b></font><font color=""#999999"">--( Approved By " & Left(ApprovedBy, InStr(ApprovedBy, "@") - 1) & " )</font></td></tr><tr><td style=""padding:1px 15px;background-color:#feffcb"">" &
                    ApprovalStr.Replace(Chr(13), "<br/>") &
                    "</td></tr></table>"
        Else
            sendTo = dtM.CreatedBy
            typeStr = "Notice"
            If dtM.org.ToString.StartsWith("US") Then
                'subject = "Quotation " & quoteId & " for: " & dtM.AccErpId.ToString
                subject = "Quotation " & QuoteNo & " for: " & dtM.AccErpId.ToString
            Else
                'subject = "eQuotation Approval request for: " & "(" & quoteId & ")" & " Type: " & typeStr
                subject = "eQuotation Approval request for: " & "(" & QuoteNo & ")" & " Type: " & typeStr
            End If
            Dim ApprovedBy As String = GPControl.getLastApprover(quoteId)
            temp = "<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""padding-left:10px;border-bottom:#ffa500 1px dashed;height:20px;background-color:#fff7cb"" align=""left"" valign=""middle"" class=""text"">" &
                    "</td></tr><tr><td style=""padding:1px 15px;background-color:#feffcb""></td></tr></table>"
        End If
        Dim FragDOC As New System.Xml.XmlDocument
        Dim iRet2 As String = Util.HtmlStrToXML(temp, FragDOC, "table")

        If iRet1 = "S" And iRet2 = "S" Then
            fragment.InnerXml = FragDOC.OuterXml.ToString
            Dim objNodeList As System.Xml.XmlNodeList = myDoc.DocumentElement.GetElementsByTagName("div")
            For Each x As System.Xml.XmlElement In objNodeList
                If x.Attributes("id") IsNot Nothing AndAlso x.Attributes("id").Value = "divDetail" Then
                    x.InsertAfter(fragment, x.LastChild)
                End If
            Next
            mailbody = myDoc.OuterXml
            mailbody = mailbody.Replace("<table cellspacing=""0"" rules=""all"" border=""1"" id=""gv1""", "<table cellspacing=""0"" rules=""all"" border=""0"" id=""gv1""")

            'Ryan 20180509 Also send notice mail to SAP VE for AEU
            If dtM.org.ToString.StartsWith("EU") AndAlso typeStr = "Notice" AndAlso Not String.IsNullOrEmpty(_QMaster.quoteToErpId) Then
                Dim SAPVE As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 ISNULL(b.EMAIL,'') as email from SAP_COMPANY_EMPLOYEE a inner join SAP_EMPLOYEE b on a.SALES_CODE = b.SALES_CODE where a.COMPANY_ID = '{0}' and a.PARTNER_FUNCTION = 'VE'", _QMaster.quoteToErpId))
                If SAPVE IsNot Nothing AndAlso Not String.IsNullOrEmpty(SAPVE) AndAlso Util.isEmail(SAPVE.ToString) Then
                    cc = cc & "," & SAPVE.ToString
                End If
            End If

            If sendTo <> "" Then
                Util.SendEmail("myadvantech@advantech.com", sendTo, cc, "", subject, "", mailbody, "")
            End If
        Else
            If sendTo <> "" Then
                HttpContext.Current.Response.Write("[" & mailbody & iRet1.ToString() & iRet2.ToString() & "]") : HttpContext.Current.Response.End()
            End If
        End If
    End Sub
    Shared Sub send_Quotation_Approval_Expiration(ByVal quoteId As String, ByVal ApprovalStr As String, ByVal oType As COMM.Fixer.eDocType)

        Dim dtM As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(quoteId)
        Dim QuoteNo As String = dtM.quoteNo
        If IsNothing(dtM) Then
            Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Error Massage by " + Util.GetCurrentUserID(), "", "DTMaster for " + quoteId + "  is no row ", "")
            Exit Sub
        End If
        Dim cc As String = "myadvantech@advantech.com"
        Dim subject As String = ""
        Dim mailbody As String = ""
        Dim sendTo As String = ""
        Dim type As String = ""
        Dim typeStr As String = ""
        Dim QAE As IEnumerable(Of Quotation_Approval_Expiration) = dtM.QuotationApprovalExpiration
        If QAE Is Nothing Then
            Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Error Massage by " + Util.GetCurrentUserID(), "", "QuotationApprovalExpiration for " + quoteId + "  is no row ", "")
            Exit Sub
        End If
        If QAE.Sum(Function(p) p.Status.Value) = QAE.Count Then
            type = "A" ' 已审核完成
        End If
        If QAE.Where(Function(p) p.Status.Value = 0).FirstOrDefault() IsNot Nothing Then
            type = "I" '审核中...
        End If
        If QAE.Where(Function(p) p.Status.Value = -1).FirstOrDefault() IsNot Nothing Then
            type = "R" '拒绝
        End If
        Dim myDoc As New System.Xml.XmlDocument
        Dim iRet1 As String = Util.HtmlToXML(String.Format("~/Quote/{1}?UID={0}", quoteId, Business.getPiPage(quoteId, dtM.DocReg)), myDoc)
        Dim fragment As System.Xml.XmlDocumentFragment = myDoc.CreateDocumentFragment()
        Dim temp As String = ""
        Dim _RunTimeSiteUrl As String = GetRuntimeSiteUrl()
        If type = "I" Then
            Dim _NextQuotation_Approval_Expiration As Quotation_Approval_Expiration = QAE.Where(Function(p) p.Status.Value = 0).OrderBy(Function(p) p.ApprovalLevel).FirstOrDefault()
            ' Dim _PreQuotation_Approval_Expiration As Quotation_Approval_Expiration = QAE.Where(Function(p) p.Status.Value = 0).OrderBy(Function(p) p.ApprovalLevel).FirstOrDefault()
            sendTo = _NextQuotation_Approval_Expiration.Approver
            typeStr = "Request"
            cc = cc & "," & GPControl.Agent(sendTo)
            cc = cc.Trim(",")
            If dtM.org.ToString.StartsWith("US") Then
                subject = "Quotation " & QuoteNo & " for: " & dtM.quoteToErpId.ToString()
            Else
                subject = "eQuotation Approval request for: " & "(" & QuoteNo & ")" & " Type: " & typeStr
            End If
            'Dim level As Integer = _NextQuotation_Approval_Expiration.ApprovalLevel.Value
            Dim uniqidY As String = _NextQuotation_Approval_Expiration.MobileApproveYes
            Dim uniqidN As String = _NextQuotation_Approval_Expiration.MobileApproveNo

            Dim st As String = "Approved"
            'Dim f As Integer = GPControl.getLastApprovalType(quoteId)
            'If f < 0 Then
            '    st = "Rejected"
            'End If
            'Dim StatusStr As String = String.Format("{0}. (Reason:<font color=""#ff0000""> {1}</font>)", "Request for GP approval", "Expiration date is more than 30 days")
            Dim StatusStr As String = "Request for GP approval"
            Dim nextlevel As Integer = _NextQuotation_Approval_Expiration.ApprovalLevel.Value
            Dim PreList As List(Of Quotation_Approval_Expiration) = QAE.Where(Function(p) p.ApprovalLevel < nextlevel).OrderBy(Function(p) p.ApprovalLevel).ToList()
            If PreList IsNot Nothing AndAlso PreList.Count > 0 Then
                For Each currqae In PreList
                    StatusStr = String.Format("<font color=""#333333"">--( {0} By {1} )</font>", st, currqae.Approver)
                Next
            End If

            temp &= "<TABLE width=""100%"" border=""0"">" &
                "<tr><td style=""padding-left:10px;border-bottom:#ffa500 1px dashed;height:20px;background-color:#fff7cb"" align=""left"" valign=""middle"" class=""text"">" &
                 "<font color=""#1965BF""><b>Comment: </b></font> " & StatusStr & "</td></tr><tr><td style=""padding:1px 15px;background-color:#feffcb"">" &
                 ApprovalStr.Replace(Chr(13), "<br/>") &
                 "</td></tr>" &
                 "<tr><td align=""center""><a style="" font-family:Times New Roman ; font-size:12.0pt"" href=""" & _RunTimeSiteUrl & "/quote/QuoteApproval.aspx?UID=" & quoteId & """ target=""_blank"">Check</a></td></tr></table>"

            Dim mobileMailBody As String = getmobilePageForApprovalExpiration(quoteId, st, "", StatusStr, oType)
            Util.SendEmail("myadvantech@advantech.com", sendTo, cc, "", "Mobile: " & subject, "", mobileMailBody, "")
        ElseIf type = "R" Then
            sendTo = dtM.createdBy
            typeStr = "Notice"
            If dtM.org.ToString.StartsWith("US") Then
                subject = "Quotation " & QuoteNo & " for: " & dtM.quoteToErpId.ToString
            Else
                subject = "eQuotation Approval request for: " & "(" & QuoteNo & ")" & " Type: " & typeStr
            End If
            Dim _rejectQuotation_Approval_Expiration As Quotation_Approval_Expiration = QAE.Where(Function(p) p.Status.Value = -1).OrderBy(Function(p) p.ApprovalLevel).FirstOrDefault()
            Dim reJectedBy As String = _rejectQuotation_Approval_Expiration.Approver
            temp = "<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""padding-left:10px;border-bottom:#ffa500 1px dashed;height:20px;background-color:#fff7cb"" align=""left"" valign=""middle"" class=""text"">" &
                    "<font color=""#ff0000""><b>Comment</b></font><font color=""#999999"">--( Rejected By " & reJectedBy & " )</font></td></tr><tr><td style=""padding:1px 15px;background-color:#feffcb"">" &
                    ApprovalStr.Replace(Chr(13), "<br/>") &
                    "</td></tr></table>"
        ElseIf type = "A" Then
            sendTo = dtM.createdBy
            typeStr = "Notice"
            If dtM.org.ToString.StartsWith("US") Then
                subject = "Quotation " & QuoteNo & " for: " & dtM.quoteToErpId.ToString
            Else
                subject = "eQuotation Approval request for: " & "(" & QuoteNo & ")" & " Type: " & typeStr
            End If
            Dim PreList As List(Of Quotation_Approval_Expiration) = QAE.Where(Function(p) p.Status.Value = 1).OrderBy(Function(p) p.ApprovalLevel).ToList()
            Dim StatusStr As String = String.Empty
            If PreList IsNot Nothing AndAlso PreList.Count > 0 Then
                For Each currqae In PreList
                    StatusStr += String.Format("<p><font color=""#333333"">--( {0} By {1} )</font></p>", "Approved", currqae.Approver)
                Next
            End If
            temp = "<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""padding-left:10px;border-bottom:#ffa500 1px dashed;height:20px;background-color:#fff7cb"" align=""left"" valign=""middle"" class=""text"">" &
                    "<font color=""#ff0000""><b>Comment</b></font>" + StatusStr + "</td></tr><tr><td style=""padding:1px 15px;background-color:#feffcb"">" &
                    ApprovalStr.Replace(Chr(13), "<br/>") &
                    "</td></tr></table>"
        Else
            sendTo = dtM.createdBy
            typeStr = "Notice"
            If dtM.org.ToString.StartsWith("US") Then
                'subject = "Quotation " & quoteId & " for: " & dtM.AccErpId.ToString
                subject = "Quotation " & QuoteNo & " for: " & dtM.quoteToErpId.ToString
            Else
                'subject = "eQuotation Approval request for: " & "(" & quoteId & ")" & " Type: " & typeStr
                subject = "eQuotation Approval request for: " & "(" & QuoteNo & ")" & " Type: " & typeStr
            End If
            Dim ApprovedBy As String = GPControl.getLastApprover(quoteId)
            temp = "<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""padding-left:10px;border-bottom:#ffa500 1px dashed;height:20px;background-color:#fff7cb"" align=""left"" valign=""middle"" class=""text"">" &
                    "</td></tr><tr><td style=""padding:1px 15px;background-color:#feffcb""></td></tr></table>"
        End If
        Dim FragDOC As New System.Xml.XmlDocument
        Dim iRet2 As String = Util.HtmlStrToXML(temp, FragDOC, "table")

        If iRet1 = "S" And iRet2 = "S" Then
            fragment.InnerXml = FragDOC.OuterXml.ToString
            Dim objNodeList As System.Xml.XmlNodeList = myDoc.DocumentElement.GetElementsByTagName("div")
            For Each x As System.Xml.XmlElement In objNodeList
                If x.Attributes("id") IsNot Nothing AndAlso x.Attributes("id").Value = "divDetail" Then
                    x.InsertAfter(fragment, x.LastChild)
                End If
            Next
            mailbody = myDoc.OuterXml
            mailbody = mailbody.Replace("<table cellspacing=""0"" rules=""all"" border=""1"" id=""gv1""", "<table cellspacing=""0"" rules=""all"" border=""0"" id=""gv1""")
            If sendTo <> "" Then
                Util.SendEmail("myadvantech@advantech.com", sendTo, cc, "", subject, "", mailbody, "")
            End If
        Else
            If sendTo <> "" Then
                HttpContext.Current.Response.Write("[" & mailbody & iRet1.ToString() & iRet2.ToString() & "]") : HttpContext.Current.Response.End()
            End If
        End If
    End Sub

    Shared Function getmobilePage(ByVal quoteId As String, ByVal st As String, ByVal ApprovalStr As String, ByVal statusStr As String, ByVal oType As COMM.Fixer.eDocType) As String
        'Dim GPid As String = GPControl.getGPIdByRefId(quoteId)

        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim str As String = ""
        Dim MT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(quoteId, oType)
        Dim DT As New DataTable
        DT = myQD.GetDT(String.Format("quoteId='{0}'", quoteId), "line_No")

        Dim detail As New List(Of struct_GP_Detail)

        For Each x As DataRow In DT.Rows
            Dim detailLine As New struct_GP_Detail
            detailLine.lineNo = x.Item("line_no")
            detailLine.PartNo = x.Item("partno")
            detailLine.Price = x.Item("newunitprice")
            detailLine.QTY = x.Item("qty")
            detailLine.Itp = x.Item("newitp")
            detail.Add(detailLine)
        Next
        str &= "<table><tr><td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Acount Name</nobr></td>" &
        "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Acount Id</nobr></td>" &
        "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Total</nobr></td>" &
        "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Margin</nobr></td></TR>"
        If Not IsNothing(MT) Then
            str &= "<tr>"
            str &= "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & MT.AccName & "</nobr></td>" &
                   "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & MT.AccErpId & "</nobr></td>"
            Dim total As Object = myQD.getTotalAmount(quoteId)
            str &= "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & total.ToString & "</nobr></td>" &
                   "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & getMargin(quoteId) & "%</nobr></td>"
            str &= "</TR>"
            Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(quoteId)
            Dim _ME As QuotationExtension = _QMaster.QuoteExtensionX
            Dim SB As New StringBuilder
            SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>")
            SB.AppendFormat(" <ul>")
            If _ME.ApprovalFlowType = eQApprovalFlowType.GP Then
                SB.AppendFormat("<li>Below GP</li>")
            End If
            If _ME.ApprovalFlowType = eQApprovalFlowType.GPandExpiration Then
                SB.AppendFormat("<li>Below GP</li>")
                SB.AppendFormat("<li>Expiration date is more than 30 days</li>")
            End If
            SB.AppendFormat("</ul>")
            str &= "<tr><td colspan=""4"" style=""font-family:Arial;color:#1965b1;"">" & SB.ToString() & "</td></tr>"
            str &= "<tr><td colspan=""4"" style=""font-family:Arial;color:#1965b1;""><b>Reason: </b>" & MT.relatedInfo & "</td></tr>"
        End If
        str &= "</table>"
        str &= "<HR/>"

        str &= "<table><tr><td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Line No</nobr></td>" &
                          "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Part No</nobr></td>" &
                          "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Unit Price</nobr></td>" &
                          "<TD style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>QTY</nobr></TD>" &
                          "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>ITP</nobr></td></TR>"
        If DT.Rows.Count > 0 Then
            For i As Integer = 0 To DT.Rows.Count - 1
                str &= "<tr>"
                str &= "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("Line_no") & "</nobr></td>" &
                       "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("partno") & "</nobr></td>" &
                       "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("newunitprice") & "</nobr></td>" &
                       "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("QTY") & "</nobr></td>" &
                       "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("newitp") & "</nobr></td>"
                str &= "</TR>"
            Next
        End If
        str &= "</TABLE>"
        str &= "<HR/>"
        str &= "<table><tr><td style=""font-family:Arial;color:#1965b1;""><b>Comment: </b>" & statusStr & "</td></tr><tr><td style=""padding:1px 15px;"">" &
                 ApprovalStr.Replace(Chr(13), "<br/>") &
                 "</td></tr>"
        str &= "</TABLE>"
        Dim level As Integer = GPControl.getNextLevel(quoteId)
        Dim uniqidY As String = GPControl.GP_getMobileYesOrNoUniqCodeByLevel(quoteId, level, "YES")
        Dim uniqidN As String = GPControl.GP_getMobileYesOrNoUniqCodeByLevel(quoteId, level, "NO")

        Dim _RunTimeSiteUrl As String = GetRuntimeSiteUrl()

        'str &= "<table><tr><td><a style=""font-family:Arial"" href=""http://eq.advantech.com/Functions/DoApprove.aspx?AC=" & uniqidY & "&uid=" & quoteId & """>Approve</a></td><td> | </td><td><a style=""font-family:Arial"" href=""http://eq.advantech.com/Functions/DoApprove.aspx?AC=" & uniqidN & "&uid=" & quoteId & """>Reject</a></td></tr></table>"
        str &= "<table><tr><td><a style=""font-family:Arial"" href=""" & _RunTimeSiteUrl
        str &= "/Functions/DoApprove.aspx?AC=" & uniqidY & "&uid=" & quoteId & """>Approve</a></td><td> | </td><td><a style=""font-family:Arial"""
        str &= " href=""" & _RunTimeSiteUrl
        str &= "/Functions/DoApprove.aspx?AC=" & uniqidN & "&uid=" & quoteId & """>Reject</a></td></tr></table>"
        Return str
    End Function
    Shared Function getmobilePageForApprovalExpiration(ByVal quoteId As String, ByVal st As String, ByVal ApprovalStr As String, ByVal statusStr As String, ByVal oType As COMM.Fixer.eDocType) As String
        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim str As String = ""
        Dim MT As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(quoteId, oType)
        Dim DT As New DataTable
        DT = myQD.GetDT(String.Format("quoteId='{0}'", quoteId), "line_No")

        Dim detail As New List(Of struct_GP_Detail)

        For Each x As DataRow In DT.Rows
            Dim detailLine As New struct_GP_Detail
            detailLine.lineNo = x.Item("line_no")
            detailLine.PartNo = x.Item("partno")
            detailLine.Price = x.Item("newunitprice")
            detailLine.QTY = x.Item("qty")
            detailLine.Itp = x.Item("newitp")
            detail.Add(detailLine)
        Next
        str &= "<table><tr><td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Acount Name</nobr></td>" &
        "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Acount Id</nobr></td>" &
        "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Total</nobr></td>" &
        "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Margin</nobr></td></TR>"
        If Not IsNothing(MT) Then
            str &= "<tr>"
            str &= "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & MT.AccName & "</nobr></td>" &
                   "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & MT.AccErpId & "</nobr></td>"
            Dim total As Object = myQD.getTotalAmount(quoteId)
            str &= "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & total.ToString & "</nobr></td>" &
                   "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & getMargin(quoteId) & "%</nobr></td>"
            str &= "</TR>"
            Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(quoteId)
            Dim _ME As QuotationExtension = _QMaster.QuoteExtensionX
            Dim SB As New StringBuilder
            SB.AppendFormat("<B>This quote must be approved through Approval Flow due to below issues:</B>")
            SB.AppendFormat(" <ul>")
            If _ME.ApprovalFlowType = eQApprovalFlowType.ThirtyDaysExpiration Then
                SB.AppendFormat("<li>Expiration date is more than 30 days</li>")
            End If
            SB.AppendFormat("</ul>")
            str &= "<tr><td colspan=""4"" style=""font-family:Arial;color:#1965b1;"">" & SB.ToString() & "</td></tr>"
            str &= "<tr><td colspan=""4"" style=""font-family:Arial;color:#1965b1;""><b>Reason: </b>" & MT.relatedInfo & "</td></tr>"
        End If
        str &= "</table>"
        str &= "<HR/>"

        str &= "<table><tr><td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Line No</nobr></td>" &
                          "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Part No</nobr></td>" &
                          "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>Unit Price</nobr></td>" &
                          "<TD style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>QTY</nobr></TD>" &
                          "<td style=""font-family:Arial;color:#1965b1;font-weight:bold;""><nobr>ITP</nobr></td></TR>"
        If DT.Rows.Count > 0 Then
            For i As Integer = 0 To DT.Rows.Count - 1
                str &= "<tr>"
                str &= "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("Line_no") & "</nobr></td>" &
                       "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("partno") & "</nobr></td>" &
                       "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("newunitprice") & "</nobr></td>" &
                       "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("QTY") & "</nobr></td>" &
                       "<td style=""font-family:Arial;color:#1965b1;""><nobr>" & DT.Rows(i).Item("newitp") & "</nobr></td>"
                str &= "</TR>"
            Next
        End If
        str &= "</TABLE>"
        str &= "<HR/>"
        str &= "<table><tr><td style=""font-family:Arial;color:#1965b1;""><b>Comment: </b>" & statusStr & "</td></tr><tr><td style=""padding:1px 15px;"">" &
                 ApprovalStr.Replace(Chr(13), "<br/>") &
                 "</td></tr>"
        str &= "</TABLE>"
        Dim dtM As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(quoteId)
        Dim QAE As IEnumerable(Of Quotation_Approval_Expiration) = dtM.QuotationApprovalExpiration
        Dim _NextQuotation_Approval_Expiration As Quotation_Approval_Expiration = QAE.Where(Function(p) p.Status.Value = 0).OrderBy(Function(p) p.ApprovalLevel).FirstOrDefault()
        Dim uniqidY As String = _NextQuotation_Approval_Expiration.MobileApproveYes
        Dim uniqidN As String = _NextQuotation_Approval_Expiration.MobileApproveNo
        Dim _RunTimeSiteUrl As String = GetRuntimeSiteUrl()
        str &= "<table><tr><td><a style=""font-family:Arial"" href=""" & _RunTimeSiteUrl
        str &= "/Functions/DoApprove.aspx?AC=" & uniqidY & "&uid=" & quoteId & """>Approve</a></td><td> | </td><td><a style=""font-family:Arial"""
        str &= " href=""" & _RunTimeSiteUrl
        str &= "/Functions/DoApprove.aspx?AC=" & uniqidN & "&uid=" & quoteId & """>Reject</a></td></tr></table>"
        Return str
    End Function
    Shared Function createQuotationByERPIDAndUser(ByVal erpid As String, ByVal createdBy As String, ByVal Desc As String, ByVal comment As String, ByVal isRepeatedOrder As Integer, ByVal org As String, ByVal Detail As List(Of struct_Quote_Detail), ByVal oType As COMM.Fixer.eDocType) As String
        If Not is_Valid_Company_Id(erpid) Then
            Return ""
        End If
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("CRM", SiebelTools.GET_Account_info_By_ERPID(erpid))
        If dt.Rows.Count <= 0 Then
            Return ""
        End If

        Dim strGETRBU As String = String.Format("select isnull(SalesOfficeName,'') as RBU from SAP_DIMCOMPANY where COMPANY_ID='{0}'", erpid)
        Dim dtSAPcompany As New DataTable
        dtSAPcompany = tbOPBase.dbGetDataTable("B2B", strGETRBU)

        'Frank 2012/07/04 Getting quote no by createdBy
        'Dim quoteId As String = Business.GetNoByPrefix("GQ")
        Dim R As IBUS.iRole = Pivot.CurrentProfile(createdBy)
        Dim M As IBUS.iDocHeaderLine = Pivot.NewLineHeader
        M.CustomId = Desc
        M.AccRowId = dt.Rows(0).Item("Row_ID")
        M.AccErpId = erpid
        M.AccName = dt.Rows(0).Item("COMPANYNAME")
        M.AccOfficeName = dtSAPcompany.Rows(0).Item("RBU")
        M.currency = Business.GET_Currency_By_Company_ID(erpid, org)
        M.salesEmail = SiebelTools.getPriSalesEmailByAccountROWID(dt.Rows(0).Item("Row_ID"))
        M.salesRowId = SiebelTools.GET_ContactRowID_by_Email(dt.Rows(0).Item("Row_ID"))
        M.relatedInfo = comment
        M.isRepeatedOrder = isRepeatedOrder
        M.org = org
        M.siebelRBU = dt.Rows(0).Item("RBU")
        Dim quoteid As String = Pivot.NewObjDocHeader.Add(M, R, oType).Key
        Dim _msg As String = String.Empty
        For Each x As struct_Quote_Detail In Detail
            'Dim line_No As Integer = 0

            'Frank: here forget to handle multi system configuration in one order, so I hard code the
            Dim line_No As Integer = x.lineNo, parent_line_No As Integer = 0
            If line_No > 100 Then parent_line_No = 100
            If x.oType = COMM.Fixer.eItemType.Parent Then
                x.oType = COMM.Fixer.eItemType.Parent
            Else
                x.oType = COMM.Fixer.eItemType.Others
            End If
            If Business.ADD2QUOTE_V2_1(quoteid, x.partNo, x.qty, x.ewFlag, x.oType, x.category, 0, 1, Now, parent_line_No, "", line_No, _msg, R, COMM.Fixer.eDocType.EQ) Then
                Dim myQD As New quotationDetail("EQ", "quotationDetail")
                myQD.Update(String.Format("quoteId='{0}' and line_no='{1}'", quoteid, line_No), String.Format("unitprice='{0}',newunitprice='{1}'", x.unitPrice, x.newUnitPrice))
            Else
                Util.InsertMyErrLog("Function createQuotationByERPIDAndUser error:" & _msg)
            End If
        Next
        Return quoteid
    End Function

    Public Shared Function IsOverCreditLimit(ByVal SalesOrg As String, ByVal Company_Id As String) As Boolean

        Dim AEU_WS As New SAP_WS.B2B_AEU_WS
        Dim iRtn As Integer = -1
        Dim clpercentage As String = ""
        'Try
        iRtn = AEU_WS.GET_CREDITLIMIT_USED_PERCENTAGE(Trim(UCase(SalesOrg)), Trim(UCase(Company_Id)), clpercentage)
        'Catch ex As Exception

        'End Try

        If iRtn = 1 Then
            IsOverCreditLimit = True
        Else
            IsOverCreditLimit = False
        End If

    End Function

    Shared Function isPriceUpdated(ByVal argID As String) As Boolean
        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        If myQD.IsExists(String.Format("quoteId='{0}' and unitprice<>newunitprice", argID)) = 1 Then
            Return True
        End If
        Return False
    End Function
    Shared Function isRepeatedOrder(ByVal argId As String, ByVal oType As COMM.Fixer.eDocType) As Boolean

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(argId, oType)
        If Not IsNothing(dt) Then
            If CInt(dt.isRepeatedOrder) = 1 Then
                Return True
            End If
        End If
        Return False
    End Function

    Shared Sub ApprovalProcess(ByVal UID As String, ByVal ACID As String)
        GPControl.doApprove(UID, ACID, "")
        Business.send_Quotation_Approval(UID, "", COMM.Fixer.eDocType.EQ)
        If GPControl.isApproved(UID) Then
            Business.transQuote2Siebel(UID)
        End If
    End Sub

    Shared Function getRUBAddress(ByVal RBU As String, ByVal Group As String, ByVal DocReg As COMM.Fixer.eDocReg) As String
        If (DocReg And COMM.Fixer.eDocReg.AEU) = DocReg Then
            RBU = "AESC"
        End If
        Dim myRBU As New rbuAddress("EQ", "rbuAddress")
        Dim Add As String = ""
        Dim DT As New DataTable
        DT = myRBU.GetDT(String.Format("RBUName='{0}' and RBUGroup='{1}'", RBU, Group), "")

        If DT.Rows.Count > 0 Then
            Add = DT.Rows(0).Item("AddressInfo")
        Else
            Dim dt2 As New DataTable
            dt2 = myRBU.GetDT(String.Format("RBUName='{0}'", RBU, Group), "")
            If dt2.Rows.Count > 0 Then
                Add = dt2.Rows(0).Item("AddressInfo")
            End If
        End If
        Return Add
    End Function

    Public Shared Function IsSpecialADAM(ByVal PartNO As String) As Boolean

        Return False
    End Function
    Public Shared Function IsPTD(ByVal PartNo As String) As Boolean

        'Ryan 20170331 Revise to exclude items with prefix SQR, SQF, EXM.
        If PartNo.StartsWith("SQR") Or PartNo.StartsWith("SQF") Or PartNo.StartsWith("EXM") Then
            Return False
        End If


        Dim f As Boolean = False
        Dim STR As String = String.Format("select count(*) from SAP_PRODUCT where " &
                                            " ((PRODUCT_TYPE = 'ZPER') " &
                                            " OR " &
                                            " ((PRODUCT_TYPE = 'ZFIN' OR PRODUCT_TYPE = 'ZOEM') AND (PART_NO LIKE 'BT%' OR PART_NO LIKE 'DSD%' OR PART_NO LIKE 'ES%' OR PART_NO LIKE 'EWM%' OR PART_NO LIKE 'GPS%' OR PART_NO LIKE 'SQF%' OR PART_NO LIKE 'WIFI%' OR PART_NO LIKE 'PMM%' OR PART_NO LIKE 'Y%')) " &
                                            " OR " &
                                            " ((PRODUCT_TYPE = 'ZRAW') AND (PART_NO LIKE '206Q%')) " &
                                            " OR " &
                                            " ((PRODUCT_TYPE = 'ZSEM') AND (PART_NO LIKE '968Q%'))) " &
                                            " AND PART_NO = '{0}'", PartNo)
        Dim o As New Object
        o = tbOPBase.dbExecuteScalar("B2B", STR)
        If CInt(o) > 0 Then
            f = True
        End If
        Return f
    End Function
    Shared Function getBTOWorkingDate() As String
        Return "10"
    End Function
    Shared Function getBTOParentDueDate(ByVal reqDate As String) As String
        reqDate = CDate(reqDate).ToString("yyyy-MM-dd")
        Dim ws As New SAP_WS.B2B_AEU_WS
        ws.Timeout = -1
        ws.Get_Next_WorkingDate_ByCode(reqDate, getBTOWorkingDate(), "NL")
        ws.Dispose()
        Return CDate(reqDate).ToString("yyyy/MM/dd")
    End Function

    ''' <summary>
    ''' this function is only userd for isBtoOrder=true
    ''' </summary>
    ''' <param name="quotationDetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getBtoQty(ByVal quotationDetail As DataTable) As Integer
        Dim qty As Integer = 1
        If quotationDetail Is Nothing Or quotationDetail.Rows.Count = 0 Then
            qty = 1
        Else
            If quotationDetail.Select("itemType='" & COMM.Fixer.eItemType.Parent & "'").Length > 0 Then
                qty = quotationDetail.Select("itemType='" & COMM.Fixer.eItemType.Parent & "'").First.Item("qty")
            End If
        End If
        Return qty
    End Function

    Public Shared Function GetTaxableAmount(ByVal quoteId As String, ByVal ShiptoId As String) As Decimal
        Dim eqDA As New EQDSTableAdapters.QuotationDetailTableAdapter
        Dim DT As New EQDS.QuotationDetailDataTable
        DT = eqDA.GetQuoteDetailById(quoteId)
        Dim amount As Decimal = 0
        For Each r As EQDS.QuotationDetailRow In DT.Rows
            'If Relics.SAPDAL.isTaxable(r.partNo, ShiptoId) Then
            If SAPDAL.SAPDAL.isTaxable(r.partNo, ShiptoId) Then
                amount += r.newUnitPrice * r.qty
            End If
        Next
        Return amount

    End Function

    Shared Function getTotalAmount(ByVal quotationDetail As DataTable) As Decimal


        Dim rlt As Decimal = 0
        If quotationDetail IsNot Nothing AndAlso quotationDetail.Rows.Count > 0 Then
            rlt = quotationDetail.Select("itemType<>'" & COMM.Fixer.eItemType.Parent & "'").Sum(Function(row) row.Field(Of Decimal)("newunitprice") * row.Field(Of Integer)("qty"))
        End If
        Return rlt
    End Function
    Shared Function getTotalPrice(ByVal quotationDetail As DataTable) As Decimal

        Dim rlt As Decimal = 0
        If quotationDetail IsNot Nothing AndAlso quotationDetail.Rows.Count > 0 Then
            rlt = quotationDetail.Select("itemType<>'" & COMM.Fixer.eItemType.Parent & "'").Sum(Function(row) row.Field(Of Decimal)("newunitprice"))
        End If
        Return rlt
    End Function
    Shared Function getTotalListPrice(ByVal quotationDetail As DataTable) As Decimal

        Dim rlt As Decimal = 0
        If quotationDetail IsNot Nothing AndAlso quotationDetail.Rows.Count > 0 Then
            rlt = quotationDetail.Select("itemType<>'" & COMM.Fixer.eItemType.Parent & "'").Sum(Function(row) row.Field(Of Decimal)("listprice"))
        End If
        Return rlt
    End Function
    Shared Function getTotalListAmount(ByVal quotationDetail As DataTable) As Decimal

        Dim rlt As Decimal = 0
        If quotationDetail IsNot Nothing AndAlso quotationDetail.Rows.Count > 0 Then
            rlt = quotationDetail.Select("itemType<>'" & COMM.Fixer.eItemType.Parent & "'").Sum(Function(row) row.Field(Of Decimal)("listprice") * row.Field(Of Integer)("qty"))
        End If
        Return rlt
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="quotationDetail"></param>
    ''' <returns>ture means the line number is available</returns>
    ''' <remarks></remarks>
    Shared Function GetSpecificLinePN(ByVal quotationDetail As DataTable, ByVal lineNumber As Integer) As Boolean

        Dim rlt As Boolean = False
        If quotationDetail IsNot Nothing AndAlso quotationDetail.Rows.Count > 0 Then
            If quotationDetail.Select(String.Format("line_No={0}", lineNumber)).Length = 0 Then
                rlt = True
            Else
                rlt = False
            End If
        Else
            'if no item in quotation detail, all number is availalbe
            rlt = True
        End If

        Return rlt
    End Function
    Shared Function extendInvalidPhaseOutInfo_old(ByVal SalesOrg As String, ByRef quotationDetail As DataTable) As Boolean
        If quotationDetail IsNot Nothing And quotationDetail.Rows.Count > 0 Then
            SalesOrg = Trim(UCase(SalesOrg))
            Dim pns As List(Of String) = New List(Of String)
            Dim IsNumericPn As Boolean = False
            For Each row In quotationDetail.Rows
                Dim PartNo = row("PartNo")
                For i As Integer = 0 To PartNo.Length - 1
                    If IsNumeric(PartNo.Substring(i, 1)) Then
                        IsNumericPn = True
                    Else
                        IsNumericPn = False : Exit For
                    End If
                Next
                If IsNumericPn Then
                    Dim intZeros As Integer = 18 - PartNo.Length
                    For i As Integer = 1 To intZeros
                        PartNo = "0" + PartNo
                    Next
                End If
                pns.Add(String.Format("'{0}'", PartNo))
            Next
            'empty part no list
            If Not pns.Any() Then
                Return False
            End If

            Dim strSql As String =
                " select a.matnr,a.vmsta as status_code, b.vmstb as status_desc from saprdp.MVKE a left join saprdp.TVMST b on a.vmsta=b.vmsta where a.mandt='168' " +
                " and b.mandt='168' and a.vkorg='" + SalesOrg + "' and a.matnr in(" + String.Join(",", pns.ToArray()) + ") and b.spras='E' "
            Dim dtProdStatus As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)

            quotationDetail.Columns.Add("isPhaseout", GetType(Boolean))
            quotationDetail.Columns.Add("StatusCode", GetType(String))
            quotationDetail.Columns.Add("StatusDesc", GetType(String))
            quotationDetail.Columns.Add("ATPQty", GetType(Integer))



            For Each row In quotationDetail.Rows
                Dim statusrow = dtProdStatus.Select(String.Format("matnr='{0}'", row("PartNo"))).FirstOrDefault()
                If statusrow IsNot Nothing Then
                    row("StatusCode") = statusrow.Item("status_code")
                    row("StatusDesc") = statusrow.Item("status_desc")
                    Dim _isphaseout As Boolean = False
                    Dim ATPQty As Integer = 0
                    Select Case statusrow.Item("status_code")
                        Case "A", "N", "H"
                            _isphaseout = False
                        Case "O", "S"
                            Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP, intInventory As Integer = -1
                            Dim atpTb As New GET_MATERIAL_ATP.BAPIWMDVETable, retTb As New GET_MATERIAL_ATP.BAPIWMDVSTable, rOfretTb As New GET_MATERIAL_ATP.BAPIWMDVS
                            rOfretTb.Req_Date = Now.ToString("yyyyMMdd") : rOfretTb.Req_Qty = 999 : retTb.Add(rOfretTb)
                            p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings("SAP_PRD"))
                            p1.Connection.Open()
                            p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", row("PartNo"), Left(SalesOrg, 2) + "H1", "", "", "", "", "PC",
                                                   "", intInventory, "", "", New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)
                            p1.Connection.Close()
                            For i As Integer = 0 To atpTb.Count - 1
                                If atpTb(i).Com_Qty > 0 Then
                                    ATPQty = atpTb(i).Com_Qty : _isphaseout = False
                                End If
                            Next
                            If ATPQty = 0 Then _isphaseout = True
                        Case "I"
                            _isphaseout = True
                        Case Else
                            _isphaseout = True
                    End Select
                    '\ ming test for SQF-P10S2-4G-CTE
                    'If row("PartNo").ToString.Equals("SQF-P10S2-4G-CTE") Then
                    '    _isphaseout = True
                    'End If
                    ' / end 
                    row("isPhaseout") = _isphaseout
                    row("ATPQty") = ATPQty
                Else
                    row("isPhaseout") = False
                    row("StatusCode") = ""
                    row("StatusDesc") = ""
                    row("ATPQty") = 0
                End If

            Next

        End If
        Return False
    End Function

    Shared Function GetNoByPrefix(ByVal R As iRole)
        Dim O As IBUS.iDoc = Pivot.NewObjDoc
        Return O.NewQuotationNo(Pivot.CurrentProfile.CurrDocReg, COMM.Fixer.eDocType.EQ)
    End Function


    Shared Function CopyQuotation(ByVal quoteid As String, ByRef NewQuoteid As String, ByRef errormessage As String, ByVal oType As COMM.Fixer.eDocType) As Boolean
        If String.Equals(Pivot.CurrentProfile.getCurrOrg, "EU10", StringComparison.CurrentCultureIgnoreCase) Then
            Return CopyAEUQuotationV2(quoteid, NewQuoteid, errormessage, oType, 0)
        End If
        If Not Business.isValidQuote(quoteid, COMM.Fixer.eDocType.EQ) Then
            errormessage = "Quote Id does not exist,or quote status is invalid."
            Return False
        End If
        'Frank 2013/07/02
        'NewQuoteid = Business.GetNoByPrefix(Pivot.CurrentProfile)

        Dim O As IBUS.iDoc = Pivot.NewObjDoc
        NewQuoteid = O.NewUID


        Dim dth As IBUS.iDocHeader = Pivot.NewObjDocHeader

        dth.CopyPasteHeaderLine(quoteid, NewQuoteid, Pivot.CurrentProfile, oType)

        Dim dthl As IBUS.iDocHeaderLine = dth.GetByDocID(quoteid, oType)
        If Not IsNothing(dth) Then
            Dim myQuoteDetail As New quotationDetail("EQ", "quotationDetail")
            Dim dtQuoteDetail As DataTable = myQuoteDetail.GetDT(String.Format("quoteId='{0}'", quoteid), "line_No")
            If dtQuoteDetail.Rows.Count = 0 Then
                errormessage = "No any product can be copied from original quotation"
                Return False
            End If
            SAPDAL.SAPDAL.GetSAPProductInfo(dthl.org, dtQuoteDetail)
            Dim phaseoutitems As DataRow() = dtQuoteDetail.Select("isPhaseout=1 and itemType<>'" & COMM.Fixer.eItemType.Parent & "'")
            For Each row As DataRow In phaseoutitems
                errormessage += String.Format("<br /><font color=""red"">{0}</font> is Phaseout", row("partNo"))
                'dtQuoteDetail.Rows.Remove(row)
            Next

            dtQuoteDetail.Columns.Remove("isPhaseout")
            dtQuoteDetail.Columns.Remove("StatusCode")
            dtQuoteDetail.Columns.Remove("StatusDesc")
            dtQuoteDetail.Columns.Remove("ATPQty")
            'Else
            '    errormessage += "get sap stock status error"
            '    Return ""
            'End If
            If dtQuoteDetail.Rows.Count > 0 Then
                'Ming copy Frank 2013/09/5: Upgrade V2.0 quotation detail data to V2.5
                Dim QM As IBUS.iDocHeader = New DOCH.DocHeader
                Dim _IsV2_0Quot As Boolean = QM.IsV2_0Quotation(quoteid)


                For Each _row As DataRow In dtQuoteDetail.Rows
                    If _row.Item("line_No") Mod 100 = 0 Then
                        _row.Item("partNo") = replaceCartBTO(_row.Item("partNo"), dthl.org)
                    End If

                    'Pivot.CurrentProfile.CurrDocReg
                    'Frank
                    If ((Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.ATW) = Pivot.CurrentProfile.CurrDocReg) _
                        OrElse ((Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.ACN) = Pivot.CurrentProfile.CurrDocReg) Then
                        If IsDBNull(_row.Item("VirtualpartNo")) OrElse String.IsNullOrEmpty(_row.Item("VirtualpartNo").ToString) Then
                            _row.Item("VirtualPartNo") = _row.Item("partNo")
                        End If
                    End If

                    If _IsV2_0Quot Then
                        If _row.Item("line_No") = 100 Then
                            _row.Item("ItemType") = 1
                        Else
                            _row.Item("ItemType") = 0
                        End If
                    End If

                Next
                dtQuoteDetail.AcceptChanges()

                'If _IsV2_0Quot Then
                '    For Each _row As DataRow In dtQuoteDetail.Rows
                '        If _row.Item("line_No") = 100 Then
                '            _row.Item("ItemType") = 1
                '        Else
                '            _row.Item("ItemType") = 0
                '        End If
                '    Next
                '    dtQuoteDetail.AcceptChanges()
                'End If

                'update price
                Dim aCompList As New ArrayList
                For Each r As DataRow In dtQuoteDetail.Rows
                    If r.Item("itemType") <> COMM.Fixer.eItemType.Parent Then
                        aCompList.Add(r.Item("partNo"))
                    End If
                Next
                Dim strPNs As String = String.Join("|", aCompList.ToArray())
                Dim dtPrice As DataTable = Nothing
                'SAPTools.getSAPPriceByTable(strPNs, dth.Rows(0)("org"), dth.Rows(0)("quoteToErpId"), dtPrice, errormessage)
                Dim company_id As String = dthl.AccErpId
                Dim org_id As String = dthl.org
                Dim ShipTo As String = GetShipToByQuoteID(quoteid)
                GetPriceBiz(company_id, ShipTo, org_id, dthl.currency, "", dthl.AccOfficeName, strPNs, dthl.CreatedBy, dtPrice, dthl.DocReg)

                If dtPrice IsNot Nothing AndAlso dtPrice.Rows.Count > 0 Then
                    Dim pricerow As DataRow()
                    For Each r As DataRow In dtQuoteDetail.Rows

                        r.Item("quoteId") = NewQuoteid

                        'Frank 20160129: AKR did not maintain assembly part's price, so bring out the price from our code 
                        If String.Equals(org_id, "KR01", StringComparison.InvariantCultureIgnoreCase) _
                            AndAlso r.Item("partNo") IsNot DBNull.Value Then
                            Dim _price As Integer = PartBusinessLogic.GetKR01AssemblyPartPrice(r.Item("partNo").ToString)
                            If _price > 0 Then
                                r.Item("newunitPrice") = _price
                                r.Item("listPrice") = _price
                                r.Item("unitPrice") = _price
                                r.Item("RecyclingFee") = 0
                                Continue For
                            End If
                        End If

                        'Ryan 20171102 Bypass price recalculation for AJP quotations
                        If Role.IsJPAonlineSales Then
                            Continue For
                        End If

                        pricerow = dtPrice.Select(String.Format("MATNR='{0}'", r.Item("partNo")))
                        If r.Item("itemType") <> COMM.Fixer.eItemType.Parent Then
                            If pricerow.Count > 0 Then
                                r.Item("newunitPrice") = pricerow(0)("Netwr")
                                r.Item("listPrice") = pricerow(0)("Kzwi1")
                                r.Item("unitPrice") = pricerow(0)("Netwr")
                                r.Item("RecyclingFee") = 0
                                'Ming 20150603 RecyclingFee
                                If String.Equals(org_id, "US01", StringComparison.InvariantCultureIgnoreCase) Then
                                    Dim _RecycleFee As Decimal = 0
                                    If Not IsDBNull(pricerow(0).Item("RECYCLE_FEE")) AndAlso Decimal.TryParse(pricerow(0).Item("RECYCLE_FEE").ToString(), _RecycleFee) Then
                                        r.Item("RecyclingFee") = _RecycleFee
                                        r.Item("listPrice") = pricerow(0)("Kzwi1") - _RecycleFee
                                        r.Item("unitPrice") = pricerow(0)("Netwr") - _RecycleFee
                                        r.Item("newunitPrice") = pricerow(0)("Netwr") - _RecycleFee
                                    End If
                                End If
                                'Ryan 20160802 USAonline users itp re-calculation
                                If Role.IsAonlineUsa Then
                                    Dim itp As Decimal = 0
                                    Business.isANAPnBelowGPV2(NewQuoteid, r.Item("partNo"), r.Item("unitPrice"), itp, "")
                                    r.Item("itp") = itp
                                    r.Item("newitp") = itp
                                End If

                                If String.Equals(org_id, "KR01", StringComparison.InvariantCultureIgnoreCase) Then
                                    r.Item("itp") = pricerow(0)("VPRS")
                                    r.Item("newitp") = pricerow(0)("VPRS")
                                End If

                                'Ryan 20171102 AJP copied quotations don't need to recalculate price, comment below out.
                                'Ryan 20171005 Add for AJP special PTrade items logic
                                'If Role.IsJPAonlineSales AndAlso IsPTD(r.Item("partNo")) Then
                                '    Dim AJPPtradePrice As Decimal = Business.GetAJPPTradePrice(r.Item("UnitPrice"), r.Item("itp"))
                                '    If r.Item("UnitPrice") < AJPPtradePrice Then
                                '        r.Item("ListPrice") = AJPPtradePrice
                                '        r.Item("UnitPrice") = AJPPtradePrice
                                '        r.Item("newUnitPrice") = AJPPtradePrice
                                '    End If
                                'End If
                            Else
                                errormessage += String.Format("<br /><font color=""red"">{0}</font> is Phaseout", r("partNo"))
                            End If
                        End If

                    Next
                Else
                    errormessage += "update price error"
                    Return False
                End If


                'Re-Calculate Extended Warranty Price
                Try
                    'Get All BTOS Parent Line No which contains EW items
                    Dim TargetParentLineNo As List(Of Integer) = New List(Of Integer)
                    Dim EWU As IBUS.iEWUtil = New DOCH.EWUtil
                    For Each r As DataRow In dtQuoteDetail.Select("partNo like 'AGS-EW*'")
                        TargetParentLineNo.Add(r.Item("Higherlevel"))
                    Next

                    'Recursive processing all target systems
                    For Each i As Integer In TargetParentLineNo
                        Dim TotalBTOSWarrantableAmount As Decimal = 0
                        Dim EWMonth As Integer = 0
                        For Each _row As DataRow In dtQuoteDetail.Select("Higherlevel = '" & i & "' and itemType<>'" & COMM.Fixer.eItemType.Parent & "'")
                            If SAPDAL.CommonLogic.isWarrantableV3(_row.Item("partNo"), org_id) Then
                                EWMonth = _row.Item("ewFlag")
                                TotalBTOSWarrantableAmount = TotalBTOSWarrantableAmount + (_row.Item("newunitPrice") * _row.Item("qty"))
                            End If
                        Next

                        Dim EWRow As DataRow = dtQuoteDetail.Select("partNo like 'AGS-EW*' and Higherlevel = '" & i & "'").FirstOrDefault()
                        If EWRow IsNot Nothing AndAlso EWRow.Item("newunitPrice") IsNot Nothing Then
                            EWRow.Item("newunitPrice") = TotalBTOSWarrantableAmount / EWRow.Item("qty") * EWU.getRateByEWItem(EWU.getEWItemByMonth(EWMonth, Pivot.CurrentProfile.CurrDocReg), Pivot.CurrentProfile.CurrDocReg)
                            EWRow.Item("unitPrice") = EWRow.Item("newunitPrice")
                            EWRow.Item("listPrice") = EWRow.Item("newunitPrice")
                        End If
                    Next
                Catch ex As Exception
                    Advantech.Myadvantech.DataAccess.Common.SendMailUtil.SendMail("yl.huang@advantech.com.tw", "eQuotation Exception while recalculating EW price in CopyQuotation Function", "Quote ID:" + quoteid + "New QuoteID: " + NewQuoteid + "Error: " + ex.ToString)
                Finally
                    dtQuoteDetail.AcceptChanges()
                End Try

                'Copy CTOS_CONFIG_LOG and update parent detail's reconfig id
                Try
                    For Each r As DataRow In dtQuoteDetail.Rows
                        If r.Item("itemType") = COMM.Fixer.eItemType.Parent AndAlso Not String.IsNullOrEmpty(r.Item("RECFIGID")) Then
                            If CInt(tbOPBase.dbExecuteScalar("EQ", String.Format("select count(*) as c FROM [CTOS_CONFIG_LOG] where ROW_ID = '{0}'", r.Item("RECFIGID")))) = 1 Then
                                Dim NewCTOSRowID As String = System.Guid.NewGuid.ToString().Replace("-", "").Substring(0, 15) + DateTime.Now.ToString("_yyyyMMddHHmmssfff")
                                Dim sql_CTOS As New StringBuilder
                                sql_CTOS.AppendLine("   INSERT INTO [CTOS_CONFIG_LOG] ")
                                sql_CTOS.AppendFormat(" SELECT TOP 1 '{0}' as [ROW_ID], [ROOT_CATEGORY_ID], [CONFIG_QTY], '{1}' as [USERID], [COMPANY_ID], [ORG_ID], [CONFIG_DATE], [CONFIG_TREE_HTML], '{2}' as [CART_ID], [ISONELEVEL] ", NewCTOSRowID, HttpContext.Current.User.Identity.Name, NewQuoteid)
                                sql_CTOS.AppendFormat(" From [CTOS_CONFIG_LOG] where ROW_ID = '{0}'", r.Item("RECFIGID"))
                                tbOPBase.dbExecuteScalar("EQ", sql_CTOS.ToString)

                                r.Item("RECFIGID") = NewCTOSRowID
                            Else
                                r.Item("RECFIGID") = String.Empty
                            End If
                        End If
                    Next
                Catch ex As Exception
                    Advantech.Myadvantech.DataAccess.Common.SendMailUtil.SendMail("yl.huang@advantech.com.tw", "eQuotation Exception while coping CTOS_CONFIG_LOG in CopyQuotation Function", "Quote ID:" + quoteid + "New QuoteID: " + NewQuoteid + "Error: " + ex.ToString)
                Finally
                    dtQuoteDetail.AcceptChanges()
                End Try


                Dim myEQPARTNER As New EQPARTNER("EQ", "EQPARTNER")
                Dim dtEQPARTNER As New DataTable
                dtEQPARTNER = myEQPARTNER.GetDT(String.Format("QuoteId='{0}' ", quoteid), "")
                If dtEQPARTNER.Rows.Count > 0 Then
                    For Each r As DataRow In dtEQPARTNER.Rows
                        r.BeginEdit()
                        r.Item("quoteId") = NewQuoteid
                        r.EndEdit()
                        r.AcceptChanges()
                    Next
                    dtEQPARTNER.AcceptChanges()
                Else
                    errormessage = "Invalid EQPARTNER!"
                    Return False
                End If
                'Copy sales note and order note
                Dim _noteDA As New EQDSTableAdapters.QuotationNoteTableAdapter()
                Dim _dt As DataTable = _noteDA.GetNoteTextBYQuoteId(quoteid)
                For Each _row As DataRow In _dt.Rows
                    _noteDA.InsertQuotationNote(NewQuoteid, _row.Item("notetype").ToString.ToUpper, _row.Item("notetext").ToString)
                Next
                'copy
                myQuoteDetail.Delete(String.Format("quoteId='{0}'", NewQuoteid))
                myEQPARTNER.Delete(String.Format("quoteId='{0}'", NewQuoteid))
                myQuoteDetail.SqlBulkCopy(dtQuoteDetail)
                myEQPARTNER.SqlBulkCopy(dtEQPARTNER)

                '\Ming20150507 仿Frank Copy quotation extension record to new revision
                Dim _QuotationExtension As Quote_Master_Extension = MyQuoteX.GetMasterExtension(quoteid)
                If _QuotationExtension IsNot Nothing Then
                    Dim _ME As New Quote_Master_Extension
                    _ME.QuoteID = NewQuoteid
                    _ME.EmailGreeting = _QuotationExtension.EmailGreeting
                    _ME.SpecialTandC = _QuotationExtension.SpecialTandC
                    _ME.SignatureRowID = _QuotationExtension.SignatureRowID
                    _ME.LastUpdatedBy = Pivot.CurrentProfile.UserId
                    _ME.LastUpdated = Now
                    _ME.Engineer = _QuotationExtension.Engineer
                    _ME.Engineer_Telephone = _QuotationExtension.Engineer_Telephone
                    _ME.Warranty = _QuotationExtension.Warranty
                    If _QuotationExtension.IsShowTotal IsNot Nothing AndAlso _QuotationExtension.IsShowTotal = True Then
                        _ME.IsShowTotal = 1
                    Else
                        _ME.IsShowTotal = 0
                    End If
                    _ME.CopyPurpose = 0
                    MyQuoteX.LogQuoteMasterExtension(_ME)
                End If
                'Ming20150507 Copy opty-quote record to new revision
                'Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
                'Dim OptyQuotedt As EQDS.optyQuoteDataTable = myOptyQuote.GetOptyQuoteByOuoteID(quoteid)
                'If OptyQuotedt IsNot Nothing AndAlso OptyQuotedt.Rows.Count > 0 Then
                '    Dim _optyQuoteRow As EQDS.optyQuoteRow = OptyQuotedt.Rows(0)
                '    myOptyQuote.InsertOptyQuote(_optyQuoteRow.optyId, _optyQuoteRow.optyName, NewQuoteid, _optyQuoteRow.optyStage)
                'End If
                '/end

            Else
                errormessage += String.Format("<br />has no availabe items")
            End If
        End If
        Return True
    End Function

    Shared Function CopyAEUQuotationV2(ByVal quoteid As String, ByRef NewQuoteid As String, ByRef errormessage As String, ByVal oType As COMM.Fixer.eDocType, ByVal CopyPurpose As Integer) As Boolean
        If Not Business.isValidQuote(quoteid, COMM.Fixer.eDocType.EQ) Then
            errormessage = "Quote Id does not exist,or quote status is invalid."
            Return False
        End If
        Dim O As IBUS.iDoc = Pivot.NewObjDoc
        NewQuoteid = O.NewUID
        Dim dth As IBUS.iDocHeader = Pivot.NewObjDocHeader
        dth.CopyPasteHeaderLine(quoteid, NewQuoteid, Pivot.CurrentProfile, oType)
        Dim NewDocHeader As IBUS.iDocHeaderLine = dth.GetByDocID(quoteid, oType)
        If Not IsNothing(dth) Then
            Dim myQuoteDetail As New quotationDetail("EQ", "quotationDetail")
            Dim dtQuoteDetail As DataTable = myQuoteDetail.GetDT(String.Format("quoteId='{0}'", quoteid), "line_No")
            If dtQuoteDetail.Rows.Count = 0 Then
                errormessage = "No any product can be copied from original quotation"
                Return False
            End If
            SAPDAL.SAPDAL.GetSAPProductInfo(NewDocHeader.org, dtQuoteDetail)
            Dim phaseoutitems As DataRow() = dtQuoteDetail.Select("isPhaseout=1 and itemType<>'" & COMM.Fixer.eItemType.Parent & "'")
            For Each row As DataRow In phaseoutitems
                errormessage += String.Format("<br /><font color=""red"">{0}</font> is Phaseout", row("partNo"))
                dtQuoteDetail.Rows.Remove(row)
            Next
            dtQuoteDetail.Columns.Remove("isPhaseout")
            dtQuoteDetail.Columns.Remove("StatusCode")
            dtQuoteDetail.Columns.Remove("StatusDesc")
            dtQuoteDetail.Columns.Remove("ATPQty")
            If dtQuoteDetail.Rows.Count > 0 Then
                'Ming copy Frank 2013/09/5: Upgrade V2.0 quotation detail data to V2.5
                Dim QM As IBUS.iDocHeader = New DOCH.DocHeader
                Dim _IsV2_0Quot As Boolean = QM.IsV2_0Quotation(quoteid)
                If _IsV2_0Quot Then
                    For Each _row As DataRow In dtQuoteDetail.Rows
                        If _row.Item("line_No") = 100 Then
                            _row.Item("ItemType") = 1
                        Else
                            _row.Item("ItemType") = 0
                        End If
                    Next
                    dtQuoteDetail.AcceptChanges()
                End If
                '\Ming 20150415 update price
                Dim aCompList As New ArrayList
                For Each r As DataRow In dtQuoteDetail.Rows
                    If r.Item("itemType") <> COMM.Fixer.eItemType.Parent Then
                        aCompList.Add(r.Item("partNo"))
                    End If
                Next
                Dim strPNs As String = String.Join("|", aCompList.ToArray())
                Dim dtPrice As DataTable = Nothing
                Dim company_id As String = NewDocHeader.AccErpId
                Dim org_id As String = NewDocHeader.org
                Dim ITP As Decimal = 0
                GetPriceBiz(company_id, company_id, org_id, NewDocHeader.currency, "", NewDocHeader.AccOfficeName, strPNs, NewDocHeader.CreatedBy, dtPrice, NewDocHeader.DocReg)
                If dtPrice IsNot Nothing AndAlso dtPrice.Rows.Count > 0 Then
                    Dim pricerow As DataRow()
                    For Each r As DataRow In dtQuoteDetail.Rows
                        ITP = 0
                        pricerow = dtPrice.Select(String.Format("MATNR='{0}'", r.Item("partNo")))
                        If r.Item("itemType") <> COMM.Fixer.eItemType.Parent Then
                            If pricerow.Count > 0 Then
                                r.Item("newunitPrice") = pricerow(0)("Netwr")
                                r.Item("listPrice") = pricerow(0)("Kzwi1")
                                r.Item("unitPrice") = pricerow(0)("Netwr")
                                Business.GetITP(NewQuoteid, r.Item("partNo"), ITP, pricerow(0)("Netwr"))
                                r.Item("Itp") = ITP
                                r.Item("newItp") = ITP
                            Else
                                errormessage += String.Format("<br /><font color=""red"">{0}</font> is Phaseout", r("partNo"))
                            End If
                        End If
                        r.Item("quoteId") = NewQuoteid
                    Next
                    dtQuoteDetail.AcceptChanges()
                Else
                    errormessage += "update price error"
                    Return False
                End If
                '/
                Dim myEQPARTNER As New EQPARTNER("EQ", "EQPARTNER")
                Dim dtEQPARTNER As New DataTable
                dtEQPARTNER = myEQPARTNER.GetDT(String.Format("QuoteId='{0}' ", quoteid), "")
                If dtEQPARTNER.Rows.Count > 0 Then
                    For Each r As DataRow In dtEQPARTNER.Rows
                        r.BeginEdit()
                        r.Item("quoteId") = NewQuoteid
                        r.EndEdit()
                        r.AcceptChanges()
                    Next
                    dtEQPARTNER.AcceptChanges()
                Else
                    errormessage = "Invalid EQPARTNER!"
                    Return False
                End If
                'Copy sales note and order note
                Dim _noteDA As New EQDSTableAdapters.QuotationNoteTableAdapter()
                Dim _dt As DataTable = _noteDA.GetNoteTextBYQuoteId(quoteid)
                For Each _row As DataRow In _dt.Rows
                    _noteDA.InsertQuotationNote(NewQuoteid, _row.Item("notetype").ToString.ToUpper, _row.Item("notetext").ToString)
                Next
                'copy
                For Each r As DataRow In dtQuoteDetail.Rows
                    r.Item("quoteId") = NewQuoteid
                Next
                myQuoteDetail.Delete(String.Format("quoteId='{0}'", NewQuoteid))
                myEQPARTNER.Delete(String.Format("quoteId='{0}'", NewQuoteid))
                myQuoteDetail.SqlBulkCopy(dtQuoteDetail)
                myEQPARTNER.SqlBulkCopy(dtEQPARTNER)
                Dim _QuotationExtension As Quote_Master_Extension = MyQuoteX.GetMasterExtension(quoteid)
                If _QuotationExtension IsNot Nothing Then
                    Dim _ME As New Quote_Master_Extension
                    _ME.QuoteID = NewQuoteid
                    _ME.EmailGreeting = _QuotationExtension.EmailGreeting
                    _ME.SpecialTandC = _QuotationExtension.SpecialTandC
                    _ME.SignatureRowID = _QuotationExtension.SignatureRowID
                    _ME.LastUpdatedBy = Pivot.CurrentProfile.UserId
                    _ME.LastUpdated = Now
                    _ME.Engineer = _QuotationExtension.Engineer
                    _ME.Engineer_Telephone = _QuotationExtension.Engineer_Telephone
                    If _QuotationExtension.IsShowTotal IsNot Nothing AndAlso _QuotationExtension.IsShowTotal = True Then
                        _ME.IsShowTotal = 1
                    Else
                        _ME.IsShowTotal = 0
                    End If

                    'Ryan 20161228 Add for copy purpose saving and opty quote coping if copy purpose = 2.
                    _ME.CopyPurpose = CopyPurpose
                    If CopyPurpose = 2 Then
                        Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
                        Dim OptyQuotedt As EQDS.optyQuoteDataTable = myOptyQuote.GetOptyQuoteByOuoteID(quoteid)
                        If OptyQuotedt IsNot Nothing AndAlso OptyQuotedt.Rows.Count > 0 Then
                            Dim _optyQuoteRow As EQDS.optyQuoteRow = OptyQuotedt.Rows(0)
                            myOptyQuote.InsertOptyQuote(_optyQuoteRow.optyId, _optyQuoteRow.optyName, NewQuoteid, _optyQuoteRow.optyStage)
                        End If
                    End If

                    MyQuoteX.LogQuoteMasterExtension(_ME)
                End If
                'Ming add 20140304 check GPcontrol
                'Dim itpType As Decimal = SAPDAL.SAPDAL.itpType.EU
                'Dim detail As New List(Of struct_GP_Detail) ', decQuoteTotal As Decimal = 0
                'For Each x As DataRow In dtQuoteDetail.Rows
                '    Dim detailLine As New struct_GP_Detail
                '    detailLine.lineNo = x.Item("line_No") : detailLine.PartNo = x.Item("partNo") : detailLine.Price = x.Item("newUnitPrice")
                '    detailLine.QTY = x.Item("qty") : detailLine.Itp = x.Item("newItp") : detail.Add(detailLine)
                '    'decQuoteTotal += x.unitPrice * x.qty
                'Next
                '            If Business.isNeedGPControl(NewQuoteid, NewDocHeader.org) And _
                '(GPControl.getLevel(NewDocHeader.AccRowId, NewDocHeader.AccErpId, detail, itpType, NewDocHeader.AccOfficeName, NewDocHeader.AccGroupName) > 0 Or (NewDocHeader.org = "EU10" And GPControl.isDLGR(detail))) Then
                '            Else
                '                Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(NewQuoteid)
                '                If _QM IsNot Nothing Then
                '                    _QM.qstatus = "FINISH"
                '                    _QM.DOCSTATUS = COMM.Fixer.eDocStatus.QFINISH
                '                    MyUtil.Current.CurrentDataContext.SubmitChanges()
                '                End If
                '            End If
                'end
            Else
                errormessage += String.Format("<br />has no availabe items")
            End If
        End If
        'Ming 20140919    Get sales email
        Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(NewQuoteid)
        If _QM IsNot Nothing Then
            _QM.salesEmail = SiebelTools.getPriSalesEmailByAccountROWID(_QM.quoteToRowId)
        End If
        MyUtil.Current.CurrentDataContext.SubmitChanges()
        Return True
    End Function
    Shared Function CreateNewRevisionQuotation(ByVal quoteid As String, ByVal createby As String, ByRef NewQuoteid As String, ByVal IsReloadPart As Boolean, ByRef errormessage As String) As Boolean
        If Not Business.isValidQuote(quoteid, COMM.Fixer.eDocType.EQ) Then
            errormessage = "Invalid Ref Id!"
            Return 0
        End If

        'Frank 2013/07/02
        'NewQuoteid = Business.GetNoByPrefix(Pivot.CurrentProfile)

        Dim dth As IBUS.iDocHeader = Pivot.NewObjDocHeader
        Dim iDocHL As IBUS.iDocHeaderLine = dth.ReviseHeaderLine(quoteid, createby, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)

        NewQuoteid = iDocHL.Key

        Dim dthl As IBUS.iDocHeaderLine = dth.GetByDocID(quoteid, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(iDocHL) Then

            'Frank 2013/07/02
            'Load quote detail
            Dim myQuoteDetail As New quotationDetail("EQ", "quotationDetail")
            Dim dtQuoteDetail As DataTable = myQuoteDetail.GetDT(String.Format("quoteId='{0}'", quoteid), "line_No")

            For Each row As DataRow In dtQuoteDetail.Rows
                row.Item("quoteid") = NewQuoteid
            Next

            'Load quote partner
            Dim myEQPARTNER As New EQPARTNER("EQ", "EQPARTNER")
            Dim dtEQPARTNER As New DataTable
            dtEQPARTNER = myEQPARTNER.GetDT(String.Format("QuoteId='{0}' ", quoteid), "")

            For Each row As DataRow In dtEQPARTNER.Rows
                row.Item("quoteid") = NewQuoteid
            Next

            If IsReloadPart Then

                SAPDAL.SAPDAL.GetSAPProductInfo(dthl.org, dtQuoteDetail)
                Dim phaseoutitems As DataRow() = dtQuoteDetail.Select("isPhaseout=1 and itemType<>'" & COMM.Fixer.eItemType.Parent & "'")
                For Each row As DataRow In phaseoutitems
                    errormessage += String.Format("<br /><font color=""red"">{0}</font> is Phaseout", row("partNo"))
                    dtQuoteDetail.Rows.Remove(row)
                Next

                dtQuoteDetail.Columns.Remove("isPhaseout")
                dtQuoteDetail.Columns.Remove("StatusCode")
                dtQuoteDetail.Columns.Remove("StatusDesc")
                dtQuoteDetail.Columns.Remove("ATPQty")
                'Else
                '    errormessage += "get sap stock status error"
                '    Return ""
                'End If
                If dtQuoteDetail.Rows.Count > 0 Then
                    'update price
                    Dim aCompList As New ArrayList
                    For Each r As DataRow In dtQuoteDetail.Rows
                        If r.Item("itemType") <> COMM.Fixer.eItemType.Parent Then
                            aCompList.Add(r.Item("partNo"))
                        End If
                    Next
                    Dim strPNs As String = String.Join("|", aCompList.ToArray())
                    Dim dtPrice As DataTable = Nothing
                    'SAPTools.getSAPPriceByTable(strPNs, dth.Rows(0)("org"), dth.Rows(0)("quoteToErpId"), dtPrice, errormessage)
                    Dim company_id As String = dthl.AccErpId
                    Dim org_id As String = dthl.org
                    Dim ShipTo As String = GetShipToByQuoteID(quoteid)
                    GetPriceBiz(company_id, ShipTo, org_id, dthl.currency, "", dthl.AccOfficeName, strPNs, dthl.CreatedBy, dtPrice, dthl.DocReg)

                    If dtPrice IsNot Nothing AndAlso dtPrice.Rows.Count > 0 Then
                        Dim pricerow As DataRow()
                        For Each r As DataRow In dtQuoteDetail.Rows
                            pricerow = dtPrice.Select(String.Format("MATNR='{0}'", r.Item("partNo")))
                            If r.Item("itemType") <> COMM.Fixer.eItemType.Parent Then
                                If pricerow.Count > 0 Then
                                    r.Item("newunitPrice") = pricerow(0)("Netwr")
                                    r.Item("listPrice") = pricerow(0)("Kzwi1")
                                    r.Item("unitPrice") = pricerow(0)("Netwr")
                                    r.Item("RecyclingFee") = 0
                                    'Ming 20150604 RecyclingFee
                                    If String.Equals(org_id, "US01", StringComparison.InvariantCultureIgnoreCase) Then
                                        Dim _RecycleFee As Decimal = 0
                                        If Not IsDBNull(pricerow(0).Item("RECYCLE_FEE")) AndAlso Decimal.TryParse(pricerow(0).Item("RECYCLE_FEE").ToString(), _RecycleFee) Then
                                            r.Item("RecyclingFee") = _RecycleFee
                                        End If
                                    End If
                                Else
                                    errormessage += String.Format("<br /><font color=""red"">{0}</font> is Phaseout", r("partNo"))
                                End If
                            End If
                            r.Item("quoteId") = NewQuoteid
                        Next
                    Else
                        errormessage += "update price error"
                        Return 0
                    End If

                    If dtEQPARTNER.Rows.Count > 0 Then
                        For Each r As DataRow In dtEQPARTNER.Rows
                            r.BeginEdit()
                            r.Item("quoteId") = NewQuoteid
                            r.EndEdit()
                            r.AcceptChanges()
                        Next
                        dtEQPARTNER.AcceptChanges()
                    Else
                        errormessage = "Invalid EQPARTNER!"
                        Return 0
                    End If
                    'Copy sales note and order note
                    Dim _noteDA As New EQDSTableAdapters.QuotationNoteTableAdapter()
                    Dim _dt As DataTable = _noteDA.GetNoteTextBYQuoteId(quoteid)
                    For Each _row As DataRow In _dt.Rows
                        _noteDA.InsertQuotationNote(NewQuoteid, _row.Item("notetype").ToString.ToUpper, _row.Item("notetext").ToString)
                    Next
                    'copy
                End If

            End If

            myQuoteDetail.Delete(String.Format("quoteId='{0}'", NewQuoteid))
            myEQPARTNER.Delete(String.Format("quoteId='{0}'", NewQuoteid))
            myQuoteDetail.SqlBulkCopy(dtQuoteDetail)
            myEQPARTNER.SqlBulkCopy(dtEQPARTNER)

            'Ryan 20160802 USAonline users itp re-calculation
            If Role.IsAonlineUsa Then
                Dim QD As DataTable = myQuoteDetail.GetDT(String.Format("quoteId='{0}'", NewQuoteid), "line_No")
                Dim itp As Decimal = 0

                For Each r As DataRow In QD.Rows
                    Business.isANAPnBelowGPV2(NewQuoteid, r.Item("partNo"), r.Item("unitPrice"), itp, "")
                    r.BeginEdit()
                    r.Item("itp") = itp
                    r.Item("newitp") = itp
                    r.EndEdit()
                    r.AcceptChanges()
                Next
                myQuoteDetail.Delete(String.Format("quoteId='{0}'", NewQuoteid))
                myQuoteDetail.SqlBulkCopy(QD)
            End If

            'Frank 20150507 Copy quotation extension record to new revision
            Dim _QuotationExtension As Quote_Master_Extension = MyQuoteX.GetMasterExtension(quoteid)
            If _QuotationExtension IsNot Nothing Then
                Dim _ME As New Quote_Master_Extension
                _ME.QuoteID = NewQuoteid
                _ME.EmailGreeting = _QuotationExtension.EmailGreeting
                _ME.SpecialTandC = _QuotationExtension.SpecialTandC
                _ME.SignatureRowID = _QuotationExtension.SignatureRowID
                _ME.LastUpdatedBy = Pivot.CurrentProfile.UserId
                _ME.LastUpdated = Now
                _ME.Engineer = _QuotationExtension.Engineer
                _ME.Engineer_Telephone = _QuotationExtension.Engineer_Telephone
                _ME.Warranty = _QuotationExtension.Warranty
                _ME.ABRQuoteType = _QuotationExtension.ABRQuoteType
                If _QuotationExtension.IsShowTotal IsNot Nothing AndAlso _QuotationExtension.IsShowTotal = True Then
                    _ME.IsShowTotal = 1
                Else
                    _ME.IsShowTotal = 0
                End If
                _ME.CopyPurpose = 0
                MyQuoteX.LogQuoteMasterExtension(_ME)
            End If

            'Frank 20150507 Copy opty-quote record to new revision
            Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
            Dim OptyQuotedt As EQDS.optyQuoteDataTable = myOptyQuote.GetOptyQuoteByOuoteID(quoteid)
            If OptyQuotedt IsNot Nothing AndAlso OptyQuotedt.Rows.Count > 0 Then
                Dim _optyQuoteRow As EQDS.optyQuoteRow = OptyQuotedt.Rows(0)
                myOptyQuote.InsertOptyQuote(_optyQuoteRow.optyId, _optyQuoteRow.optyName, NewQuoteid, _optyQuoteRow.optyStage)
            End If

            'Frank 20150518 copy Quotation Note
            Dim quotenotedt As DataTable = tbOPBase.dbGetDataTable("EQ", String.Format("SELECT  quoteid,notetype,notetext from QuotationNote where QuoteID ='{0}' ", quoteid))
            If quotenotedt IsNot Nothing AndAlso quotenotedt.Rows.Count > 0 Then
                For Each _row As DataRow In quotenotedt.Rows
                    _row.Item("quoteid") = NewQuoteid
                Next
            End If
            Dim bk As New System.Data.SqlClient.SqlBulkCopy(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
            bk.DestinationTableName = "QuotationNote"
            bk.WriteToServer(quotenotedt)

        Else
            errormessage += String.Format("<br />has no availabe items")
        End If



        Return 1
    End Function

End Class
