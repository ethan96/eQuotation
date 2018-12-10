Public Class CreditInfo
    Inherits System.Web.UI.Page

    Public ERPID As String = "", ORG As String = "", CE As Decimal = 0, CL As Decimal = 0, P As String = ""
    Public _IsUSAENCUser As Boolean = False, _IsABRUser As Boolean = False, _IsATWUser As Boolean = False
    Sub showCreditInfo(ByVal companyid As String)
        If companyid <> "" Then
            Dim org As String = Business.getOrgByErpId(companyid)
            If org <> "" Then
                ''Dim ws As New MySAPDALWS.MYSAPDAL
                'Dim ws As New SAPDAL.SAPDAL
                ''ws.Timeout = -1 : ws.GetCustomerCreditExposure(companyid, org, CL, CE, P) : ws.Dispose()
                'If _IsUSAENCUser Then
                '    ws.GetCustomerCreditExposure(companyid, "USAENC", CL, CE, P)
                'Else
                '    ws.GetCustomerCreditExposure(companyid, org, CL, CE, P)
                'End If
                Dim _temporg As String = org
                If _IsUSAENCUser Then
                    org = "USAENC"
                End If

                'ws.Dispose()

                Dim cld As Advantech.Myadvantech.DataAccess.CreditLimitData =
                Advantech.Myadvantech.Business.QuoteBusinessLogic.GetCustomerCreditExposure(companyid, org)

                Me.lbCreditControlArea.Text = cld.CreditControlAreaOption.ToString
                Me.lbCurrency.Text = cld.Currency.ToString
                Me.lbCreditLimit.Text = cld.CreditLimit
                Me.lbCreditExposure.Text = cld.CreditExposure
                'Me.lbPercentage.Text = cld.Percentage
                Me.lbPercentage.Text = Math.Round(cld.CreditLimitUsed * 100, 2, MidpointRounding.AwayFromZero) & " %"
                Me.lbReceivables.Text = cld.Receivables
                Me.lbSpecialLiability.Text = cld.SpecialLiability
                Me.lbSalesValue.Text = cld.SalesValue
                Me.lbRiskCategory.Text = cld.RiskCategory

                If cld.Blocked Then
                    Me.lbBlocked.Text = "Yes"
                Else
                    Me.lbBlocked.Text = ""
                End If



                'ws.GetCustomerCreditExposure(companyid, org)


                'If _IsABRUser Then
                '    Dim creditdataBR01 As New CreditLimitData(companyid, CreditControlAreaOptions.BR01)
                '    Dim creditDataList As New List(Of CreditLimitData)
                '    creditdataBR01.GetCreditData()
                '    creditDataList.Add(creditdataBR01)
                '    'vCustCredit.DataSource = creditDataList : gvCustCredit.DataBind()
                'End If

                Exit Sub
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        _IsUSAENCUser = Role.IsUsaAenc
        _IsABRUser = Role.IsABRSales
        _IsATWUser = Role.IsTWAonlineSales
        Me.customer_ID = Request("ID")
        showCreditInfo(Request("ID"))

        If _IsATWUser Then

            Dim txtDateFrom As String = DateConvert(CDate(FormatDate(Date.Now.AddDays(-180))))
            Dim txtDateTo As String = DateConvert(CDate(FormatDate(Date.Now)))
            Dim txtDueDateFrom As String = DateConvert(CDate(FormatDate(Date.Now.AddDays(-90))))
            Dim txtDueDateTo As String = DateConvert(CDate(FormatDate(Date.Now.AddDays(90))))

            Me.Search("TW01", "10", "00", Me.customer_ID, txtDateFrom, txtDateTo, txtDueDateFrom, txtDueDateTo)
            Me.gv1.Visible = True
            Me.gv1.DataSource = table
            Me.gv1.DataBind()
        End If

    End Sub


    'Public Class CreditLimitData
    '    Public Property CustomerId As String : Public CreditControlAreaOption As CreditControlAreaOptions : Public Property HorizonDate As Date
    '    Public Property Currency As String : Public Property CreditLimit As Decimal : Public Property Delta2Limit As Decimal : Public Property Percentage As String
    '    Public Property Receivables As Decimal : Public Property SpecialLiability As Decimal
    '    Public Property OpenDelivery As Decimal : Public Property OpenDeliverySecure As Decimal : Public Property OpenInvoice As Decimal
    '    Public Property OpenInvoiceSecure As Decimal : Public Property OpenOrders As Decimal : Public Property OpenOrderSecure As Decimal : Public Property SumOpen As Decimal
    '    Public Property RiskCategory As String : Public Property Blocked As Boolean
    '    Public ReadOnly Property CreditControlArea As String
    '        Get
    '            Return Me.CreditControlAreaOption.ToString()
    '        End Get
    '    End Property
    '    Public ReadOnly Property CreditExposure As Decimal
    '        Get
    '            Return CreditLimit - Delta2Limit
    '        End Get
    '    End Property
    '    Public ReadOnly Property SalesValue As Decimal
    '        Get
    '            Return Me.CreditExposure - Receivables - SpecialLiability
    '        End Get
    '    End Property

    '    Public Sub New(CustomerId As String, CreditControlArea As CreditControlAreaOptions)
    '        Me.CustomerId = Trim(CustomerId).ToUpper() : Me.CreditControlAreaOption = CreditControlArea : Me.HorizonDate = New Date(9999, 12, 31)
    '    End Sub

    '    Public Sub New(CustomerId As String, CreditControlArea As CreditControlAreaOptions, HorizonDate As Date)
    '        Me.CustomerId = Trim(CustomerId).ToUpper() : Me.CreditControlAreaOption = CreditControlArea : Me.HorizonDate = HorizonDate
    '    End Sub

    '    Public Function GetCreditData() As Boolean
    '        Dim p As New GetCreditExposure.GetCreditExposure(ConfigurationManager.AppSettings("SAP_PRD"))
    '        Dim dtKnkk As GetCreditExposure.KNKK = Nothing, Knkli As String = ""
    '        p.Connection.Open()
    '        p.Zcredit_Exposure(HorizonDate.ToString("yyyyMMdd"), CreditControlArea, CustomerId, Currency, CreditLimit, Delta2Limit, dtKnkk, Knkli, OpenDelivery, OpenDeliverySecure, _
    '                   OpenInvoice, OpenInvoiceSecure, Receivables, OpenOrders, OpenOrderSecure, SpecialLiability, Percentage, SumOpen)

    '        Me.RiskCategory = dtKnkk.Ctlpc

    '        If dtKnkk.Crblb.Trim = "" Then
    '            Me.Blocked = False
    '        Else
    '            Me.Blocked = True
    '        End If



    '        p.Connection.Close()
    '        Return True
    '    End Function

    'End Class

    'Public Enum CreditControlAreaOptions
    '    CNC1
    '    CNC2
    '    CNC3
    '    CNC4
    '    CN01
    '    CN02
    '    CN08
    '    HK05
    '    ID01
    '    IN01
    '    EU01
    '    EU80
    '    USC1
    '    USC2
    '    TW01
    '    TW02
    '    TW03
    '    TW04
    '    TW05
    '    TW06
    '    TW07
    '    TW08
    '    TW09
    '    TW10
    '    TW16
    '    TW99
    '    JP01
    '    KR01
    '    MY01
    '    SG01
    '    TL01
    '    AU01
    '    BR01
    'End Enum

    Public dt_vbrk As New DataTable, dt_bsid As New DataTable, dt_bsad As New DataTable, table As New DataTable
    Public customer_ID As String = ""
    Public multiplier As Integer = 1
    Protected Sub Search(ByVal Sales_Org As String, ByVal discha As String, ByVal division As String, ByVal customer_id As String, ByVal SDate As String, ByVal EDate As String, ByVal S_Due_Date As String, ByVal E_Due_Date As String)
        Dim salesOrg As String = Session("org_id")
        Dim sb As New StringBuilder
        With sb

            'Frank 2013/05/30: Remove 3 sub select statements, they are too slow and not in used.
            '.AppendFormat("select distinct a.vkorg,a.vbeln,a.fkdat,nvl(a.netwr,0) as netwr,a.waerk,nvl(a.mwsbk,0) as mwsbk,a.kunag,a.kunrg,b.aubel,(select c.kunnr from SAPRDP.vbpa c where c.vbeln=b.aubel and rownum = 1 and c.parvw = 'WE') as kunnr,(select d.kunnr from SAPRDP.vbpa d where d.vbeln=b.aubel and rownum=1 and d.parvw = 'RE') as kunnr2,(select e.bstkd from SAPRDP.vbkd e where b.aubel = e.vbeln and rownum=1) as bstkd ")
            .AppendFormat("select distinct a.vkorg,a.vbeln,a.fkdat,nvl(a.netwr,0) as netwr,a.waerk,nvl(a.mwsbk,0) as mwsbk,a.kunag,a.kunrg,b.aubel ")
            .AppendFormat(" FROM SAPRDP.vbrk a inner join SAPRDP.vbrp b on a.vbeln = b.vbeln ")
            .AppendFormat(" WHERE a.mandt='168' and b.mandt='168' and a.fksto = ' ' and a.sfakn = ' ' and a.vbeln <> ' ' and a.vkorg = '{0}' and a.spart = '{1}' ", Sales_Org, division)
            If customer_id <> "EKGBEC01" Then
                .AppendFormat(" and a.kunag = '{0}' ", customer_id)
            Else
                If LCase(Session("user_id")) = "freya.huggard@ecauk.com" Then
                    .AppendFormat(" and (a.kunag in ('EKGBEC01','EKGBEC02','EKGBEC03','EKGBEC04')) ")
                Else
                    .AppendFormat(" and a.kunag = '{0}' ", customer_id)
                End If
            End If
        End With
        'Dim sql As String = String.Format("select distinct a.vkorg,a.vbeln,a.fkdat,nvl(a.netwr,0) as netwr,a.waerk,nvl(a.mwsbk,0) as mwsbk,a.kunag,a.kunrg,b.aubel,(select c.kunnr from SAPRDP.vbpa c where c.vbeln=b.aubel and rownum = 1 and c.parvw = 'WE') as kunnr,(select d.kunnr from SAPRDP.vbpa d where d.vbeln=b.aubel and rownum=1 and d.parvw = 'RE') as kunnr2,(select e.bstkd from SAPRDP.vbkd e where b.aubel = e.vbeln and rownum=1) as bstkd " + _
        '                                  " FROM SAPRDP.vbrk a inner join SAPRDP.vbrp b on a.vbeln = b.vbeln" + _
        '                                  " WHERE a.mandt='168' and b.mandt='168' and a.fksto = ' ' and a.sfakn = ' ' and a.vbeln <> ' ' and a.vkorg = '{0}' and a.spart = '{1}' and a.kunag = '{2}' and a.fkdat between '{3}' and '{4}' order by a.fkdat desc", Sales_Org, division, customer_id, SDate, EDate)
        'If Session("user_id") = "rudy.wang@advantech.com.tw" Then 
        'Response.Write(sb.ToString())
        dt_vbrk = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString)
        If dt_vbrk IsNot Nothing AndAlso dt_vbrk.Rows.Count > 0 Then
            Dim _currency As String = dt_vbrk.Rows(0).Item("waerk").ToString
            Dim sql As String = "SELECT 100 * power(10 , CURRDEC) as num1 FROM SAPRDP.TCURX WHERE CURRKEY = '" & _currency & "'"
            Dim _a As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", sql)
            If _a IsNot Nothing Then Integer.TryParse(_a.ToString, multiplier)
        End If

        Dim t1 As New Threading.Thread(AddressOf GetOpenItem), t2 As New Threading.Thread(AddressOf GetCloseItem)
        t1.Start() : t2.Start()
        t1.Join() : t2.Join()
        table = GetARTable(SDate, EDate, S_Due_Date, E_Due_Date)
    End Sub

    Private Sub GetOpenItem()
        Dim arrInvoiceNo As New ArrayList(), arrInvoiceNo1 As New ArrayList
        Dim i As Integer = 0, _sql As New StringBuilder, _IsExecSQL As Boolean = False
        For Each r As DataRow In dt_vbrk.Rows
            If arrInvoiceNo1.Count Mod 1000 = 0 Then
                If arrInvoiceNo1.Count >= 1000 Then
                    _IsExecSQL = True
                    _sql.AppendLine(String.Join(",", arrInvoiceNo1.ToArray()) & " ) ")
                    _sql.AppendLine(" union all ")
                End If
                If _sql.Length = 0 Or arrInvoiceNo1.Count > 0 Then
                    _sql.AppendLine(" SELECT vbeln,budat,blart,xzahl,shkzg,nvl(wrbtr,0) as wrbtr,zfbdt,zbd1t,waers,xblnr ")
                    _sql.AppendLine(" From SAPRDP.bsid ")
                    _sql.AppendLine(" where mandt='168' and kunnr='" & customer_ID & "' and vbeln in ( ")
                End If
                arrInvoiceNo1.Clear()
            End If
            If Not arrInvoiceNo.Contains("'" + r.Item("vbeln") + "'") Then
                arrInvoiceNo.Add("'" + r.Item("vbeln") + "'")
                arrInvoiceNo1.Add("'" + r.Item("vbeln") + "'")
            End If
            i += 1
        Next


        If arrInvoiceNo1.Count > 0 Then
            'Dim sql As String = "SELECT vbeln,budat,blart,xzahl,shkzg,nvl(wrbtr,0) as wrbtr,zfbdt,zbd1t,waers From SAPRDP.bsid where vbeln in (" + String.Join(",", arrInvoiceNo.ToArray()) + ")"
            'Response.Write(sql)
            'dt_bsid = OraDbUtil.dbGetDataTable("SAP_PRD", sql)
            _sql.AppendLine(String.Join(",", arrInvoiceNo1.ToArray()) & " ) ")
            _IsExecSQL = True
        End If
        If _IsExecSQL Then dt_bsid = OraDbUtil.dbGetDataTable("SAP_PRD", _sql.ToString)
    End Sub

    Private Sub GetCloseItem()
        Dim arrInvoiceNo As New ArrayList(), arrInvoiceNo1 As New ArrayList
        Dim i As Integer = 0, _sql As New StringBuilder, _IsExecSQL As Boolean = False
        For Each r As DataRow In dt_vbrk.Rows
            If arrInvoiceNo1.Count Mod 1000 = 0 Then
                If arrInvoiceNo1.Count >= 1000 Then
                    _IsExecSQL = True
                    _sql.AppendLine(String.Join(",", arrInvoiceNo1.ToArray()) & " ) ")
                    _sql.AppendLine(" union all ")
                End If
                If _sql.Length = 0 Or arrInvoiceNo1.Count > 0 Then
                    _sql.AppendLine(" SELECT vbeln,budat,blart,nvl(wrbtr,0) as wrbtr,shkzg,waers,zfbdt,zbd1t,xblnr ")
                    _sql.AppendLine(" From SAPRDP.bsad ")
                    _sql.AppendLine(" where mandt='168' and kunnr='" & customer_ID & "' and vbeln in ( ")
                End If
                arrInvoiceNo1.Clear()
            End If
            If Not arrInvoiceNo.Contains("'" + r.Item("vbeln") + "'") Then
                arrInvoiceNo.Add("'" + r.Item("vbeln") + "'")
                arrInvoiceNo1.Add("'" + r.Item("vbeln") + "'")
            End If
            i += 1
        Next

        If arrInvoiceNo1.Count > 0 Then
            _sql.AppendLine(String.Join(",", arrInvoiceNo1.ToArray()) & " ) ")
            _IsExecSQL = True
        End If
        If _IsExecSQL Then dt_bsad = OraDbUtil.dbGetDataTable("SAP_PRD", _sql.ToString)
    End Sub

    'Private Sub GetCloseItem()
    '    Dim arrInvoiceNo As New ArrayList
    '    For Each r As DataRow In dt_vbrk.Rows
    '        If Not arrInvoiceNo.Contains("'" + r.Item("vbeln") + "'") Then arrInvoiceNo.Add("'" + r.Item("vbeln") + "'")
    '    Next
    '    If arrInvoiceNo.Count > 0 Then
    '        Dim sql As String = "SELECT vbeln,budat,blart,nvl(wrbtr,0) as wrbtr,shkzg,waers,zfbdt,zbd1t From SAPRDP.bsad where vbeln in (" + String.Join(",", arrInvoiceNo.ToArray()) + ")"
    '        'Response.Write(sql)
    '        'If Session("user_id") = "rudy.wang@advantech.com.tw" Then Response.Write(sql)
    '        dt_bsad = OraDbUtil.dbGetDataTable("SAP_PRD", sql)
    '    End If
    'End Sub

    Protected Function GetARTable(ByVal SDate As String, ByVal EDate As String, ByVal S_Due_Date As String, ByVal E_Due_Date As String) As DataTable
        Dim dt_ar As New DataTable
        dt_ar.Columns.Add("AR_NO", GetType(System.String))
        dt_ar.Columns.Add("RE_NO", GetType(System.String))
        dt_ar.Columns.Add("AR_DATE", GetType(System.String))
        'dt_ar.Columns.Add("SOLDTO", GetType(System.String))
        dt_ar.Columns.Add("AMOUNT", GetType(System.Double))
        dt_ar.Columns.Add("CURRENCY", GetType(System.String))
        dt_ar.Columns.Add("AR_DUE_DATE", GetType(System.String))
        dt_ar.Columns.Add("LOCAL_AMOUNT", GetType(System.Double))
        dt_ar.Columns.Add("AR_STATUS", GetType(System.String))
        'dt_ar.Columns.Add("SONO", GetType(System.String))
        'dt_ar.Columns.Add("SHIPTO", GetType(System.String))
        'dt_ar.Columns.Add("BILLTO", GetType(System.String))
        'dt_ar.Columns.Add("PONO", GetType(System.String))

        If dt_bsad.Rows.Count > 0 Then
            For Each r As DataRow In dt_bsad.Rows
                Dim row As DataRow = dt_ar.NewRow()
                row.Item("AR_NO") = r.Item("vbeln")
                row.Item("RE_NO") = r.Item("xblnr")
                row.Item("AR_DATE") = Date.ParseExact(r.Item("budat"), "yyyyMMdd", New System.Globalization.CultureInfo("fr-FR"), System.Globalization.DateTimeStyles.None).ToString("yyyy/MM/dd")
                row.Item("CURRENCY") = r.Item("waers")
                Dim dr() As DataRow = dt_vbrk.Select("vbeln='" + r.Item("vbeln") + "'")
                row.Item("AMOUNT") = dr(0).Item("netwr")
                For i As Integer = 0 To dr.Length - 1
                    row.Item("AMOUNT") += dr(i).Item("mwsbk")
                Next
                row.Item("AR_DUE_DATE") = DateAdd(DateInterval.Day, CDbl(r.Item("zbd1t")), Date.ParseExact(r.Item("zfbdt"), "yyyyMMdd", New System.Globalization.CultureInfo("fr-FR"), System.Globalization.DateTimeStyles.None)).ToString("yyyy/MM/dd")
                row.Item("LOCAL_AMOUNT") = row.Item("AMOUNT") - r.Item("wrbtr")
                row.Item("AR_STATUS") = "Cleared"
                dt_ar.Rows.Add(row)
            Next
        End If

        If dt_bsid.Rows.Count > 0 Then
            For Each r As DataRow In dt_bsid.Rows
                Dim row As DataRow = dt_ar.NewRow()
                row.Item("AR_NO") = r.Item("vbeln")
                row.Item("RE_NO") = r.Item("xblnr")
                row.Item("AR_DATE") = Date.ParseExact(r.Item("budat"), "yyyyMMdd", New System.Globalization.CultureInfo("fr-FR"), System.Globalization.DateTimeStyles.None).ToString("yyyy/MM/dd")
                row.Item("CURRENCY") = r.Item("waers")
                Dim dr() As DataRow = dt_vbrk.Select("vbeln='" + r.Item("vbeln") + "'")
                row.Item("AMOUNT") = dr(0).Item("netwr")
                For i As Integer = 0 To dr.Length - 1
                    row.Item("AMOUNT") += dr(i).Item("mwsbk")
                Next
                row.Item("AR_DUE_DATE") = DateAdd(DateInterval.Day, CDbl(r.Item("zbd1t")), Date.ParseExact(r.Item("zfbdt"), "yyyyMMdd", New System.Globalization.CultureInfo("fr-FR"), System.Globalization.DateTimeStyles.None)).ToString("yyyy/MM/dd")
                If r.Item("shkzg") = "H" Then
                    Dim oridr() As DataRow = dt_ar.Select("AR_NO='" + r.Item("vbeln") + "'")
                    If oridr.Length > 0 Then
                        row.Item("AR_DATE") = oridr(0).Item("AR_DATE")
                        row.Item("LOCAL_AMOUNT") = row.Item("AMOUNT") + r.Item("wrbtr")
                        dt_ar.Rows(dt_ar.Rows.IndexOf(oridr(0))).Delete()
                    End If
                Else
                    row.Item("LOCAL_AMOUNT") = r.Item("wrbtr")
                End If

                If IsDBNull(row.Item("LOCAL_AMOUNT")) Then
                    row.Item("AR_STATUS") = "Cleared"
                ElseIf row.Item("LOCAL_AMOUNT") = 0 Then
                    row.Item("AR_STATUS") = "Cleared"
                Else
                    If row.Item("AR_DUE_DATE") = "" Or IsDBNull(row.Item("AR_DUE_DATE")) Then
                        row.Item("AR_STATUS") = "Open"
                    Else
                        If row.Item("AMOUNT") - row.Item("LOCAL_AMOUNT") <> 0 Then
                            If CDate(row.Item("AR_DUE_DATE")) < Date.Today Then
                                row.Item("AR_STATUS") = "Partial Overdue"
                            Else
                                row.Item("AR_STATUS") = "Partially Cleared"
                            End If
                        Else
                            If CDate(row.Item("AR_DUE_DATE")) < Date.Today Then
                                row.Item("AR_STATUS") = "Overdue"
                            Else
                                row.Item("AR_STATUS") = "Open"
                            End If
                        End If
                    End If
                End If
                dt_ar.Rows.Add(row)
            Next
        End If

        Dim dr1() As DataRow = dt_ar.Select("AR_DUE_DATE < '" + Date.ParseExact(S_Due_Date, "yyyyMMdd", New System.Globalization.CultureInfo("fr-FR"), System.Globalization.DateTimeStyles.None).ToString("yyyy/MM/dd") + "' or AR_DUE_DATE > '" + Date.ParseExact(E_Due_Date, "yyyyMMdd", New System.Globalization.CultureInfo("fr-FR"), System.Globalization.DateTimeStyles.None).ToString("yyyy/MM/dd") + "'")
        For i As Integer = 0 To dr1.Length - 1
            dt_ar.Rows(dt_ar.Rows.IndexOf(dr1(i))).Delete()
        Next

        dr1 = dt_ar.Select("AR_STATUS ='Cleared' and LOCAL_AMOUNT='0'")
        For i As Integer = 0 To dr1.Length - 1
            dt_ar.Rows(dt_ar.Rows.IndexOf(dr1(i))).Delete()
        Next

        Return dt_ar
    End Function

    Function DateConvert(ByVal strVal) As String
        If IsDate(strVal) Then
            Dim yyyy As String = Year(strVal).ToString()
            Dim mm As String = ""
            Dim dd As String = ""
            Select Case Month(strVal).ToString().Length
                Case 1
                    mm = "0" & Month(strVal).ToString()
                Case 2
                    mm = Month(strVal).ToString()
            End Select
            Select Case Day(strVal).ToString().Length
                Case 1
                    dd = "0" & Day(strVal).ToString()
                Case 2
                    dd = Day(strVal).ToString()
            End Select
            DateConvert = yyyy & mm & dd
        Else
            DateConvert = "00000000"
        End If
    End Function

    Function DateConvertRevsese(ByVal strVal) As String
        Dim yyyy As String = Mid(strVal, 1, 4)
        Dim mm As String = Mid(strVal, 5, 2)
        Dim dd As String = Mid(strVal, 7, 2)
        DateConvertRevsese = yyyy & "/" & mm & "/" & dd
    End Function

    Function FormatDate(ByVal xDate) As String
        Dim xYear As String = "0000"
        Dim xMonth As String = "00"
        Dim xDay As String = "00"

        If IsDate(xDate) = True Then
            xYear = Year(xDate).ToString
            xMonth = Month(xDate).ToString
            xDay = Day(xDate).ToString
        Else
            Dim ArrDate() As String = xDate.Split("/")

            If ArrDate(0).Length = 4 Then
                xYear = ArrDate(0)
                xMonth = ArrDate(1)
                xDay = ArrDate(2)
            Else
                xYear = ArrDate(2)
                xMonth = ArrDate(0)
                xDay = ArrDate(1)
            End If
        End If

        If xMonth.Length = 1 Then
            xMonth = "0" & xMonth
        End If
        If xDay.Length = 1 Then
            xDay = "0" & xDay
        End If
        FormatDate = xYear & "/" & xMonth & "/" & xDay
    End Function

    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFEEAA'")
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor")
            'e.Row.Cells(1).Text = "<a href='/Order/BO_InvoiceInquiry.aspx?inv_no=" & CInt(e.Row.Cells(1).Text) & "' target='_blank'>" & CInt(e.Row.Cells(1).Text) & "</a>"
            Dim strRowCell8 As String = e.Row.Cells(8).Text
            If strRowCell8 = "Cleared" Then
                e.Row.Cells(8).Text = "<font >" & "--" & "</font>"
            ElseIf strRowCell8 = "Overdue" Or strRowCell8 = "Partial Overdue" Then
                Dim diff As TimeSpan = CDate(System.DateTime.Today.ToString("yyyy-MM-dd")) - CDate(e.Row.Cells(7).Text)
                e.Row.Cells(8).Text = "<table width='100%'><tr><td bgcolor='#ffcc66'><font color='red'>" & diff.TotalDays.ToString() & "</font></td></tr></table>"
                'style="BACKGROUND-COLOR: #ffcc66;WIDTH=100%"     style='BACKGROUND-COLOR: #99ff66;WIDTH=100%'
            End If
            If strRowCell8 = "Open" Then
                e.Row.Cells(8).Text = "<table width='100%'><tr><td bgcolor='#99ff66'><font color='red'>" & "Open" & "</font></td></tr></table>"
            End If
            e.Row.Cells(5).Text = (CDbl(e.Row.Cells(5).Text) * multiplier).ToString("#,##0.00")
            Dim open_amount As Double
            If Double.TryParse(e.Row.Cells(6).Text, open_amount) Then e.Row.Cells(6).Text = (open_amount * multiplier).ToString("#,##0.00")
            'e.Row.Cells(5).Text = CDbl(e.Row.Cells(5).Text).ToString("#,##0.00")
        End If
    End Sub
End Class