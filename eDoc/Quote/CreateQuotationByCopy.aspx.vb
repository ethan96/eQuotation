Public Class CreateQuotationByCopy
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LabDetail.Visible = False
        If Not IsPostBack Then

            If IsNothing(Pivot.CurrentProfile) Then
                Session.Abandon() : Util.ClearCookie_Login("ADEQCOOK")
                Response.Redirect(String.Format("~/login.aspx?RURL={0}", HttpContext.Current.Server.UrlEncode(Request.RawUrl)))
            End If

            'Me.txtOrderFrom.Text = Now.AddDays(-30).ToString("yyyy/MM/dd")
            'Me.txtOrderTo.Text = Now.ToString("yyyy/MM/dd")

            If Role.IsUsaUser() Then
                lbSearchOrg.Text = "US01"
            ElseIf (Role.IsTWAonlineSales OrElse Role.IsHQDCSales) Then
                lbSearchOrg.Text = "TW01"
            ElseIf Role.IsCNAonlineSales Then
                lbSearchOrg.Text = "CN10"
            ElseIf Role.IsKRAonlineSales Then
                lbSearchOrg.Text = "KR01"
            ElseIf Role.IsJPAonlineSales Then
                lbSearchOrg.Text = "JP01"
            ElseIf Role.IsABRSales Then
                lbSearchOrg.Text = "BR01"
            Else
                lbSearchOrg.Text = "EU10"
            End If
            'dlOrg.Attributes.Add("disabled", "disable")
            ' showData()
        End If
        Porder.Visible = False
        PQuote.Visible = False
        PeStore.Visible = False
        If Qtype.SelectedValue = 0 Then Porder.Visible = True
        If Qtype.SelectedValue = 1 Then Porder.Visible = True
        If Qtype.SelectedValue = 2 Then PQuote.Visible = True
        If Qtype.SelectedValue = 3 Then PeStore.Visible = True
    End Sub
    Sub showData(Optional ByVal TopNum As Integer = 100)
        'If String.IsNullOrEmpty(txtOrderNo.Text.Trim()) AndAlso TopNum <> 0 Then
        '    Util.showMessage(" Please input Order No. ")
        '    Exit Sub
        'End If
        'If Date.TryParse(Me.txtOrderFrom.Text, Now) = False Then txtOrderFrom.Text = Now.AddDays(-30).ToString("yyyy/MM/dd")
        'If Date.TryParse(Me.txtOrderTo.Text, Now) = False Then txtOrderTo.Text = Now.ToString("yyyy/MM/dd")
        'If Not IsPostBack Then TopNum = 10
        Dim dt As New DataTable
        If TopNum = 0 Then
            dt = Nothing
            'Frank 2013/10/15
            Me.gv1.DataSource = Nothing
            Me.gv1.DataBind()
        Else
            dt = SAPTools.GetOrderListFromSAP(Me.txtPONO.Text, Me.txtOrderNo.Text, _
                                          Me.txtCompanyId.Text, Me.txtCompanyName.Text, _
                                          lbSearchOrg.Text, Me.txtOrderFrom.Text, _
                                          Me.txtOrderTo.Text, TopNum, Qtype.SelectedValue)
        End If
        Me.GridView1.DataSource = dt
        Me.GridView1.DataBind()
    End Sub
    Sub showDataForeStore(Optional ByVal TopNum As Integer = 100)
        Dim dt As New DataTable
        'If TopNum = 0 Then
        '    dt = Nothing
        '    Me.GridView2.DataSource = Nothing
        '    Me.GridView2.DataBind()
        'Else
        ' dt = SAPTools.GetOrderListFromSAP(Me.txtPONO.Text, Me.txtOrderNo.Text, _
        '               Me.txtCompanyId.Text, lbSearchOrg.Text, _
        'Me.txtOrderFrom.Text, Me.txtOrderTo.Text, TopNum, Qtype.SelectedValue)
        Dim OrgID As String = Pivot.CurrentProfile.getCurrOrg
        Dim sb As New StringBuilder
        sb.AppendLine(" SELECT TOP 100 quoteid,quoteToErpId,quoteToName,ORG,createdDate,createdBy from V_eStoreAllQuotation where 1=1 ")
        If Not String.IsNullOrEmpty(OrgID) Then
            sb.AppendFormat(" and ORG ='{0}' ", OrgID)
        End If
        If Not String.IsNullOrEmpty(TBQuoteid.Text.Trim) Then
            sb.AppendFormat(" and quoteid like '%{0}%' ", TBQuoteid.Text.Trim.Replace("''", "'").Replace("*", ""))
        End If
        If Not String.IsNullOrEmpty(TBCustomerName.Text.Trim) Then
            sb.AppendFormat(" and quoteToName like N'%{0}%' ", TBCustomerName.Text.Trim.Replace("''", "'").Replace("*", ""))
        End If
        If Not String.IsNullOrEmpty(TBCreator.Text.Trim) Then
            sb.AppendFormat(" and createdBy like '%{0}%' ", TBCreator.Text.Trim.Replace("''", "'").Replace("*", ""))
        End If
        If Date.TryParse(TBfrom.Text.Trim, Now) Then
            sb.AppendLine(String.Format(" and createdDate >=  '{0}'   ", Date.Parse(TBfrom.Text.Trim).ToString("yyyyMMdd")))
        End If
        If Date.TryParse(TBto.Text.Trim, Now) Then
            sb.AppendLine(String.Format(" and createdDate <=  '{0}'   ", Date.Parse(TBto.Text.Trim).AddDays(1).ToString("yyyyMMdd")))
        End If
        sb.AppendLine(" order by createdDate desc")
        dt = tbOPBase.dbGetDataTableSchema("Estore", sb.ToString)
        'Response.Write(sb.ToString)
        'End If
        GridView3.Visible = False : LabeStoreDetail.Visible = False
        Me.GridView2.DataSource = dt
        Me.GridView2.DataBind()
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            Select Case Qtype.SelectedValue
                Case 0 'quote
                    CType(e.Row.FindControl("l1"), Label).Text = "Quote No"
                    CType(e.Row.FindControl("l9"), Label).Text = "Quote Date"
                Case Else 'order
                    CType(e.Row.FindControl("l1"), Label).Text = "Order No"
                    CType(e.Row.FindControl("l9"), Label).Text = "Order Date"
            End Select
        End If
    End Sub
    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        LabDetail.Visible = False
        gv1.Visible = False
        showData()
    End Sub
    Protected Sub BTeStore_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' LabDetail.Visible = False
        'gv1.Visible = False
        showDataForeStore()
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        showData()
    End Sub
    Protected Sub GridView2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView2.PageIndex = e.NewPageIndex
        showDataForeStore()
    End Sub

    Protected Sub btnDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'PanelOrderDetail.Visible = True
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim sono As String = Me.GridView1.DataKeys(row.RowIndex).Value
        ShowDataDetail(sono)
    End Sub

    Sub ShowDataDetail(ByVal OrderNO As String)
        Dim dt As DataTable = SAPTools.GetOrderDetailFromSAPBySoNo(OrderNO)
        Me.gv1.DataSource = dt
        Me.gv1.DataBind()
        LabDetail.Visible = True
        gv1.Visible = True
        TBhide.Focus()
    End Sub
    Sub ShoweStoreDetail(ByVal quoteid As String)
        Dim dt As DataTable = tbOPBase.dbGetDataTableSchema("Estore", String.Format("SELECT line_No,CASE WHEN BTONO IS  NULL THEN PARTNO ELSE BTONO END AS PARTNO,qty,ReqDate,unitPrice,UnitPrice*qty as Amount  FROM V_eStoreQuotationDetail where quoteId = '{0}'", quoteid))
        Me.GridView3.DataSource = dt
        Me.GridView3.DataBind()
        LabeStoreDetail.Visible = True
        GridView3.Visible = True
        TBhide2.Focus()
    End Sub
    Function ParseSONO(ByVal SONO As String) As String
        If IsNumericItem(SONO) Then
            Return Integer.Parse(SONO).ToString()
        End If
        Return SONO
    End Function
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
    Function ParseSAPDate(ByVal strSAPDate As String) As String
        Dim strOutputDate As Date = Date.MinValue
        If Date.TryParseExact(strSAPDate, "yyyyMMdd", New Globalization.CultureInfo("en-US"), Globalization.DateTimeStyles.None, strOutputDate) Then
            Return strOutputDate.ToString("yyyy/MM/dd")
        End If
        Return strSAPDate
    End Function
    Protected Sub btnQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim OrderNo As String = Me.GridView1.DataKeys(row.RowIndex).Value
        Response.Redirect("~/quote/Order2Quote.aspx?SoNo=" & OrderNo)
    End Sub
    Protected Sub btnQuoteStore_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim quoteid As String = Me.GridView2.DataKeys(row.RowIndex).Value
        Response.Redirect("~/quote/eStoreQuote2Quote.aspx?QUOTEID=" & quoteid)
    End Sub
    Protected Sub GridView1_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs)
        Dim row As GridViewRow = GridView1.Rows(e.NewSelectedIndex)
        Dim sono As String = Me.GridView1.DataKeys(row.RowIndex).Value
        ShowDataDetail(sono)
    End Sub
    Protected Sub GridView2_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs)
        Dim row As GridViewRow = GridView2.Rows(e.NewSelectedIndex)
        Dim sono As String = Me.GridView2.DataKeys(row.RowIndex).Value
        ShoweStoreDetail(sono)
    End Sub

    Protected Sub Qtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case Qtype.SelectedValue
            Case 0
                showData(0)
                Me.Label3.Text = "Quote No"
                Me.LabDetail.Text = "Quote Detail:"
            Case 2
                Me.ascxPickQuoteCopy.ShowData("")
            Case Else
                showData(0)
                Me.Label3.Text = "Order No"
                Me.LabDetail.Text = "Order Detail:"
                Me.GridView1.Columns(0).HeaderText = "Order No"
        End Select

        'If Qtype.SelectedValue = 2 Then
        '    Me.ascxPickQuoteCopy.ShowData("")
        'Else
        '    showData(0)
        'End If
    End Sub
End Class