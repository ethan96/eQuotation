Public Class ChartOver
    Inherits System.Web.UI.Page

    Function getCommonWhere() As String
        'Return "where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%') and active=1 and Year(quoteDate)>2000 and office<>'' and SalesEmail<>'' and org='US01' and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "'"
        'Ming 2013/09/03
        'JJ 2014/2/25：經Jay指示加入由SIEBEL_POSITION join SIEBEL_CONTACT查找Sales所屬的ORG，由客戶所屬ORG改為Sales所屬ORG
        'Return String.Format(" left join MyAdvantechGlobal.dbo.SIEBEL_POSITION sp on QuotationMaster.createdBy = sp.EMAIL_ADDR" & _
        '                     " inner join MyAdvantechGlobal.dbo.SIEBEL_CONTACT sc on sp.CONTACT_ID = sc.ROW_ID" & _
        '                     " where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%') and Year(quoteDate)>2000" & _
        '                     " and siebelRBU<>'' and SalesEmail<>'' and org='US01' and (DOCSTATUS='{0}' or qStatus='FINISH') ", CInt(COMM.Fixer.eDocStatus.QFINISH))

        'Return String.Format(" left join MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_ALIAS a on QuotationMaster.createdBy=a.Email " & _
        '             " left join MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID " & _
        '             " inner join MyAdvantechGlobal.dbo.SIEBEL_POSITION c on b.Email=c.EMAIL_ADDR " & _
        '             " inner join MyAdvantechGlobal.dbo.SIEBEL_CONTACT sc on c.CONTACT_ID=sc.ROW_ID " & _
        '             " where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%') and Year(quoteDate)>2000" & _
        '             " and siebelRBU<>'' and SalesEmail<>'' and (DOCSTATUS='{0}' or qStatus='FINISH') ", CInt(COMM.Fixer.eDocStatus.QFINISH))

        'Frank 20150311
        'Remove condition "and SalesEmail<>''"
        'Return String.Format(" left join MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_ALIAS a on QuotationMaster.createdBy=a.Email " & _
        '     " left join MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID " & _
        '     " inner join MyAdvantechGlobal.dbo.SIEBEL_POSITION c on b.Email=c.EMAIL_ADDR " & _
        '     " inner join MyAdvantechGlobal.dbo.SIEBEL_CONTACT sc on c.CONTACT_ID=sc.ROW_ID " & _
        '     " where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%') and Year(quoteDate)>2000" & _
        '     " and siebelRBU<>'' and (DOCSTATUS='{0}' or qStatus='FINISH') ", CInt(COMM.Fixer.eDocStatus.QFINISH))

        'Frank 20150311
        'Remove condition "and siebelRBU<>''"
        'Return String.Format(" left join MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_ALIAS a on QuotationMaster.createdBy=a.Email " & _
        '    " left join MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID " & _
        '    " inner join MyAdvantechGlobal.dbo.SIEBEL_POSITION c on b.Email=c.EMAIL_ADDR " & _
        '    " inner join MyAdvantechGlobal.dbo.SIEBEL_CONTACT sc on c.CONTACT_ID=sc.ROW_ID " & _
        '    " left join QUOTE_TO_ORDER_LOG qo on QuotationMaster.quoteId=qo.QUOTEID " & _
        '    " left join MyAdvantechGlobal.dbo.ORDER_MASTER om on qo.SO_NO=om.ORDER_ID " & _
        '    " where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%') and Year(quoteDate)>2000" & _
        '    " and (DOCSTATUS='{0}' or qStatus='FINISH') ", CInt(COMM.Fixer.eDocStatus.QFINISH))

        'Frank 20170327 New Tables for storing outlook mail group information
        'Return String.Format(" left join MyAdvantechGlobal.dbo.AD_MEMBER_ALIAS a on QuotationMaster.createdBy=a.ALIAS_EMAIL " & _
        '" left join MyAdvantechGlobal.dbo.AD_MEMBER_ALIAS b on a.EMAIL=b.EMAIL " & _
        '" inner join MyAdvantechGlobal.dbo.SIEBEL_POSITION c on b.ALIAS_EMAIL=c.EMAIL_ADDR " & _
        '" inner join MyAdvantechGlobal.dbo.SIEBEL_CONTACT sc on c.CONTACT_ID=sc.ROW_ID " & _
        '" left join QUOTE_TO_ORDER_LOG qo on QuotationMaster.quoteId=qo.QUOTEID " & _
        '" left join MyAdvantechGlobal.dbo.ORDER_MASTER om on qo.SO_NO=om.ORDER_ID " & _
        '" where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%') and Year(quoteDate)>2000" & _
        '" and (DOCSTATUS='{0}' or qStatus='FINISH') ", CInt(COMM.Fixer.eDocStatus.QFINISH))


        Return String.Format(" left join MyAdvantechGlobal.dbo.AD_MEMBER_ALIAS a on QuotationMaster.createdBy=a.ALIAS_EMAIL " &
        " left join MyAdvantechGlobal.dbo.AD_MEMBER_ALIAS b on a.EMAIL=b.EMAIL " &
        " inner join MyAdvantechGlobal.dbo.SIEBEL_POSITION c on b.ALIAS_EMAIL=c.EMAIL_ADDR " &
        " inner join MyAdvantechGlobal.dbo.SIEBEL_CONTACT sc on c.CONTACT_ID=sc.ROW_ID " &
        " left join QUOTE_TO_ORDER_LOG qo on QuotationMaster.quoteId=qo.QUOTEID " &
        " left join MyAdvantechGlobal.dbo.ORDER_MASTER om on qo.SO_NO=om.ORDER_ID " &
        " where (quoteNo like 'GQ%' or quoteNo like 'AUSQ%' or quoteNo like 'AMXQ%') and Year(quoteDate)>2000" &
        " and (DOCSTATUS='{0}') ", CInt(COMM.Fixer.eDocStatus.QFINISH))


        'and org='US01' 


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
                If r.Item("RBU") <> "" AndAlso r.Item("RBU") <> "ANA" Then
                    Me.drpRBU.Items.Add(New ListItem(r.Item("RBU"), r.Item("RBU")))
                End If
            Next
        End If
    End Sub
    Function getRBUList(ByVal Y As Integer, ByVal F As Integer, ByVal T As Integer, ByVal Org As String) As DataTable
        Dim dt As New DataTable
        'dt = tbOPBase.dbGetDataTable("EQ", String.Format("select distinct office as RBU from quotationMaster " & getCommonWhere() & " and YEAR(quotedate) = '" & Y & "' and MONTH(quotedate) >= '" & F & "' and MONTH(quotedate) <= '" & T & "' and Org like '%" & Org & "%' order by RBU Asc"))
        'Ming 2013/09/03
        'dt = tbOPBase.dbGetDataTable("EQ", String.Format("select distinct siebelRBU as RBU from quotationMaster " & getCommonWhere() & " and YEAR(quotedate) = '" & Y & "' and MONTH(quotedate) >= '" & F & "' and MONTH(quotedate) <= '" & T & "' and Org like '%" & Org & "%' order by RBU Asc"))
        'JJ 2014/2/25：經Jay指示加入由SIEBEL_POSITION join SIEBEL_CONTACT查找Sales所屬的ORG，由客戶所屬RBU改為Sales所屬ORG
        dt = tbOPBase.dbGetDataTable("EQ", String.Format("select distinct sc.OrgID as RBU from quotationMaster " & getCommonWhere() & " and YEAR(quotedate) = '" & Y & "' and MONTH(quotedate) >= '" & F & "' and MONTH(quotedate) <= '" & T & "' and Org like '%" & Org & "%' order by RBU Asc"))
        'dt = tbOPBase.dbGetDataTable("EQ", String.Format("select distinct siebelRBU as RBU from quotationMaster where Org like '%" & Org & "%' order by RBU Asc"))
        Return dt
    End Function

    Protected Sub drpOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        initRBU(getRBUList(Me.drpYear.SelectedValue, Me.drpMF.SelectedValue, Me.drpMT.SelectedValue, Me.drpOrg.SelectedValue))
        Dim dt As DataTable = getSalesList(Me.drpYear.SelectedValue, drpOrg.SelectedValue, drpRBU.SelectedValue)
        initSales(dt)
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
            'JJ 2014/2/21：ANA-DMF和ANADMF是一樣的，只是一個是SAP、一個是Siebel的命名，這兩個都必須包含ANA
            '雖然現在不太有ANA都是ANADMF了，不過ANA還是必須包含顯示出來，所以下拉選單選ANADMF就必須把ANADMF和ANA都顯示出來
            If Me.drpRBU.SelectedValue = "ANA-DMF" Or Me.drpRBU.SelectedValue = "ANADMF" Then
                If str <> "" Then
                    str = str & " and "
                End If
                'str = str & "(siebelRBU='" & Me.drpRBU.SelectedValue & "' or siebelRBU='ANA')"
                'JJ 2014/2/25：經Jay指示由客戶所屬ORG改為Sales所屬ORG
                str = str & "(sc.OrgID='" & Me.drpRBU.SelectedValue & "' or sc.OrgID='ANA')"
            Else
                If str <> "" Then
                    str = str & " and "
                End If
                'Ming 2013/09/03
                'str = str & "office='" & Me.drpRBU.SelectedValue & "'"
                'str = str & "siebelRBU='" & Me.drpRBU.SelectedValue & "'"
                'JJ 2014/2/25：經Jay指示由客戶所屬ORG改為Sales所屬ORG
                str = str & "sc.OrgID='" & Me.drpRBU.SelectedValue & "'"
            End If
        End If
        Dim ar As ArrayList = Me.ascxPickSales.SalesArr
        If Not IsNothing(ar) AndAlso ar.Count > 0 Then
            If str <> "" Then
                str = str & " and "

                'str = str & "QuotationMaster.createdBy in ('" & String.Join("','", ar.ToArray) & "')"
                str = str & "QuotationMaster.salesEmail in ('" & String.Join("','", ar.ToArray) & "')"

            End If
        End If
        Dim salesstr As String = "Bafford.Howard@Advantech.com,Bill.Kunze@Advantech.com,Denise.Kwong@Advantech.com,Fei.Khong@Advantech.com,Jane.Hsieh@Advantech.com,nada.liu@Advantech.com.cn,Richard.ko@advantech.com"
        Dim p() As String = Split(salesstr, ",")
        For Each i As String In p
            If str <> "" Then
                str = str & " and "
            End If
            str = str & "QuotationMaster.createdBy<>'" & i & "'"
        Next
        Return str
    End Function
    Function getColumnData() As DataTable
        'Dim str As String = String.Format("SELECT Q.Sales as Sales,Q.M as Month,isnull(sum(O.CAMT),0) AS [Converted Amount],isnull(SUM(Q.Amount),0) AS Amount,count(Q.[Quote Id]) as [Quote Number],COUNT(O.[quoteid]) as [Converted Quote Number],0 as estoreAmount FROM " & _
        '                                " (select quoteId as [Quote Id],QuotationMaster.createdBy as Sales,MONTH(quoteDate) as M,(select isnull(SUM(newunitprice * qty),0) from QuotationDetail " & _
        '                                " where QuotationDetail.quoteId=QuotationMaster.quoteId) as Amount from QuotationMaster " & _
        '                                " {0}) q" & _
        '                                "        Left Join" & _
        '                                "  (SELECT A.QUOTEID, SUM(CAMOUNT) AS CAMT FROM (SELECT QUOTEID,(select ISNULL(SUM(UNIT_PRICE * QTY),0) from " & _
        '                                 " MyAdvantechGlobal.dbo.ORDER_DETAIL WHERE ORDER_ID=QUOTE_TO_ORDER_LOG.SO_NO)AS CAMOUNT FROM QUOTE_TO_ORDER_LOG) " & _
        '                                  " A GROUP BY A.QUOTEID) o on q.[Quote Id]=o.QUOTEID GROUP BY Q.M,Q.SALES ORDER BY SALES", getCommonWhere() & " and " & getDataFilter())

        Dim _str As New StringBuilder
        '_str.AppendLine(" SELECT Q.Sales as Sales,Q.M as Month,isnull(sum(O.CAMT),0) AS [Converted Amount], ")
        '_str.AppendLine(" isnull(SUM(Q.Amount),0) AS Amount,count(Q.[Quote Id]) as [Quote Number], ")
        '_str.AppendLine(" COUNT(O.[quoteid]) as [Converted Quote Number],0 as estoreAmount FROM ")
        '_str.AppendLine(" ( ")
        '_str.AppendLine(" select * from( ")
        '_str.AppendLine(" select quoteId as [Quote Id],QuotationMaster.createdBy as Sales,MONTH(quoteDate) as M, ")
        '_str.AppendLine(" (select isnull(SUM(newunitprice * qty),0) from QuotationDetail ")
        '_str.AppendLine(" where QuotationDetail.quoteId=QuotationMaster.quoteId) as Amount from QuotationMaster ")
        '_str.AppendLine(" " & getCommonWhere() & " and " & getDataFilter() & ") q1 group by q1.Sales,q1.[Quote Id],q1.M,q1.Amount ")
        '_str.AppendLine(" ) Q ")
        '_str.AppendLine(" Left Join ")
        '_str.AppendLine(" (SELECT A.QUOTEID, SUM(CAMOUNT) AS CAMT FROM ( ")
        '_str.AppendLine(" SELECT QUOTEID,(select ISNULL(SUM(UNIT_PRICE * QTY),0) from ")
        '_str.AppendLine(" MyAdvantechGlobal.dbo.ORDER_DETAIL WHERE ORDER_ID=QUOTE_TO_ORDER_LOG.SO_NO)AS CAMOUNT FROM QUOTE_TO_ORDER_LOG) ")
        '_str.AppendLine(" A GROUP BY A.QUOTEID) o on q.[Quote Id]=o.QUOTEID GROUP BY Q.M,Q.SALES ORDER BY SALES ")

        _str.AppendLine(" SELECT Q.Sales as Sales,Q.M as Month,Q.DISTRICT,isnull(sum(Q.OrderAmount),0) AS [Converted Amount] ")
        _str.AppendLine(" ,isnull(SUM(Q.Amount),0) AS Amount,count(Q.[Quote Id]) as [Quote Number],  ")
        _str.AppendLine(" SUM(Q.IsConverted) as [Converted Quote Number],0 as estoreAmount ")
        _str.AppendLine(" FROM ( ")
        _str.AppendLine("       SELECT p.[Quote Id],p.Sales,p.M,p.Amount,p.IsConverted,p.DISTRICT,isnull(SUM(od.UNIT_PRICE*od.QTY),0) as OrderAmount ")
        _str.AppendLine("       FROM (  ")
        _str.AppendLine("             SELECT * ")
        _str.AppendLine("             FROM ( ")
        _str.AppendLine("                   Select QuotationMaster.quoteId as [Quote Id],QuotationMaster.salesEmail as Sales,MONTH(quoteDate) as M ")
        _str.AppendLine("                   ,(select isnull(SUM(newunitprice * qty),0) from QuotationDetail where QuotationDetail.quoteId=QuotationMaster.quoteId) as Amount  ")
        _str.AppendLine("                   ,(case isnull(qo.SO_NO,'') when '' then 0 else 1 end) as IsConverted, qo.SO_NO as OrderId  ")
        _str.AppendLine("                   ,(case isnull(om.DISTRICT,'') when '' then isnull(QuotationMaster.DISTRICT,'') else om.DISTRICT end) as DISTRICT ")
        _str.AppendLine("                   From QuotationMaster  ")
        _str.AppendLine("                   " & getCommonWhere() & " and " & getDataFilter())
        _str.AppendLine("                   ) S group by S.[Quote Id],S.Sales,S.M,S.Amount,S.IsConverted,S.OrderId,S.DISTRICT ")
        _str.AppendLine("             ) p left join MyAdvantechGlobal.dbo.ORDER_DETAIL od on p.OrderId=od.ORDER_ID ")
        _str.AppendLine("             Group by p.[Quote Id],p.Sales,p.M,p.Amount,p.IsConverted,p.DISTRICT        ")
        _str.AppendLine("        ) Q ")
        _str.AppendLine("        Group by Q.Sales,Q.M,Q.DISTRICT ")
        _str.AppendLine("        Order by Q.Sales,Q.DISTRICT,Q.M ")

        'Response.Write(str) : Response.End()
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", _str.ToString)
        Return dt
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'If Now.Month > 1 Then
            '    Me.drpMF.SelectedValue = DateAdd(DateInterval.Month, -1, Now).Month
            'Else
            '    Me.drpMF.SelectedValue = Now.Month
            'End If
            If Role.IsEUSales() Then
                drpOrg.SelectedValue = "EU10"
            End If
            If Role.IsUsaUser() Then
                drpOrg.SelectedValue = "US01"
            End If
            Me.drpMF.SelectedValue = 1
            Me.drpMT.SelectedValue = 12
            'Me.drpMT.SelectedValue = Now.Month
            initOption()
            'Dim dt As New DataTable
            'dt = getColumnData()
            'getDataFormat(dt)
            Dim dt As DataTable = getSalesList(Me.drpYear.SelectedValue, drpOrg.SelectedValue, drpRBU.SelectedValue)
            initSales(dt)

        End If

    End Sub
    'JJ：寫入Salse到Pick Sales
    Sub initSales(ByVal dt As DataTable)
        Dim ar As New ArrayList
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            For Each r As DataRow In dt.Rows
                ar.Add(r.Item("Sales"))
            Next
            If ar.Count > 0 Then
                Me.ascxPickSales.SalesArrInit = ar
            End If
        Else
            Me.ascxPickSales.ClearList()
        End If
    End Sub
    Function getSalesList(ByVal Year As String, ByVal Org As String, ByVal RBU As String) As DataTable
        Dim dt As New DataTable
        Dim strRBU As String = ""
        'JJ 2014/2/21：現在下拉RBU只有ANADMF或ANA-DMF，但此兩者都必須包含ANA的Sales
        If RBU = "ANA-DMF" Or RBU = "ANADMF" Then
            strRBU = "(sc.OrgID='" & RBU & "' or sc.OrgID='ANA')"
        Else
           
            strRBU = " sc.OrgID like '%" & RBU & "%'"
        End If

        Dim str As String = String.Format("select distinct QuotationMaster.createdBy as Sales from quotationMaster " & getCommonWhere() & " and   QuotationMaster.createdBy<>'Bafford.Howard@Advantech.com' and  QuotationMaster.createdBy<>'Bill.Kunze@Advantech.com' and  QuotationMaster.createdBy<>'Denise.Kwong@Advantech.com' and  QuotationMaster.createdBy<>'Fei.Khong@Advantech.com' and  QuotationMaster.createdBy<>'Jane.Hsieh@Advantech.com' and  QuotationMaster.createdBy<>'nada.liu@Advantech.com.cn' and  QuotationMaster.createdBy<>'Richard.ko@advantech.com' and quotedate like '%" & Year & "%' and Org like '%" & Org & "%' and " & strRBU & " order by Sales Asc")
        'Ming 2013/09/03
        dt = tbOPBase.dbGetDataTable("EQ", str)
        Return dt
    End Function
    Protected Sub imgXls_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Util.DataTable2ExcelDownload(getColumnData(), "Quote Report")
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        initYear()
    End Sub
    Protected Sub btnQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuery.Click
        If CInt(Me.drpMT.SelectedValue) < CInt(Me.drpMF.SelectedValue) Then
            Util.showMessage("To Month is earlier than From Month")
            Exit Sub
        End If
        Dim dt As New DataTable
        dt = getColumnData()
        dt.Columns.Add(New DataColumn("Converted_quote_rate", GetType(String)))
        dt.Columns.Add(New DataColumn("Convert_revenue_rate", GetType(String)))
        'dt.Columns.Add(New DataColumn("estoreAmount ", GetType(Decimal)))
        'dt.AcceptChanges()
        'Ming add 20131225   estore quote report
        Dim sb As New StringBuilder
        sb.AppendFormat(" select M.Sales, M.MH as Month ,isnull(SUM(M.Amount),0) AS Amount, M.DISTRICT from ")
        sb.AppendFormat("  ( ")
        sb.AppendFormat("  select  createdBy as Sales,MONTH(quoteDate) as MH, OrderTotalAmount  as [Converted Amount] , QuotationTotalAmount as Amount, DISTRICT   ")
        sb.AppendFormat("  from   V_eStoreInternalQuotation   ")
        sb.AppendFormat("  WHERE  1=1  ")
        Dim ar As ArrayList = Me.ascxPickSales.SalesArr
        If Not IsNothing(ar) AndAlso ar.Count > 0 Then
            sb.AppendFormat(" and createdBy in ('{0}') ", String.Join("','", ar.ToArray))
        End If
        sb.AppendFormat("  and qstatus ='Confirmed'   ")
        sb.AppendFormat("  and org='{0}'   ", Me.drpOrg.SelectedValue)
        sb.AppendFormat(" and year(quoteDate)='{0}' and Month(quoteDate)>='1' and Month(quoteDate)<='12' ", drpYear.SelectedValue)
        sb.AppendFormat(" )  M ")
        sb.AppendFormat(" GROUP BY M.Sales,M.MH,M.DISTRICT ORDER BY M.Sales ")
        Dim eSoreQuotedt As DataTable = tbOPBase.dbGetDataTableSchema("Estore", sb.ToString)
        ViewState("eSoreQuotedt") = eSoreQuotedt
        'end
        For Each dr As DataRow In dt.Rows
            dr.BeginEdit()
            dr.Item("Sales") = dr.Item("Sales").ToString.Trim.ToLower
            If Decimal.Parse(dr.Item("Quote Number").ToString) > 0 Then
                dr.Item("Converted_quote_rate") = (CInt(dr.Item("Converted Quote Number")) / CInt(dr.Item("Quote Number")))
            Else
                dr.Item("Converted_quote_rate") = "0"
            End If
            If Decimal.Parse(dr.Item("Converted Amount").ToString) > 0 Then
                dr.Item("Convert_revenue_rate") = (CInt(dr.Item("Converted Amount")) / CInt(dr.Item("Amount")))
            Else
                dr.Item("Convert_revenue_rate") = "0"
            End If
            'If eSoreQuotedt.Rows.Count > 0 Then
            '    For Each sdr As DataRow In eSoreQuotedt.Rows
            '        If sdr.Item("Sales") IsNot Nothing AndAlso String.Equals(sdr.Item("Sales"), dr.Item("Sales"), StringComparison.OrdinalIgnoreCase) Then
            '            If String.Equals(sdr.Item("Month"), dr.Item("Month"), StringComparison.OrdinalIgnoreCase) AndAlso Decimal.TryParse(sdr.Item("Amount"), 0) Then
            '                dr.Item("estoreAmount") = Decimal.Parse(sdr.Item("Amount"))
            '                Exit For
            '                'Else
            '                '    dr.Item("estoreAmount") = 0
            '            End If
            '        End If
            '    Next
            'End If
            dr.EndEdit()
        Next
        dt.AcceptChanges()
        ViewState("dt") = dt
        ' Util.showDT(dt)
        Repeater1.DataSource = dt.DefaultView.ToTable(True, New String() {"Sales", "DISTRICT"})
        Repeater1.DataBind()
        'getDataFormat(dt)
    End Sub
    Public Function GetZ(ByVal user As [Object], ByVal Mfrom As Integer, ByVal Mto As Integer, ByVal type As Integer, ByVal DISTRICT As String) As Decimal
        Dim result As DataTable = CType(ViewState("dt"), DataTable)
        Dim dr As DataRow() = result.Select(String.Format(" month >={0} and month <={1} and sales ='{2}' and DISTRICT='{3}'", Mfrom, Mto, user, DISTRICT), "month desc")
        Dim _Total As Decimal = 0
        If dr.Length > 0 Then
            Dim cd As Double = 0, qd As Decimal = 0
            For Each row As DataRow In dr
                If type = 0 Then
                    cd += Double.Parse(row.Item("Converted Quote Number"))
                    qd += Double.Parse(row.Item("Quote Number"))
                End If
                If type = 1 Then
                    cd += Double.Parse(row.Item("Converted Amount"))
                    qd += Double.Parse(row.Item("Amount"))
                End If
            Next
            If qd > 0 Then
                _Total = cd / qd
            End If
            Return _Total
        End If
        Return 0
    End Function
    Public Function GetQ(ByVal user As [Object], ByVal month As Integer, ByVal type As Integer, ByVal DISTRICT As String) As Decimal
        Dim eSoreQuotedt As DataTable = CType(ViewState("eSoreQuotedt"), DataTable)
        If type = 7 Then
            Dim drs() As DataRow = eSoreQuotedt.Select("Sales='" & user & "' and Month=" & month & " and DISTRICT='" & DISTRICT & "'")
            If drs.Length = 1 Then
                If drs(0).Item("Amount") IsNot Nothing AndAlso Decimal.TryParse(drs(0).Item("Amount"), 0) Then
                    Return [Decimal].Parse(drs(0).Item("Amount"))
                Else
                    Return 0
                End If
            End If
        End If
        Dim result As DataTable = CType(ViewState("dt"), DataTable)
        For Each i As DataRow In result.Rows
            If String.Equals(i.Item("Sales"), user.ToString.Trim, StringComparison.CurrentCultureIgnoreCase) _
                AndAlso Integer.Parse(i.Item("Month")) = month _
                AndAlso String.Equals(i.Item("DISTRICT"), DISTRICT, StringComparison.CurrentCultureIgnoreCase) Then
                Select Case type
                    Case 1
                        Return [Decimal].Parse(i.Item("Quote Number"))
                        Exit Select
                    Case 2
                        Return [Decimal].Parse(i.Item("Converted Quote Number"))
                        Exit Select
                    Case 3
                        Return [Decimal].Parse(i.Item("Amount"))
                        Exit Select
                    Case 4
                        Return [Decimal].Parse(i.Item("Converted Amount"))
                        Exit Select
                    Case 5
                        Return [Decimal].Parse(i.Item("Converted_quote_rate"))
                        Exit Select
                    Case 6
                        Return [Decimal].Parse(i.Item("Convert_revenue_rate"))
                        Exit Select
                        'Case 7
                        'Exit Select
                End Select
                Exit For
            End If
        Next
        Return 0
    End Function

    Protected Sub drpYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpYear.SelectedIndexChanged
        initOption()

        Dim dt As DataTable = getSalesList(Me.drpYear.SelectedValue, drpOrg.SelectedValue, drpRBU.SelectedValue)
        initSales(dt)

    End Sub

    Protected Sub drpRBU_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpRBU.SelectedIndexChanged

        Dim dt As DataTable = getSalesList(Me.drpYear.SelectedValue, drpOrg.SelectedValue, drpRBU.SelectedValue)
        initSales(dt)

    End Sub
End Class