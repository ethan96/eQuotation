Imports Advantech.Myadvantech.DataAccess

Public Class QuoteApproval
    Inherits System.Web.UI.Page
    Private ReadOnly Property CurrQuotationMaster
        Get
            If Request("UID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request("UID")) Then
                Dim QM As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(Request("UID").ToString.Trim())
                Return QM
            End If
            Return Nothing
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            getPageStr(Request("UID"))
            Dim _NextQuotation_Approval_Expiration As Quotation_Approval_Expiration = Nothing
            Dim _QMaster As QuotationMaster = Me.CurrQuotationMaster
            If _QMaster Is Nothing Then Exit Sub
            Dim _ME As QuotationExtension = _QMaster.QuoteExtensionX
            If _ME Is Nothing Then Exit Sub
            If _ME.ApprovalFlowTypeX = eQApprovalFlowType.ThirtyDaysExpiration Then
                Dim QAE As IEnumerable(Of Quotation_Approval_Expiration) = _QMaster.QuotationApprovalExpiration
                If QAE Is Nothing Then
                    Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Error Massage by " + Util.GetCurrentUserID(), "", "QuotationApprovalExpiration for " + Request("UID").ToString.Trim + "  is no row ", "")
                    Exit Sub
                End If
                _NextQuotation_Approval_Expiration = QAE.Where(Function(p) p.Status.Value = 0).OrderBy(Function(p) p.ApprovalLevel).FirstOrDefault()
                If _NextQuotation_Approval_Expiration IsNot Nothing AndAlso _NextQuotation_Approval_Expiration.Approver = Pivot.CurrentProfile.UserId.ToString.ToUpper Then
                    Me.btnProcess.Enabled = True
                End If
            Else
                If GPControl.getNextApprover(Request("UID")).ToUpper = Pivot.CurrentProfile.UserId.ToString.ToUpper Then
                    Me.btnProcess.Enabled = True
                End If
            End If
            If COMM.Util.IsTesting() Then
                Me.btnProcess.Enabled = True
                If _NextQuotation_Approval_Expiration IsNot Nothing Then
                    Me.btnProcess.Text = "Process for " + _NextQuotation_Approval_Expiration.Approver
                End If
            End If
        End If
    End Sub

    Sub getPageStr(ByVal UID As String)
        Dim QM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        Me.divContent.InnerHtml = Business.getPageStrInternal(UID, QM.DocReg)
    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.UPProcess.Update()
        Me.MPProcess.Show()
    End Sub

    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'If GPControl.getNextApprover(Request("UID")).ToUpper <> Pivot.CurrentProfile.UserId.ToString.ToUpper Then
        '    Exit Sub
        'End If

        'Dim AppId As String = GPControl.GP_getMobileYesOrNoUniqCodeByLevel(Request("UID"), GPControl.getNextLevel(Request("UID")), "YES")
        'GPControl.doApprove(Request("UID"), AppId, Me.txtComment.Text)
        'Business.send_Quotation_Approval(Request("UID"), Me.txtComment.Text.Trim, COMM.Fixer.eDocType.EQ)
        'If GPControl.isApproved(Request("UID")) Then
        '    '20131210 Ming : After approved Quote,  update it's Status in time
        '    Pivot.NewObjDocHeader.ChangeDocStatus(Request("UID"), CInt(COMM.Fixer.eDocStatus.QFINISH), COMM.Fixer.eDocType.EQ)
        '    'end
        '    Business.transQuote2Siebel(Request("UID"))
        'End If
        'Response.Redirect("~/Quote/QuoteApproval.aspx?UID=" & Request("UID"))
        doApprove("YES")
    End Sub

    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'If GPControl.getNextApprover(Request("UID")).ToUpper <> Pivot.CurrentProfile.UserId.ToString.ToUpper Then
        '    Exit Sub
        'End If

        'Dim AppId As String = GPControl.GP_getMobileYesOrNoUniqCodeByLevel(Request("UID"), GPControl.getNextLevel(Request("UID")), "NO")
        'GPControl.doApprove(Request("UID"), AppId, Me.txtComment.Text)
        'Business.send_Quotation_Approval(Request("UID"), Me.txtComment.Text.Trim, COMM.Fixer.eDocType.EQ)
        'Response.Redirect("~/Quote/QuoteApproval.aspx?UID=" & Request("UID"))
        doApprove("NO")
    End Sub
    Private Sub doApprove(ByVal dotype As String)
        Dim _QMaster As QuotationMaster = Me.CurrQuotationMaster
        If _QMaster Is Nothing Then Exit Sub
        Dim _ME As QuotationExtension = _QMaster.QuoteExtensionX
        If _ME Is Nothing Then Exit Sub
        '\Ming add 20150409 如果eQApprovalFlowType 为null，就去相关数据表检查类型
        Dim currApprovalFlowTyp As eQApprovalFlowType = eQApprovalFlowType.Normal
        If Not (IsDBNull(_ME.ApprovalFlowType) OrElse _ME.ApprovalFlowType Is Nothing) Then
            currApprovalFlowTyp = _ME.ApprovalFlowTypeX
        Else
            Dim obj As Object = tbOPBase.dbExecuteScalar("EQ", String.Format("select  COUNT(QuoteID) as num from  Quotation_Approval_Expiration where QuoteID='{0}'", _ME.QuoteID))
            If obj IsNot Nothing AndAlso IsNumeric(obj) Then
                If Integer.Parse(obj) > 0 Then
                    currApprovalFlowTyp = eQApprovalFlowType.ThirtyDaysExpiration
                End If
            End If
            obj = tbOPBase.dbExecuteScalar("EQ", String.Format("select COUNT(Quote_ID) as num  from quotation_approval where Quote_ID='{0}'", _ME.QuoteID))
            If obj IsNot Nothing AndAlso IsNumeric(obj) Then
                If Integer.Parse(obj) > 0 Then
                    currApprovalFlowTyp = eQApprovalFlowType.GP
                End If
            End If
        End If
        '/end
        If currApprovalFlowTyp = eQApprovalFlowType.ThirtyDaysExpiration Then
            Dim QAE As IEnumerable(Of Quotation_Approval_Expiration) = _QMaster.QuotationApprovalExpiration
            If QAE Is Nothing Then
                Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "QuoteApproval Error Massage by " + Util.GetCurrentUserID(), "", "QuotationApprovalExpiration for " + Request("UID").ToString.Trim + "  is no row ", "")
                Exit Sub
            End If
            Dim _NextQuotation_Approval_Expiration As Quotation_Approval_Expiration = QAE.Where(Function(p) p.Status.Value = 0).OrderBy(Function(p) p.ApprovalLevel).FirstOrDefault()
            If _NextQuotation_Approval_Expiration IsNot Nothing Then
                Select Case dotype
                    Case "YES"
                        _NextQuotation_Approval_Expiration.Status = 1
                    Case "NO"
                        _NextQuotation_Approval_Expiration.Status = -1
                End Select
                eQuotationContext.Current.SaveChanges()
                If QAE.Sum(Function(p) p.Status.Value) = QAE.Count Then
                    _QMaster.DOCSTATUS = CInt(COMM.Fixer.eDocStatus.QFINISH)
                    eQuotationContext.Current.SaveChanges()
                    Business.transQuote2Siebel(Request("UID"))
                End If
                Business.send_Quotation_Approval_Expiration(Request("UID"), Me.txtComment.Text.Trim, COMM.Fixer.eDocType.EQ)
            End If
            'Response.Redirect("~/Quote/QuoteApproval.aspx?UID=" & Request("UID"))
        Else
            Select Case dotype
                Case "YES"
                    Dim AppId As String = GPControl.GP_getMobileYesOrNoUniqCodeByLevel(Request("UID"), GPControl.getNextLevel(Request("UID")), "YES")
                    GPControl.doApprove(Request("UID"), AppId, Me.txtComment.Text)
                    Business.send_Quotation_Approval(Request("UID"), Me.txtComment.Text.Trim, COMM.Fixer.eDocType.EQ)
                    If GPControl.isApproved(Request("UID")) Then
                        '20131210 Ming : After approved Quote,  update it's Status in time
                        Pivot.NewObjDocHeader.ChangeDocStatus(Request("UID"), CInt(COMM.Fixer.eDocStatus.QFINISH), COMM.Fixer.eDocType.EQ)
                        'end
                        Business.transQuote2Siebel(Request("UID"))
                    End If
                    'Response.Redirect("~/Quote/QuoteApproval.aspx?UID=" & Request("UID"))
                Case "NO"
                    Dim AppId As String = GPControl.GP_getMobileYesOrNoUniqCodeByLevel(Request("UID"), GPControl.getNextLevel(Request("UID")), "NO")
                    GPControl.doApprove(Request("UID"), AppId, Me.txtComment.Text)
                    Business.send_Quotation_Approval(Request("UID"), Me.txtComment.Text.Trim, COMM.Fixer.eDocType.EQ)
                    ' Response.Redirect("~/Quote/QuoteApproval.aspx?UID=" & Request("UID"))
            End Select
        End If
        'Response.Redirect("~/Quote/QuoteApproval.aspx?UID=" & Request("UID"))
        Dim RetUrl As String = String.Format("{0}/Quote/myTeamsQuotation.aspx", Util.GetRuntimeSiteUrl())
        ScriptManager.RegisterStartupScript(UPProcess, UPProcess.GetType(), "updateScript", "window.location.href='" + RetUrl + "'", True)

    End Sub
End Class