Public Class rGPControl
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not PROF.RoleCheck.isAdmin(Pivot.CurrentProfile.UserId) AndAlso
               Not Pivot.CurrentProfile.UserId.Equals("Abderrahim.Bouiba@advantech.nl", StringComparison.InvariantCultureIgnoreCase) Then
                Response.Redirect(String.Format("~/Home.aspx"))
            End If
        End If
    End Sub

    Protected Sub imgXls_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        '    Dim str As String = "select quoteId AS [QuoteID],quoteNo,Revision_Number,quoteToErpId as [Company Id],customid as [Description], quoteToName as [Company Name], " & _
        '" (select sum(newunitprice*qty) from quotationDetail where quotationDetail.quoteid=quotationMaster.quoteid) as [Total Amount],Currency , " & _
        '" (select GPlever FROM quotation_approval where quotation_approval.quote_id=quotationMaster.quoteid And Approval_level=1) AS [GP1%], " & _
        '" (select Approver FROM quotation_approval where quotation_approval.quote_id=quotationMaster.quoteid And Approval_level=1) AS [GP Manager 1], " & _
        '" (select case when [status]>0 then 'Approved' when [status]=0 then 'Approving' when [status]<0 then 'Rejected' end FROM quotation_approval where quotation_approval.quote_id=quotationMaster.quoteid and Approval_level=1)  AS [First Approval] , " & _
        '" (select GPlever FROM quotation_approval where quotation_approval.quote_id=quotationMaster.quoteid and Approval_level=2) AS [GP2%], " & _
        '" (select Approver FROM quotation_approval where quotation_approval.quote_id=quotationMaster.quoteid and Approval_level=2) AS [GP Manager 2], " & _
        '" (select case when [status]>0 then 'Approved' when [status]=0 then 'Approving' when [status]<0 then 'Rejected' end FROM quotation_approval where quotation_approval.quote_id=quotationMaster.quoteid and Approval_level=2) AS [Second Approval], " & _
        '" QuoteDate as [Creation Date] " & _
        '" from quotationMaster where active=1 and year(quoteDate)='" & Me.txtYear.Text.Trim & "' and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "' "

        'AEU Version for Ruud

        Dim startDate As DateTime = DateTime.Now, endDate As DateTime = DateTime.Now
        DateTime.TryParseExact(Me.txtStartDate.Text, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, startDate)
        DateTime.TryParseExact(Me.txtEndDate.Text, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, endDate)

        Dim str As String = String.Format("select isnull(a.quoteNo,a.quoteid) as QuoteNo,a.quoteToErpId as [Company Id],a.customid as [Description], a.quoteToName as [Company Name], " &
                            " a.SalesEmail,a.CreatedBy, " &
                            " isnull(b.SECTOR,'') as [Sector], a.SalesGroup, a.SalesOffice, " &
                            " (select sum(newunitprice*qty) from quotationDetail where quotationDetail.quoteid=a.quoteid) as [Total Amount],a.Currency , " &
                            " (select GPlever FROM quotation_approval where quotation_approval.quote_id=a.quoteid and Approval_level=1) AS [GP1%], " &
                            " (select Approver FROM quotation_approval where quotation_approval.quote_id=a.quoteid and Approval_level=1) AS [GP Manager 1], " &
                            " (select case when [status]>0 then 'Approved' when [status]=0 then 'Approving' when [status]<0 then 'Rejected' end FROM quotation_approval where quotation_approval.quote_id=a.quoteid and Approval_level=1)  AS [First Approval] , " &
                            " (select GPlever FROM quotation_approval where quotation_approval.quote_id=a.quoteid and Approval_level=2) AS [GP2%], " &
                            " (select Approver FROM quotation_approval where quotation_approval.quote_id=a.quoteid and Approval_level=2) AS [GP Manager 2], " &
                            " (select case when [status]>0 then 'Approved' when [status]=0 then 'Approving' when [status]<0 then 'Rejected' end FROM quotation_approval where quotation_approval.quote_id=a.quoteid and Approval_level=2) AS [Second Approval], " &
                            " (select GPlever FROM quotation_approval where quotation_approval.quote_id=a.quoteid and Approval_level=3) AS [GP3%], " &
                            " (select Approver FROM quotation_approval where quotation_approval.quote_id=a.quoteid and Approval_level=3) AS [GP Manager 3], " &
                            " (select case when [status]>0 then 'Approved' when [status]=0 then 'Approving' when [status]<0 then 'Rejected' end FROM quotation_approval where quotation_approval.quote_id=a.quoteid and Approval_level=3) AS [Third Approval], " &
                            " a.QuoteDate as [Creation Date] " &
                            " from quotationMaster a left join MyAdvantechGlobal.dbo.SAP_DIMCOMPANY b on a.quoteToErpId=b.company_id " &
                            " where a.org='EU10' and a.active=1 and (a.quoteDate<='{0}' and a.quoteDate>='{1}') and a.DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "' " &
                            " order by a.QuoteDate ", endDate.ToString("yyyyMMdd"), startDate.ToString("yyyyMMdd"))

        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("eq", str)
        Util.DataTable2ExcelDownload(dt, "GPControl")
    End Sub

End Class