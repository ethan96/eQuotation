Imports System.Web.UI.DataVisualization.Charting
Public Class chart
    Inherits System.Web.UI.Page

    Function getCommonWhere() As String
        'Return "where (quoteid like 'GQ%' or quoteid like 'AUSQ%' or quoteid like 'AMXQ%') and Year(quoteDate)>2000 and office<>'' and SalesEmail<>'' and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "'"
        'Return "where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%') and active=1 and Year(quoteDate)>2000 and office<>'' and SalesEmail<>'' and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "'"
        Return "where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%')  and Year(quoteDate)>2000 and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "'"
    End Function
    Sub initOption()
        initYear(getYearList())
        initOrg(getOrgList(Me.drpYear.SelectedValue))
        initRBU(getRBUList(Me.drpYear.SelectedValue, Me.drpOrg.SelectedValue))
        initSales(getSalesList(Me.drpYear.SelectedValue, Me.drpOrg.SelectedValue, Me.drpRBU.SelectedValue))
    End Sub
    Sub initChart(ByVal dt As DataTable)
        For i As Integer = 1 To 12
            'Dim temp() As DataRow = dt.Select("[quote date] >'" & CDate(Me.drpYear.SelectedValue & "-" & i.ToString("00") & "-01") & "' and [quote date]<'" & DateAdd(DateInterval.Day, 1, CDate(Me.drpYear.SelectedValue & "-" & (i).ToString("00") & "-" & Date.DaysInMonth(Me.drpYear.SelectedValue, i) & "")) & "'")
            Dim V As Decimal = 0
            Dim CV As Decimal = 0
            Dim o As New Object
            Dim co As New Object
            o = dt.Compute("sum(Amount)", "[quote date] >'" & CDate(Me.drpYear.SelectedValue & "-" & i.ToString("00") & "-01") & "' and [quote date]<'" & DateAdd(DateInterval.Day, 1, CDate(Me.drpYear.SelectedValue & "-" & (i).ToString("00") & "-" & Date.DaysInMonth(Me.drpYear.SelectedValue, i) & "")) & "'")
            co = dt.Compute("sum([Converted Amount])", "[quote date] >'" & CDate(Me.drpYear.SelectedValue & "-" & i.ToString("00") & "-01") & "' and [quote date]<'" & DateAdd(DateInterval.Day, 1, CDate(Me.drpYear.SelectedValue & "-" & (i).ToString("00") & "-" & Date.DaysInMonth(Me.drpYear.SelectedValue, i) & "")) & "'")
            If Not IsDBNull(o) Then
                V = CDec(o)
            End If
            If Not IsDBNull(co) Then
                CV = CDec(co)
            End If
            'For Each r As DataRow In temp
            '    V += r.Item("Amount")
            'Next
            Dim name As String = ""
            Select Case i : Case 1 : name = "Jan" : Case 2 : name = "Feb" : Case 3 : name = "Mar" : Case 4 : name = "Apr" : Case 5 : name = "May"
                Case 6 : name = "June" : Case 7 : name = "July" : Case 8 : name = "Aug" : Case 9 : name = "Sept" : Case 10 : name = "Oct" : Case 11 : name = "Nov" : Case 12 : name = "Dec"
            End Select
            Dim r As Decimal = 0

            If V <> 0 Then
                Dim FV As Decimal = FormatNumber(V / 1000, 2) : Dim FCV As Decimal = FormatNumber(CV / 1000, 2)
                r = FormatNumber(CV / V * 100, 4) ': r = FormatNumber(r * 100, 2)

                Chart1.Series("Converted(K $)").ChartType = SeriesChartType.StackedColumn
                Chart1.Series("Not Converted(K $)").ChartType = SeriesChartType.StackedColumn
                Chart1.Series("Monthly Rate(%)").ChartType = SeriesChartType.StackedColumn
                Chart1.Series("Converted(K $)").Points.AddXY(name, FCV)
                Chart1.Series("Not Converted(K $)").Points.AddXY(name, FV - FCV)
                Chart1.Series("Monthly Rate(%)").Points.AddXY(name, r)
                Chart1.Series("Converted(K $)").IsValueShownAsLabel = True
                Chart1.Series("Not Converted(K $)").IsValueShownAsLabel = True
                Chart1.Series("Monthly Rate(%)").IsValueShownAsLabel = True
                Chart1.Series("Converted(K $)").LabelFormat = "{0:N}"
                Chart1.Series("Not Converted(K $)").LabelFormat = "{0:N}"
                Chart1.Series("Monthly Rate(%)").LabelFormat = "{0.00}%"
            End If

            'S.Points.AddY(100)
            'Chart1.Series("Series 1").IsValueShownAsLabel = True
        Next



    End Sub
    Sub initChartPie(ByVal dt As DataTable)
        Me.Chart2.Series.Clear()
        Dim S As New Series
        Dim totalAmount As Decimal = 0
        Dim convertAmount As Decimal = 0
        Dim o As New Object
        o = dt.Compute("sum(Amount)", "")
        If Not IsDBNull(o) Then
            totalAmount = CDec(o)
        End If
        o = dt.Compute("sum([Converted Amount])", "")
        If Not IsDBNull(o) Then
            convertAmount = CDec(o)
        End If

        If totalAmount <> 0 Then
            S.Points.AddXY("Converted(%)", FormatNumber((convertAmount / totalAmount), 4) * 100)
            S.Points.AddXY("Not Converted(%)", (1 - FormatNumber((convertAmount / totalAmount), 4)) * 100)
        End If
        S.IsValueShownAsLabel = True
        S.ChartType = SeriesChartType.Pie
        Me.Chart2.Series.Add(S)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            initOption()
            drpYear.SelectedValue = Now.Year
            drpYear_SelectedIndexChanged(drpYear, Nothing)
            If Now.Month > 1 Then
                Me.drpMF.SelectedValue = DateAdd(DateInterval.Month, -1, Now).Month
            Else
                Me.drpMF.SelectedValue = Now.Month
            End If
            Me.drpMT.SelectedValue = Now.Month
            If Not IsNothing(Request("Y")) Then
                Me.drpYear.SelectedValue = Request("Y")
            End If
            If Not IsNothing(Request("MT")) Then
                Me.drpMT.SelectedValue = Request("MT")
            End If
            If Not IsNothing(Request("MF")) Then
                Me.drpMF.SelectedValue = Request("MF")
            End If
            If Not IsNothing(Request("ORG")) Then
                Me.drpOrg.SelectedValue = Request("ORG")
            End If
            If Not IsNothing(Request("RBU")) Then
                Me.drpRBU.SelectedValue = Request("RBU")
            End If
            If Not IsNothing(Request("U")) Then
                Dim Ar As New ArrayList
                Ar.Add(Request("U").ToString)
                Me.ascxPickSales.SalesArr = Ar
                ' Me.drpSales.SelectedValue = Request("U")
            End If
        End If

    End Sub
    Sub initYear(ByVal dt As DataTable)
        Me.drpYear.Items.Clear()
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If dt.Rows.Count > 1 Then
                Me.drpYear.Items.Add(New ListItem("ALL", ""))
            End If
            For Each r As DataRow In dt.Rows
                Me.drpYear.Items.Add(New ListItem(r.Item("year"), r.Item("year")))
            Next
        End If
    End Sub
    Function getYearList() As DataTable
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", String.Format("select distinct year(quoteDate) as year from quotationMaster " & getCommonWhere() & " order by year desc"))
        Return dt
    End Function

    Sub initOrg(ByVal dt As DataTable)
        Me.drpOrg.Items.Clear()
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If dt.Rows.Count > 1 Then
                Me.drpOrg.Items.Add(New ListItem("ALL", ""))
            End If
            For Each r As DataRow In dt.Rows
                Me.drpOrg.Items.Add(New ListItem(r.Item("Org"), r.Item("Org")))
            Next
        End If
    End Sub
    Function getOrgList(ByVal Year As String) As DataTable
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", String.Format("select distinct org as org from quotationMaster " & getCommonWhere() & " and quotedate like '%" & Year & "%'  order by org Asc"))
        Return dt
    End Function

    Sub initRBU(ByVal dt As DataTable)
        Me.drpRBU.Items.Clear()
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            If dt.Rows.Count > 1 Then
                Me.drpRBU.Items.Add(New ListItem("ALL", ""))
            End If
            For Each r As DataRow In dt.Rows
                If r.Item("RBU") <> "" Then
                    Me.drpRBU.Items.Add(New ListItem(r.Item("RBU"), r.Item("RBU")))
                End If
            Next
        End If
    End Sub
    Function getRBUList(ByVal Year As String, ByVal Org As String) As DataTable
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", String.Format("select distinct office as RBU from quotationMaster " & getCommonWhere() & " and quotedate like '%" & Year & "%' and Org like '%" & Org & "%' order by RBU Asc"))
        Return dt
    End Function

    Sub initSales(ByVal dt As DataTable)
        Dim ar As New ArrayList
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            For Each r As DataRow In dt.Rows
                ar.Add(r.Item("Sales"))
            Next
            If ar.Count > 0 Then
                Me.ascxPickSales.SalesArrInit = ar
            End If
        End If
    End Sub
    Function getSalesList(ByVal Year As String, ByVal Org As String, ByVal RBU As String) As DataTable
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", String.Format("select distinct SalesEmail as Sales from quotationMaster " & getCommonWhere() & " and quotedate like '%" & Year & "%' and Org like '%" & Org & "%' and Office like '%" & RBU & "%' order by Sales Asc"))
        Return dt
    End Function

    Protected Sub drpYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        initOrg(getOrgList(Me.drpYear.SelectedValue))
        initRBU(getRBUList(Me.drpYear.SelectedValue, Me.drpOrg.SelectedValue))
        initSales(getSalesList(Me.drpYear.SelectedValue, Me.drpOrg.SelectedValue, Me.drpRBU.SelectedValue))
    End Sub

    Protected Sub drpOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        initRBU(getRBUList(Me.drpYear.SelectedValue, Me.drpOrg.SelectedValue))
        initSales(getSalesList(Me.drpYear.SelectedValue, Me.drpOrg.SelectedValue, Me.drpRBU.SelectedValue))
    End Sub

    Protected Sub drpRBU_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        initSales(getSalesList(Me.drpYear.SelectedValue, Me.drpOrg.SelectedValue, Me.drpRBU.SelectedValue))
    End Sub

    Function getDataFilter() As String
        Dim str As String = ""
        If Me.drpYear.SelectedValue <> "" Then
            str = str & "year(quoteDate)='" & Me.drpYear.SelectedValue & "'"
        End If

        If str <> "" Then
            str = str & " and "
        End If
        str = str & "year(quoteDate)='" & drpYear.SelectedValue & "' and Month(quoteDate)>='" & Me.drpMF.SelectedValue & "' and Month(quoteDate)<='" & Me.drpMT.SelectedValue & "'"

        If Me.drpOrg.SelectedValue <> "" Then
            If str <> "" Then
                str = str & " and "
            End If
            str = str & "Org='" & Me.drpOrg.SelectedValue & "'"
        End If
        If Me.drpRBU.SelectedValue <> "" Then
            If str <> "" Then
                str = str & " and "
            End If
            str = str & "office='" & Me.drpRBU.SelectedValue & "'"
        End If
        Dim ar As ArrayList = Me.ascxPickSales.SalesArr
        If Not IsNothing(ar) AndAlso ar.Count > 0 Then
            If str <> "" Then
                str = str & " and "

                str = str & "salesEmail in ('" & String.Join("','", ar.ToArray) & "')"

            End If
        End If
        Return str
    End Function
    Function getColumnData() As DataTable
        Dim str As String = String.Format(" SELECT Q.[Quote No],Q.[ERP Id],Q.[Quote To],Q.[RBU],Q.[Sales],Q.[Quote Date] " & _
                                          " ,Q.[Exp Date],Q.[Created By],Q.[Org],Q.[OPTY Id],Q.[Currency],Q.[Amount] " & _
                                          " ,isnull(O.CAMT,0) AS [Converted Amount],(CASE WHEN O.CAMT>0 THEN 'Y' ELSE '' END) AS Converted " & _
                                          " FROM (select quoteId as [Quote Id],quoteNo as [Quote No],quoteToErpId as [ERP Id],quoteToName as [Quote To],office as [RBU],salesEmail as [Sales],quoteDate as [Quote Date]," & _
                                          " expiredDate as [Exp Date],createdBy as [Created By],org as [Org],(select top 1 optyid from optyQuote where optyQuote.quoteId=QuotationMaster.quoteId) as [OPTY Id]," & _
                                          " currency as [Currency]," & _
                                          " (select isnull(SUM(newunitprice * qty),0) as Amount from QuotationDetail where QuotationDetail.quoteId=QuotationMaster.quoteId) as Amount from QuotationMaster " & _
                                          " {0}) q" & _
                                          "        Left Join" & _
                                          " (SELECT A.QUOTEID, isnull(SUM(CAMOUNT),0) AS CAMT FROM " & _
                                          " (SELECT QUOTEID,(select ISNULL(SUM(UNIT_PRICE * QTY),0) from MyAdvantechGlobal.dbo.ORDER_DETAIL " & _
                                          " WHERE ORDER_ID=QUOTE_TO_ORDER_LOG.SO_NO)AS CAMOUNT FROM QUOTE_TO_ORDER_LOG)  A " & _
                                          " GROUP BY A.QUOTEID) o on q.[Quote Id]=o.QUOTEID ORDER BY Q.[QUOTE DATE] DESC, Q.[Quote Id] DESC", getCommonWhere() & " and " & getDataFilter())

        'Response.Write(str) ' : Response.End()
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", str)
        Return dt
    End Function
    Protected Sub imgXls_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Util.DataTable2ExcelDownload(getColumnData(), "Quote Report")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim dt As New DataTable
        dt = getColumnData()
        initChart(dt)
        initChartPie(dt)
        Dim DV As New DataView(dt)

        If Not IsNothing(ViewState("E")) AndAlso Not IsNothing(ViewState("D")) Then
            DV.Sort = ViewState("E") & " " & ViewState("D")
        End If

        Me.GridView1.DataSource = DV
        Me.GridView1.DataBind()
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
    End Sub

    Protected Sub drpMF_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If CInt(Me.drpMT.SelectedValue) < CInt(Me.drpMF.SelectedValue) Then
            Util.showMessage("To Month is earlier than From Month")
            Exit Sub
        End If
    End Sub

    Protected Sub drpMT_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If CInt(Me.drpMT.SelectedValue) < CInt(Me.drpMF.SelectedValue) Then
            Util.showMessage("To Month is earlier than From Month")
            Exit Sub
        End If
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        ViewState("E") = e.SortExpression
        If IsNothing(ViewState("D")) OrElse ViewState("D") = "DESC" Then
            ViewState("D") = "ASC"
        Else
            ViewState("D") = "DESC"
        End If
    End Sub

    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            For Each c As TableCell In e.Row.Cells
                Dim txtHeader As LinkButton = c.Controls(0)
                If txtHeader.Text = ViewState("E") Then
                    Dim L As New Label
                    L.Text = IIf(ViewState("D") = "ASC", "▲", "▼")
                    c.Controls.Add(L)
                End If
            Next
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim g As GridView = CType(sender, GridView) : Dim nAmt As Integer = 0 : Dim nCAmt As Integer = 0 : Dim nQD As Integer = 0 : Dim nEP As Integer = 0 : Dim n As Integer = 0
            For Each c As TableCell In g.HeaderRow.Cells
                n += 1
                Dim txtHeader As LinkButton = c.Controls(0)
                If txtHeader.Text = "Amount" Then
                    nAmt = n - 1
                End If
                If txtHeader.Text = "Converted Amount" Then
                    nCAmt = n - 1
                End If
                If txtHeader.Text = "Quote Date" Then
                    nQD = n - 1
                End If
                If txtHeader.Text = "Exp Date" Then
                    nEP = n - 1
                End If
            Next
            Dim rd As DataRowView = CType(e.Row.DataItem, DataRowView)
            e.Row.Cells(nAmt).Text = "$" & FormatNumber(rd.Item("Amount"), 0)
            e.Row.Cells(nCAmt).Text = "$" & FormatNumber(rd.Item("Converted Amount"), 0)
            e.Row.Cells(nQD).Text = CDate(e.Row.Cells(nQD).Text).ToString("dd/MM/yyyy mm:ss")
            e.Row.Cells(nEP).Text = CDate(e.Row.Cells(nEP).Text).ToString("dd/MM/yyyy mm:ss")
        End If
    End Sub

End Class