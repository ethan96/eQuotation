Public Class ChartOver_old
    Inherits System.Web.UI.Page

    Function getCommonWhere() As String
        Return "where (quoteid like 'GQ%' or quoteid like 'AUSQ%' or quoteid like 'AMXQ%') and Year(quoteDate)>2000 and office<>'' and SalesEmail<>'' and org='US01' and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "'"
    End Function
    Sub initYear()
        Me.drpYear.Items.Clear()
        Me.drpYear.Items.Add(New ListItem(Now.Year - 1, Now.Year - 1))
        Me.drpYear.Items.Add(New ListItem(Now.Year, Now.Year))
        If Not IsPostBack Then
            Me.drpYear.SelectedValue = Now.Year
        End If
    End Sub
    Sub initOption()
        initRBU(getRBUList(Me.drpYear.SelectedValue, Me.drpMF.SelectedValue, Me.drpMT.SelectedValue, Me.drpOrg.SelectedValue))
    End Sub

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
    Function getRBUList(ByVal Y As Integer, ByVal F As Integer, ByVal T As Integer, ByVal Org As String) As DataTable
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", String.Format("select distinct office as RBU from quotationMaster " & getCommonWhere() & " and YEAR(quotedate) = '" & Y & "' and MONTH(quotedate) >= '" & F & "' and MONTH(quotedate) <= '" & T & "' and Org like '%" & Org & "%' order by RBU Asc"))
        Return dt
    End Function

    Protected Sub drpOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        initRBU(getRBUList(Me.drpYear.SelectedValue, Me.drpMF.SelectedValue, Me.drpMT.SelectedValue, Me.drpOrg.SelectedValue))
    End Sub
    Sub getDataFormat(ByVal DT As DataTable)
        Dim M As New Table
        Dim MR1 As New TableRow
        Dim MR2 As New TableRow
        Dim MC1 As New TableCell
        MC1.RowSpan = 2
        MC1.Text = "Sales"
        MC1.Font.Bold = True
        MR1.Cells.Add(MC1)


        For I As Integer = Me.drpMF.SelectedValue To Me.drpMT.SelectedValue
            Dim name As String = ""
            Select Case I : Case 1 : name = "Jan" : Case 2 : name = "Feb" : Case 3 : name = "Mar" : Case 4 : name = "Apr" : Case 5 : name = "May"
                Case 6 : name = "June" : Case 7 : name = "July" : Case 8 : name = "Aug" : Case 9 : name = "Sept" : Case 10 : name = "Oct" : Case 11 : name = "Nov" : Case 12 : name = "Dec"
            End Select
            Dim C As New TableCell
            C.ColumnSpan = 5
            C.Text = name
            C.Font.Bold = True
            MR1.Cells.Add(C)
            Dim C1 As New TableCell
            C1.Text = "Quote"
            C1.Font.Bold = True
            Dim C2 As New TableCell
            C2.Text = "Converted"
            C2.Font.Bold = True
            Dim C3 As New TableCell
            C3.Text = "Converted Rate"
            C3.Font.Bold = True
            Dim C4 As New TableCell
            C4.Text = "Number"
            C4.Font.Bold = True
            Dim C5 As New TableCell
            C5.Text = "Converted Number"
            C5.Font.Bold = True
            MR2.Cells.Add(C1) : MR2.Cells.Add(C2) : MR2.Cells.Add(C3) : MR2.Cells.Add(C4) : MR2.Cells.Add(C5)
        Next
        Dim ct As New TableCell
        ct.Text = "Total"
        ct.ColumnSpan = 5
        ct.Font.Bold = True
        MR1.Cells.Add(ct)
        Dim ct1 As New TableCell : Dim ct2 As New TableCell : Dim ct3 As New TableCell : Dim ct4 As New TableCell : Dim ct5 As New TableCell
        ct1.Text = "Quote" : ct1.Font.Bold = True
        ct2.Text = "Converted" : ct2.Font.Bold = True
        ct3.Text = "Converted Rate" : ct3.Font.Bold = True
        ct4.Text = "Number" : ct4.Font.Bold = True
        ct5.Text = "Converted Number" : ct5.Font.Bold = True
        MR2.Cells.Add(ct1) : MR2.Cells.Add(ct2) : MR2.Cells.Add(ct3) : MR2.Cells.Add(ct4) : MR2.Cells.Add(ct5)
        M.Rows.Add(MR1) : M.Rows.Add(MR2)

        Dim salest As String = ""
        Dim dttemp As New DataTable
        dttemp.Columns.Add("Sales")
        For Each R As DataRow In DT.Rows
            If salest <> R.Item("Sales") Then
                salest = R.Item("Sales")
                Dim rtemp As DataRow = dttemp.NewRow
                rtemp.Item("Sales") = salest
                dttemp.Rows.Add(rtemp)
            End If
        Next

        For Each r As DataRow In dttemp.Rows
            Dim TR As New TableRow
            Dim TD As New TableCell
            Dim L As New HyperLink
            L.Text = r.Item("Sales")
            L.NavigateUrl = "~/quote/chart.aspx?U=" & r.Item("Sales") & "&Y=" & Me.drpYear.SelectedValue & "&MF=" & drpMF.SelectedValue & "&MT=" & drpMT.SelectedValue & "&ORG=" & Me.drpOrg.SelectedValue & "&RBU=" & drpRBU.SelectedValue
            TD.Controls.Add(L)
            TR.Cells.Add(TD)
            For I As Integer = Me.drpMF.SelectedValue To Me.drpMT.SelectedValue
                Dim cur As New Label : cur.Text = "$" : Dim cur1 As New Label : cur1.Text = "$"
                Dim orowA() As DataRow = DT.Select("Sales='" & r.Item("Sales") & "' and Month=" & I)
                Dim ll As New HyperLink
                If orowA.Count > 0 Then
                    ll.Text = FormatNumber(orowA(0).Item("Amount"), 0)
                    ll.NavigateUrl = "~/quote/chart.aspx?U=" & r.Item("Sales") & "&Y=" & Me.drpYear.SelectedValue & "&MF=" & I & "&MT=" & I & "&ORG=" & Me.drpOrg.SelectedValue & "&RBU=" & drpRBU.SelectedValue
                Else
                    ll.Text = 0
                End If
                Dim llC As New HyperLink
                If orowA.Count > 0 Then
                    llC.Text = FormatNumber(orowA(0).Item("Converted Amount"), 0)
                    llC.NavigateUrl = "~/quote/chart.aspx?U=" & r.Item("Sales") & "&Y=" & Me.drpYear.SelectedValue & "&MF=" & I & "&MT=" & I & "&ORG=" & Me.drpOrg.SelectedValue & "&RBU=" & drpRBU.SelectedValue
                Else
                    llC.Text = 0
                End If
                Dim c1 As New TableCell
                Dim c2 As New TableCell
                c1.Controls.Add(cur) : c2.Controls.Add(cur1)
                c1.Controls.Add(ll) : c2.Controls.Add(llC)
                Dim c3 As New TableCell
                If ll.Text <> 0 Then
                    c3.Text = FormatNumber(100 * (llC.Text / ll.Text), 2) & "%"
                Else
                    c3.Text = 0
                End If
                Dim c4 As New TableCell
                If orowA.Count > 0 Then
                    c4.Text = orowA(0).Item("Quote Number")
                End If
                Dim c5 As New TableCell
                If orowA.Count > 0 Then
                    c5.Text = orowA(0).Item("Converted Quote Number")
                End If
                TR.Cells.Add(c1) : TR.Cells.Add(c2) : TR.Cells.Add(c3) : TR.Cells.Add(c4) : TR.Cells.Add(c5)
            Next

            Dim curT As New Label : curT.Text = "$" : Dim curT1 As New Label : curT1.Text = "$"
            Dim orowT() As DataRow = DT.Select("Sales='" & r.Item("Sales") & "'")
            Dim llT As New HyperLink
            If orowT.Count > 0 Then
                Dim TT As Decimal = 0
                For Each RT As DataRow In orowT
                    TT += RT.Item("Amount")
                Next
                llT.Text = FormatNumber(TT, 0)
                llT.NavigateUrl = "~/quote/chart.aspx?U=" & r.Item("Sales") & "&Y=" & Me.drpYear.SelectedValue & "&MF=" & Me.drpMF.SelectedValue & "&MT=" & Me.drpMT.SelectedValue & "&ORG=" & Me.drpOrg.SelectedValue & "&RBU=" & drpRBU.SelectedValue
            Else
                llT.Text = 0
            End If
            Dim llTC As New HyperLink
            If orowT.Count > 0 Then
                Dim TT As Decimal = 0
                For Each RT As DataRow In orowT
                    TT += RT.Item("Converted Amount")
                Next
                llTC.Text = FormatNumber(TT, 0)
                llTC.NavigateUrl = "~/quote/chart.aspx?U=" & r.Item("Sales") & "&Y=" & Me.drpYear.SelectedValue & "&MF=" & Me.drpMF.SelectedValue & "&MT=" & Me.drpMT.SelectedValue & "&ORG=" & Me.drpOrg.SelectedValue & "&RBU=" & drpRBU.SelectedValue
            Else
                llTC.Text = 0
            End If
            Dim cTT1 As New TableCell
            Dim cTT2 As New TableCell
            cTT1.Controls.Add(curT) : cTT2.Controls.Add(curT1)
            cTT1.Controls.Add(llT) : cTT2.Controls.Add(llTC)
            Dim cTT3 As New TableCell
            If llT.Text <> 0 Then
                cTT3.Text = FormatNumber(100 * (llTC.Text / llT.Text), 2) & "%"
            Else
                cTT3.Text = 0
            End If
            Dim cTT4 As New TableCell
            If orowT.Count > 0 Then
                Dim TT As Decimal = 0
                For Each RT As DataRow In orowT
                    TT += RT.Item("Quote Number")
                Next
                cTT4.Text = TT
            End If
            Dim cTT5 As New TableCell
            If orowT.Count > 0 Then
                Dim TT As Decimal = 0
                For Each RT As DataRow In orowT
                    TT += RT.Item("Converted Quote Number")
                Next
                cTT5.Text = TT
            End If
            TR.Cells.Add(cTT1) : TR.Cells.Add(cTT2) : TR.Cells.Add(cTT3) : TR.Cells.Add(cTT4) : TR.Cells.Add(cTT5)

            M.Rows.Add(TR)
        Next
        Me.plC.Controls.Add(M)
    End Sub
    Function getDataFilter() As String
        Dim str As String = ""
        'If Me.drpMF.SelectedValue And Me.drpMT.SelectedValue Then
        str = str & "year(quoteDate)='" & drpYear.SelectedValue & "' and Month(quoteDate)>='" & Me.drpMF.SelectedValue & "' and Month(quoteDate)<='" & Me.drpMT.SelectedValue & "'"
        'End If
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
        Return str
    End Function
    Function getColumnData() As DataTable
        Dim str As String = String.Format("SELECT Q.Sales as Sales,Q.M as Month,isnull(sum(O.CAMT),0) AS [Converted Amount],isnull(SUM(Q.Amount),0) AS Amount,count(Q.[Quote Id]) as [Quote Number],COUNT(O.[quoteid]) as [Converted Quote Number] FROM " & _
                                        " (select quoteId as [Quote Id],salesEmail as Sales,MONTH(quoteDate) as M,(select isnull(SUM(newunitprice * qty),0) from QuotationDetail " & _
                                        " where QuotationDetail.quoteId=QuotationMaster.quoteId) as Amount from QuotationMaster " & _
                                        " {0}) q" & _
                                        "        Left Join" & _
                                        "  (SELECT A.QUOTEID, SUM(CAMOUNT) AS CAMT FROM (SELECT QUOTEID,(select ISNULL(SUM(UNIT_PRICE * QTY),0) from " & _
                                         " MyAdvantechGlobal.dbo.ORDER_DETAIL WHERE ORDER_ID=QUOTE_TO_ORDER_LOG.SO_NO)AS CAMOUNT FROM QUOTE_TO_ORDER_LOG) " & _
                                          " A GROUP BY A.QUOTEID) o on q.[Quote Id]=o.QUOTEID GROUP BY Q.M,Q.SALES ORDER BY SALES", getCommonWhere() & " and " & getDataFilter())

        'Response.Write(str) : Response.End()
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", str)
        Return dt
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Now.Month > 1 Then
                Me.drpMF.SelectedValue = DateAdd(DateInterval.Month, -1, Now).Month
            Else
                Me.drpMF.SelectedValue = Now.Month
            End If
            Me.drpMT.SelectedValue = Now.Month
            initOption()
            Dim dt As New DataTable
            dt = getColumnData()
            getDataFormat(dt)
        End If

    End Sub
    Protected Sub imgXls_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Util.DataTable2ExcelDownload(getColumnData(), "Quote Report")
    End Sub

    Protected Sub btnQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If CInt(Me.drpMT.SelectedValue) < CInt(Me.drpMF.SelectedValue) Then
            Util.showMessage("To Month is earlier than From Month")
            Exit Sub
        End If
        Dim dt As New DataTable
        dt = getColumnData()
        getDataFormat(DT)
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        initYear()
    End Sub

End Class