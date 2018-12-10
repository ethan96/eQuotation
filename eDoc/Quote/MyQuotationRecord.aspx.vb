Public Class MyQuotationRecord
    Inherits System.Web.UI.Page

    Dim myQD As New QuotationDetail("EQ", "quotationDetail")
    Dim _IsANAUser As Boolean = True
    'Dim _IsFranchiser As Boolean = False
    Public Sub getData(ByVal CustomId As String, ByVal CompanyName As String, ByVal CompanyID As String, ByVal Status As String)
        Dim QuoteFromDate As Date = Date.MinValue, QuoteToDate As Date = Date.MaxValue
        GetQuoteFromTo(QuoteFromDate, QuoteToDate)

        Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordV2( _
            txtQuoteId.Text.Trim.Replace("'", "''"), Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, "", QuoteFromDate, QuoteToDate)
    End Sub

    Sub GetQuoteFromTo(ByRef FromDate As Date, ByVal ToDate As Date)
        Dim fd As Date = Date.MinValue, td As Date = Date.MaxValue
        If Date.TryParse(txtQuoteCreateFrom.Text, Now) Then
            fd = CDate(txtQuoteCreateFrom.Text)
        Else
            fd = DateAdd(DateInterval.Month, -1, Now)
        End If
        If Date.TryParse(txtQuoteCreateTo.Text, Now) Then
            td = CDate(txtQuoteCreateTo.Text)
        Else
            td = Now
        End If
        FromDate = fd : ToDate = td
    End Sub
    Private Sub BindGridView(ByVal CustomId As String, ByVal CompanyName As String, ByVal CompanyID As String, ByVal Status As String, Optional ByVal IsReSearch As Boolean = False)
        If ViewState("dt") Is Nothing OrElse IsReSearch Then
            Dim _SONO As String = Trim(Me.TextBox_SONO.Text), _PONO As String = Trim(Me.TextBox_PONO.Text)
            Dim _SQL As String = Business.getQuoteListBySOPOV2(_SONO, _PONO), _QuoteIdIN As String = String.Empty
            If String.IsNullOrEmpty(_SQL) = False Then
                Dim dt1 As DataTable = tbOPBase.dbGetDataTable("EQ", _SQL)
                If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then
                    For Each _row As DataRow In dt1.Rows
                        _QuoteIdIN &= "'" & _row.Item("QuoteID").ToString.Replace("'", "''") & "',"
                    Next
                    _QuoteIdIN = _QuoteIdIN.TrimEnd(",")
                End If
            End If

            Dim QuoteFromDate As Date = Date.MinValue, QuoteToDate As Date = Date.MaxValue
            GetQuoteFromTo(QuoteFromDate, QuoteToDate)
            Dim dt As DataTable = Nothing

            Dim strSearchQuoteSql As String = Business.getMyQuoteRecordV2( _
                                             txtQuoteId.Text.Trim.Replace("'", "''"), Pivot.CurrentProfile.UserId, CustomId, CompanyName, _
                                             CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate)
            dt = tbOPBase.dbGetDataTable("EQ", strSearchQuoteSql)
            ViewState("dt") = dt
        End If

        Me.GridView1.DataSource = CType(ViewState("dt"), DataTable)
        Me.GridView1.DataBind()
    End Sub
    Public Sub ShowData(ByVal CustomId As String, ByVal CompanyName As String, ByVal CompanyID As String, ByVal Status As String, Optional ByVal IsReSearch As Boolean = False)
        'getData(CustomId, CompanyName, CompanyID, Status)
        'Me.GridView1.DataBind()
        BindGridView(CustomId, CompanyName, CompanyID, Status, IsReSearch)
        'Frank 2012/07/23: If user is aonline sales then hiding below columns
        If Me._IsANAUser Then
            Me.GridView1.Columns(1).Visible = False
        End If
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue)
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, True)
    End Sub

    Public Function GetLoginUserIsANAUser() As Boolean
        If ViewState("IsANAUser") Is Nothing Then

            ViewState("IsANAUser") = Role.IsUsaUser()
        End If
        Return ViewState("IsANAUser")
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Pivot.CurrentProfile) Then
            Session.Abandon() : Util.ClearCookie_Login("ADEQCOOK")
            Response.Redirect(String.Format("~/login.aspx?RURL={0}", HttpContext.Current.Server.UrlEncode(Request.RawUrl)))
        End If
        Me._IsANAUser = GetLoginUserIsANAUser()

        'Response.Write(_IsFranchiser)
        If Not IsPostBack Then
            txtQuoteCreateFrom.Text = DateAdd(DateInterval.Month, -1, Now).ToString("yyyy/MM/dd") : txtQuoteCreateTo.Text = Now.ToString("yyyy/MM/dd")
            ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue)
        End If
    End Sub

    Protected Sub ibtnDetail_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value
        Response.Redirect(String.Format("~/Quote/QuotationPreview.aspx?UID={0}", id))
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value
        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        Dim C As IBUS.iDocHeader = Pivot.NewObjDocHeader
        If Not IsNothing(dt) Then

            If dt.qStatus = CInt(COMM.Fixer.eDocStatus.QFINISH) Then
                C.Update(id, String.Format("expiredDate='{0}',quoteDate='{1}'", "1900-1-1", "1900-1-1"), COMM.Fixer.eDocType.EQ)
            Else
                C.Remove(id, COMM.Fixer.eDocType.EQ)
                myQD.Delete(String.Format("quoteId='{0}'", id))
            End If

            ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue)
        End If

    End Sub

    Protected Sub ibtnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value
        Response.Redirect(String.Format("~/Quote/QuotationMaster.aspx?UID={0}", id))
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ID As String = Me.GridView1.DataKeys(e.Row.RowIndex).Value
            'If Not Business.isOrderable(ID) Then
            Dim DBITEM As DataRowView = CType(e.Row.DataItem, DataRowView)
            'If DBITEM.Item("quoteToERPid").ToString.Trim = "" OrElse Franchise.isFranchiser(DBITEM.Item("siebelRBU")) OrElse _
            '    DBITEM.Item("qStatus").ToString.Equals("DRAFT", StringComparison.OrdinalIgnoreCase) Then
            '    CType(e.Row.FindControl("btnOrder"), Button).Visible = False
            'End If
            Dim btnOrder As Button = CType(e.Row.FindControl("btnOrder"), Button)
            btnOrder.Visible = False
            If Business.isOrderable(ID) Then
                btnOrder.Visible = True
            End If
            If (DBITEM.Item("org").ToString.Equals("EU10", StringComparison.OrdinalIgnoreCase)) AndAlso _
               (Not DBITEM.Item("DOCSTATUS") = CInt(COMM.Fixer.eDocStatus.QDRAFT)) Then
                CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = False

            End If
            'If myQM.IsExists(String.Format("quoteId='{0}' and quoteToErpId<>''", ID)) = 1 Then
            '    CType(e.Row.FindControl("btnGetErpId"), Button).Visible = False
            'End If
            CType(e.Row.FindControl("lbStatusV"), Label).Text = COMM.EnumHelper.getDescription(CType(DBITEM.Item("DOCSTATUS"), COMM.Fixer.eDocStatus))
            CType(e.Row.FindControl("lbGpFlow"), Label).Text = GPControl.getApproverStr(ID)
            'CType(e.Row.FindControl("lbSiebelQuoteId"), Label).Text = Business.getSiebelQuoteIdByQuoteId(ID)
            If Business.isExpired(ID, COMM.Fixer.eDocType.EQ) Then
                CType(e.Row.FindControl("lbStatusV"), Label).Text = "Expired"
                CType(e.Row.FindControl("btnOrder"), Button).Visible = False
            End If

            'Frank 2012/06/27: Display the last order log
            Dim _dt As DataTable = tbOPBase.dbGetDataTable("EQ", Business.getQuoteToOrderLog(ID, True))
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                Dim GV_OrderInfo As GridView = CType(e.Row.FindControl("GridView_OrderInfo"), GridView)
                GV_OrderInfo.DataSource = _dt
                GV_OrderInfo.DataBind()
            Else
                Dim _HyperLink_CheckAll As HyperLink = CType(e.Row.FindControl("HyperLink_CheckAll"), HyperLink)
                _HyperLink_CheckAll.Visible = False
            End If


        End If
        'If _IsFranchiser AndAlso (e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header) Then
        '    Dim g As GridView = CType(sender, GridView)
        '    Dim nCompanyID As Integer = 0
        '    Dim nGetERPIDfromSiebel As Integer = 0
        '    Dim nGPFlow As Integer = 0
        '    Dim n As Integer = 0
        '    For Each c As TableCell In g.HeaderRow.Cells
        '        n += 1
        '        Dim txtHeader As LinkButton = c.Controls(0)
        '        Response.Write(txtHeader.Text)
        '        'If txtHeader.Text.Contains("Company ID") Then
        '        '    nCompanyID = n - 1
        '        'End If
        '        'If txtHeader.Text.Contains("Get ERP ID from Siebel") Then
        '        '    nGetERPIDfromSiebel = n - 1
        '        'End If
        '        'If txtHeader.Text.Contains("GP Flow") Then
        '        '    nGPFlow = n - 1
        '        'End If
        '    Next
        '    e.Row.Cells(nCompanyID).Visible = False : e.Row.Cells(nGetERPIDfromSiebel).Visible = False : e.Row.Cells(nGPFlow).Visible = False
        'End If
    End Sub

    Protected Sub ibtnPdf_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value
        Dim O As New BigText("eq", "BigText")
        Dim dtC As New DataTable
        dtC = O.GetDT(String.Format("quoteId='{0}'", id), "")


        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)


        If dtC.Rows.Count > 0 AndAlso Not Role.IsUsaUser() Then
            Dim co As String = dtC.Rows(0).Item("content")
            Dim myContentByte As Byte() = Util.DownloadQuotePDFbyStr(co)
            If Not IsNothing(dt) Then
                Dim fname As String = dt.CustomId.ToString.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
                Response.Clear()
                Response.AddHeader("Content-Type", "binary/octet-stream")
                Response.AddHeader("Content-Disposition", "attachment; filename=" & fname & ".pdf;size = " & myContentByte.Length.ToString())
                Response.Flush()
                Response.BinaryWrite(myContentByte)
                Response.Flush() : Response.End()
            End If
        Else
            If Not IsNothing(dt) Then
                Dim myContentByte As Byte() = Nothing

                If Role.IsUsaUser() Then
                    Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
                    myContentByte = Util.DownloadQuotePDFByHtmlString(strUSAOnlineQuoteTemplateHtml, False)
                Else
                    Dim URL As String = String.Format("http://{0}{1}{2}/quote/{3}?UID={4}&ROLE=1", _
               Request.ServerVariables("SERVER_NAME"), _
               IIf(Request.ServerVariables("SERVER_PORT") = "80", "", ":" + Request.ServerVariables("SERVER_PORT")), _
               HttpRuntime.AppDomainAppVirtualPath, _
               Business.getPiPage(id, dt.DocReg), id)
                    myContentByte = Util.DownloadQuotePDF(URL)
                End If

                Dim fname As String = dt.CustomId.ToString.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
                Response.Clear()
                Response.AddHeader("Content-Type", "binary/octet-stream")
                Response.AddHeader("Content-Disposition", "attachment; filename=" & fname & ".pdf;size = " & myContentByte.Length.ToString())
                Response.Flush()
                Response.BinaryWrite(myContentByte)
                Response.Flush() : Response.End()
            End If
        End If
    End Sub

    Protected Sub btnGetErpId_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(dt) Then
            Business.updateQuoteToErpId(dt.AccRowId, id, COMM.Fixer.eDocType.EQ)
            ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, True)
        End If
    End Sub

    Protected Sub btnOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value
        If Business.isOrderable(id) Then
            Response.Redirect("~/quote/quoteModify.aspx?UID=" & id)
        Else
            Util.showMessage("Invalid ERP ID.")
        End If
    End Sub


    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        'Me.GridView1.S
        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue)
    End Sub

    'Protected Sub btnFW_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim obj As Button = CType(sender, Button)
    '    Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
    '    Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value
    '    Me.txtQuoteIdFW.Text = id
    '    txtFwdSubject.Text = "Advantech eQuotation " & Me.txtQuoteIdFW.Text
    '    Me.MPFW.Show()
    'End Sub

    'Function getPageStr(ByVal UID As String) As String
    '    'Return Business.getPageStr(UID)
    '    Return Business.getPageStrInternal(UID, False)
    'End Function

    Protected Sub btnDeleteBatch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oDate As New Date
        If IsNumeric(Me.txtMonth.Text.Trim) Then
            Dim N As Integer = "-" & Me.txtMonth.Text.Trim
            oDate = DateAdd(DateInterval.Month, N, Now.Date)

            Dim DT As New DataTable
            DT = tbOPBase.dbGetDataTable("EQ", String.Format("Select * from QuotationMaster where QuoteDate<'{0}' and createdBy='{1}' and DOCSTATUS='{2}'", oDate, Pivot.CurrentProfile.UserId, CInt(COMM.Fixer.eDocStatus.QDRAFT)))
            If DT.Rows.Count > 0 Then
                Dim mm As Integer = 0
                For Each R As DataRow In DT.Rows
                    If Business.isEidtableQuote(R.Item("quoteId")) Then
                        mm = mm + 1
                        Pivot.NewObjDocHeader.Remove(R.Item("quoteId"), COMM.Fixer.eDocType.EQ)
                        myQD.Delete(String.Format("quoteid='{0}'", R.Item("quoteId")))
                    End If
                Next
                Util.showMessage("Delete Success! total: " & mm & " pic.")
            End If
        End If
    End Sub


    Public Class TBBasePage
        Inherits System.Web.UI.Page
        Private FIsVerifyRender As Boolean = True
        Public Property IsVerifyRender() As Boolean
            Get
                Return FIsVerifyRender
            End Get
            Set(ByVal value As Boolean)
                FIsVerifyRender = value
            End Set
        End Property
        Public Overrides Sub VerifyRenderingInServerForm(ByVal Control As System.Web.UI.Control)
            If Me.IsVerifyRender Then
                MyBase.VerifyRenderingInServerForm(Control)
            End If
        End Sub
        Public Overrides Property EnableEventValidation() As Boolean
            Get
                If Me.IsVerifyRender Then
                    Return MyBase.EnableEventValidation
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                MyBase.EnableEventValidation = value
            End Set
        End Property
    End Class

    'Protected Sub lnkHideForwardPanel_Click(sender As Object, e As System.EventArgs)
    '    Me.MPFW.Hide()
    'End Sub

End Class
