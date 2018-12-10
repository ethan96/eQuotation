Public Class ChartProduct
    Inherits System.Web.UI.Page

    Function getCommonWhere() As String
        'Return "where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%'   or quoteNo like 'AACQ%') and active=1 and Year(quoteDate)>2000 and office<>'' and SalesEmail<>'' and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "'"
        Return "where (quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%' or quoteNo like 'AACQ%') and active=1 and Year(quoteDate)>2000 and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "'"
    End Function
    Sub initOption()
        initYear(getYearList())
        initOrg(getOrgList(Me.drpYear.SelectedValue))
        initRBU(getRBUList(Me.drpYear.SelectedValue, Me.drpOrg.SelectedValue))
        initSales(getSalesList(Me.drpYear.SelectedValue, Me.drpOrg.SelectedValue, Me.drpRBU.SelectedValue))
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
    Function getPlant() As String
        If Me.drpOrg.SelectedValue <> "" Then
            Return Left(Me.drpOrg.SelectedValue, 2) & "H1"
        End If
        Return "USH1"
    End Function
    Function getColumnData() As DataTable

        '===================Ryan 20170815 Comment out old SQL===================
        'Dim str As String = String.Format(" Select Q.*,isnull(O.[Converted Qty],0) as [Converted Qty], CONVERT(nvarchar,convert(numeric(17,2),Isnull((Convert(decimal,O.[Converted Qty]) / Convert(decimal,Q.Qty ) * 100),0))) + '%' as ConversionRate, " &
        ' " isnull(O.[Converted Amount],0) as [Converted Amount],isnull((SELECT TOP 1 (case ABC_INDICATOR when '' THEN 'z(null)' ELSE ABC_INDICATOR end) as AA from MyAdvantechGlobal.dbo.SAP_PRODUCT_ABC WHERE PART_NO=Q.[Part No] And PLANT='" & getPlant() & "'),'z(null)') AS [Indicator] " & 
        ' " from " &
        ' "(select distinct PartNo as [Part No], isnull(SUM(QTY), 0) As [Qty], isnull(SUM(newUnitPrice * qty), 0) as [Amount] from QuotationDetail where quoteid in( " &
        ' " select quoteid from quotationMaster {0} And partNo<>'' and partNo not like '%BTO' and partNo not like 'C-CTOS%') and partNo like '%" + TBpartno.Text.Trim.Replace("'", "''") + "%' group by partNo) Q " &
        ' " left join " &
        ' " (select distinct Part_No as [Part No], isnull(SUM(QTY),0) AS [Converted Qty],isnull(SUM(unit_price * qty),0) as [Converted Amount] from MyAdvantechGlobal.dbo.ORDER_DETAIL where ORDER_ID in ( " &
        ' " select so_no from QUOTE_TO_ORDER_LOG where QUOTEID in ( " &
        ' " select quoteid from quotationMaster {0} " &
        ' " )) group by PART_NO) O on Q.[Part No]=o.[Part No]  ", getCommonWhere() & " and " & getDataFilter())
        '===================End 20170815 Comment Out===================

        'Ryan 20170815 New SQL
        Dim str As String = String.Format(" Select Q.*,isnull(O.[Converted Qty],0) as [Converted Qty], CONVERT(nvarchar,convert(numeric(17,2),Isnull((Convert(decimal,O.[Converted Qty]) / Convert(decimal,Q.Qty ) * 100),0))) + '%' as ConversionRate, " &
         " isnull(O.[Converted Amount],0) as [Converted Amount], " &
         " isnull((case ISNULL(O2.HasBTOSOrder,'N') " &
         " when 'N' then (SELECT TOP 1 ISNULL(ABC_INDICATOR,'') from MyAdvantechGlobal.dbo.SAP_PRODUCT_ABC WHERE PART_NO=Q.[Part No] AND PLANT='" & getPlant() & "') " &
         " when 'Y' THEN (SELECT TOP 1 (CASE ISNULL(ABC_INDICATOR,'') WHEN '' THEN '' ELSE 'z(' +  ABC_INDICATOR + ')'END) from MyAdvantechGlobal.dbo.SAP_PRODUCT_ABC WHERE PART_NO=Q.[Part No] AND PLANT='" & getPlant() & "') " &
         " else (SELECT TOP 1 ISNULL(ABC_INDICATOR,'') from MyAdvantechGlobal.dbo.SAP_PRODUCT_ABC WHERE PART_NO=Q.[Part No] AND PLANT='" & getPlant() & "') end ),'') as [Indicator] " &
         " from " &
         " (select distinct PartNo as [Part No], isnull(SUM(QTY), 0) As [Qty], isnull(SUM(newUnitPrice * qty), 0) as [Amount] from QuotationDetail where quoteid in( " &
         " select quoteid from quotationMaster {0} And partNo<>'' and partNo not like '%BTO' and partNo not like 'C-CTOS%') and partNo like '%" + TBpartno.Text.Trim.Replace("'", "''") + "%' group by partNo) Q " &
         " left join " &
         " (select distinct Part_No as [Part No], isnull(SUM(QTY),0) AS [Converted Qty],isnull(SUM(unit_price * qty),0) as [Converted Amount] from MyAdvantechGlobal.dbo.ORDER_DETAIL where ORDER_ID in " &
         " (select so_no from QUOTE_TO_ORDER_LOG where QUOTEID in ( " &
         " select quoteid from quotationMaster {0} " &
         " )) group by PART_NO) O " &
         " on Q.[Part No]=o.[Part No] " &
         " left join " &
         " (SELECT distinct PART_NO as [Part No],'Y' as [HasBTOSOrder] FROM MyAdvantechGlobal.dbo.ORDER_DETAIL where ORDER_ID in " &
         " (select so_no from QUOTE_TO_ORDER_LOG where QUOTEID in ( " &
         " select quoteid from quotationMaster {0} " &
         " ) and ORDER_LINE_TYPE in ('-1','1'))) O2 " &
         " ON Q.[Part No] = O2.[Part No] ", getCommonWhere() & " And " & getDataFilter())


        'Response.Write(getDataFilter()) ': Response.End()
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
            Dim g As GridView = CType(sender, GridView) : Dim nAmt As Integer = 0 : Dim nCAmt As Integer = 0 : Dim n As Integer = 0
            For Each c As TableCell In g.HeaderRow.Cells
                n += 1
                Dim txtHeader As LinkButton = c.Controls(0)
                If txtHeader.Text = "Amount" Then
                    nAmt = n - 1
                End If
                If txtHeader.Text = "Converted Amount" Then
                    nCAmt = n - 1
                End If
            Next
            Dim rd As DataRowView = CType(e.Row.DataItem, DataRowView)
            e.Row.Cells(nAmt).Text = "$" & FormatNumber(rd.Item("Amount"), 0)
            e.Row.Cells(nCAmt).Text = "$" & FormatNumber(rd.Item("Converted Amount"), 0)
            e.Row.Cells(0).Text = String.Format("<a onclick='openWin(""productreport.aspx?partno={0}&y={1}&mf={2}&mt={3}&org={4}&rbu={5}"")'  href='javascript:void(0);' >{0}</a>", e.Row.Cells(0).Text, drpYear.SelectedValue, drpMF.SelectedValue, drpMT.SelectedValue, drpOrg.SelectedValue, drpRBU.SelectedValue)
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dt As New DataTable
        dt = getColumnData()
        Dim DV As New DataView(dt)
        If Not IsNothing(ViewState("E")) AndAlso Not IsNothing(ViewState("D")) Then
            DV.Sort = ViewState("E") & " " & ViewState("D")
        End If
        Me.GridView1.DataSource = DV
        Me.GridView1.DataBind()
    End Sub
End Class