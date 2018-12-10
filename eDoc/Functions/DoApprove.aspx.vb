Imports Advantech.Myadvantech.DataAccess

Public Class DoApprove
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Not IsPostBack AndAlso Request("UID") IsNot Nothing AndAlso Request("AC") IsNot Nothing Then
        '    GPControl.doApprove(Request("UID"), Request("AC"), "")
        '    Business.send_Quotation_Approval(Request("UID"), "", COMM.Fixer.eDocType.EQ)
        '    If GPControl.isApproved(Request("UID")) Then
        '        '20131210 Ming : After approved Quote,  update it's Status in time
        '        Pivot.NewObjDocHeader.ChangeDocStatus(Request("UID"), CInt(COMM.Fixer.eDocStatus.QFINISH), COMM.Fixer.eDocType.EQ)
        '        'end
        '        Business.transQuote2Siebel(Request("UID"))
        '    End If
        '    Response.Write("Your request has been proceeded successfully.")
        'End If

        Dim _UID As String = String.Empty, _AC As String = String.Empty

        If Request("UID") IsNot Nothing Then _UID = Request("UID")
        If Request("AC") IsNot Nothing Then _AC = Request("AC")
        Dim _content As New StringBuilder
        _content.AppendLine("QuoteID=" & _UID)
        _content.AppendLine("<br />AC=" & _AC)
        _content.AppendLine("<br />Time=" & Now.ToString)
        'Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "(eQ) Mobile approval page has been called (QuoteID=  " & _UID & ")", "", _content.ToString, "")
        If Not IsPostBack AndAlso Not String.IsNullOrEmpty(_UID) AndAlso Not String.IsNullOrEmpty(_AC) Then
            Dim dtM As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(_UID)
            If dtM IsNot Nothing Then
                Dim _ME As QuotationExtension = dtM.QuoteExtensionX
                If _ME Is Nothing Then Exit Sub
                If _ME.ApprovalFlowTypeX = eQApprovalFlowType.ThirtyDaysExpiration Then
                    Dim QAE As IEnumerable(Of Quotation_Approval_Expiration) = dtM.QuotationApprovalExpiration
                    If QAE Is Nothing Then Exit Sub
                    'do approve
                    For Each currQAE In QAE
                        If currQAE.MobileApproveYes = _AC Then
                            currQAE.Status = 1
                            Exit For
                        End If
                        If currQAE.MobileApproveNo = _AC Then
                            currQAE.Status = -1
                            Exit For
                        End If
                    Next
                    eQuotationContext.Current.SaveChanges()
                    If QAE.Sum(Function(p) p.Status.Value) = QAE.Count Then
                        dtM.DOCSTATUS = CInt(COMM.Fixer.eDocStatus.QFINISH)
                        eQuotationContext.Current.SaveChanges()
                        Business.transQuote2Siebel(_UID)
                    End If
                    Business.send_Quotation_Approval_Expiration(_UID, "", COMM.Fixer.eDocType.EQ)
                    Response.Write("Your request has been proceeded successfully.")
                Else
                    GPControl.doApprove(_UID, _AC, "")
                    Business.send_Quotation_Approval(_UID, "", COMM.Fixer.eDocType.EQ)
                    If GPControl.isApproved(_UID) Then
                        '20131210 Ming : After approved Quote,  update it's Status in time
                        Pivot.NewObjDocHeader.ChangeDocStatus(_UID, CInt(COMM.Fixer.eDocStatus.QFINISH), COMM.Fixer.eDocType.EQ)
                        'end
                        Business.transQuote2Siebel(_UID)
                    End If
                    Response.Write("Your request has been proceeded successfully.")
                End If

            End If


            'Dim _NextQuotation_Approval_Expiration As Quotation_Approval_Expiration = QAE.Where(Function(p) p.Status.Value = 0).OrderBy(Function(p) p.ApprovalLevel).FirstOrDefault()
            'Dim uniqidY As String = _NextQuotation_Approval_Expiration.MobileApproveYes
            'Dim uniqidN As String = _NextQuotation_Approval_Expiration.MobileApproveNo



        End If
    End Sub

End Class