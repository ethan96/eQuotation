Imports System.IO

Public Class Home
    Inherits System.Web.UI.Page

    Private _IsTWAonlineSales As Boolean = False
    Private _IsCNAonlineSales As Boolean = False
    Private _IsKRAonlineSales As Boolean = False
    Private _IsHQDCSales As Boolean = False
    Private _IsUSAonlineEC As Boolean = False
    Private _IsABRSales As Boolean = False
    Private _IsAAUSales As Boolean = False


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Pivot.CurrentProfile) Then

            Session.Abandon() : Util.ClearCookie_Login("ADEQCOOK")
            Response.Redirect(String.Format("~/login.aspx?RURL={0}", HttpContext.Current.Server.UrlEncode(Request.RawUrl)))
        End If

        If Not Page.IsPostBack AndAlso Not IsNothing(Pivot.CurrentProfile) Then

            _IsTWAonlineSales = Role.IsTWAonlineSales()
            _IsCNAonlineSales = Role.IsCNAonlineSales()
            _IsKRAonlineSales = Role.IsKRAonlineSales()
            _IsHQDCSales = Role.IsHQDCSales()
            _IsUSAonlineEC = Role.IsAonlineUsa
            _IsABRSales = Role.IsABRSales
            _IsAAUSales = Role.IsAAUSales

            'Frank 2013/08/06 Upload Signature function link
            If Role.IsEUSales Then
                trSignature.Visible = True
            Else
                trSignature.Visible = False
            End If

            If Role.IsUsaUser() Then
                trUsQuickConfig.Visible = True : trUsSallBTOS.Visible = True : trCopyQuotation.Visible = False
                trMyTeamQuote.Visible = False : hyMyAccountQuote.Text = "My Team's Quotation" : hyCopyQuote.Visible = False : Me.trReport.Visible = True

            ElseIf _IsABRSales Then
                trCopyQuotation.Visible = False : trMyTeamQuote.Visible = False
                hyMyAccountQuote.Text = "My Team's Quotation" : hyCopyQuote.Visible = False
                Me.trReport.Visible = False

            ElseIf _IsAAUSales Then
                trCopyQuotation.Visible = False : trMyTeamQuote.Visible = False
                hyMyAccountQuote.Text = "My Team's Quotation" : hyCopyQuote.Visible = False
                Me.trReport.Visible = False

            ElseIf _IsTWAonlineSales Then
                trCopyQuotation.Visible = False : trMyTeamQuote.Visible = False
                hyMyAccountQuote.Text = "My Team's Quotation" : hyCopyQuote.Visible = False
                Me.trReport.Visible = False : Me.trSiebelQuoteToOrder.Visible = True
                Me.trSOPforATWAOnline.Visible = True

            ElseIf _IsCNAonlineSales Then
                trCopyQuotation.Visible = False : trMyTeamQuote.Visible = False
                hyMyAccountQuote.Text = "My Team's Quotation" : hyCopyQuote.Visible = False
                Me.trReport.Visible = False : Me.trSiebelQuoteToOrder.Visible = False
                Me.trSOPforATWAOnline.Visible = False

            ElseIf _IsKRAonlineSales Then
                trCopyQuotation.Visible = False : trMyTeamQuote.Visible = False
                hyMyAccountQuote.Text = "My Team's Quotation" : hyCopyQuote.Visible = False
                Me.trReport.Visible = False : Me.trSiebelQuoteToOrder.Visible = False
                Me.trSOPforATWAOnline.Visible = False : Me.trSOPforAKRAOnline.Visible = True

            ElseIf _IsHQDCSales Then
                trCopyQuotation.Visible = False : trMyTeamQuote.Visible = False
                hyMyAccountQuote.Text = "My Team's Quotation" : hyCopyQuote.Visible = False
                Me.trReport.Visible = False : Me.trSiebelQuoteToOrder.Visible = False
                Me.trSOPforATWAOnline.Visible = False : Me.trSOPforIntercon.Visible = True

            ElseIf Pivot.CurrentProfile.GroupBelTo.Contains(("Sales.AEU").ToUpper) OrElse Pivot.CurrentProfile.GroupBelTo.Contains(("OP.AEU").ToUpper) _
                OrElse Pivot.CurrentProfile.GroupBelTo.Contains(("FINANCE.AEU").ToUpper) OrElse Pivot.CurrentProfile.GroupBelTo.Contains(("AESC.Mgt").ToUpper) Then
                'hyMyTeamQuote.NavigateUrl = "~/Quote/myTeamsQuotation.aspx"
                trAEUITP.Visible = True : trDxPrice.Visible = True : trGP.Visible = True : Me.trReport.Visible = True
            End If
            If Role.IsFranchiser() Then
                Me.trCopySAPOrder.Visible = False : Me.trMyTeamQuote.Visible = False
            End If
            If Role.IsJPAonlineSales() Then
                trMyTeamQuote.Visible = False
                hyMyAccountQuote.Text = "My Team's Quotation"
            End If


            'Frank 2013/10/16: Only US AOnline power user and system administrator can access unit price overwriting function
            'If Pivot.CurrentProfile.UserId.ToUpper.Contains(("Cathee.Cao").ToUpper) OrElse _
            '   Pivot.CurrentProfile.UserId.ToUpper.Contains(("Jay.Lee").ToUpper) OrElse _
            '   Role.IsAdmin Then

            'Hello Jay, 
            'Per Fei, please provide Fei and me the access right to allow leveling pricing per ERP ID and/or quote.
            'Gabriela has a new customer that will need to be quoted L4 pricing
            'Regards, 
            'Denise Kwong

            'Hi Frank,
            'Can you also grant Denise access to eQuotation price change function too?
            'Thanks,
            'Jay

            'Fei:Since I have been questioned by Ween on our margin drop by AOnline, I need to have Jay reverse all the GP block authority to me. Starting from today.

            'Jay 2014/10/01 Can you temporarily remove Cathee’s access at overwriting eQuotation price and grant this access to Fei?

            'Jay 2014/12/05
            'Hi TC and Frank,
            'Once Aonline IAG group is up and ready at eQuotation, please make sure you grant price update right to only Gary and Denise.
            'If Pivot.CurrentProfile.UserId.ToUpper.Contains(("Fei.Khong").ToUpper) OrElse _
            '   Pivot.CurrentProfile.UserId.ToUpper.Contains(("Denise.Kwong").ToUpper) OrElse _
            '   Pivot.CurrentProfile.UserId.ToUpper.Contains(("Jay.Lee").ToUpper) OrElse _
            '   Pivot.CurrentProfile.UserId.ToUpper.Contains(("Gary.Lee").ToUpper) OrElse _
            '   Role.IsAdmin Then
            If Role.IsAUSpowerUser(Pivot.CurrentProfile.UserId) Then
                Me.trUSPriceOW.Visible = True
            End If

            'Ryan 20170224 If is AUS power user of some specific users, set trOptyManagemnet visible to true.
            If Role.IsAUSpowerUser(Pivot.CurrentProfile.UserId) _
                OrElse Pivot.CurrentProfile.UserId.Equals("tara.valle@advantech.com") _
                OrElse Pivot.CurrentProfile.UserId.Equals("mike.arcure@advantech.com") _
                OrElse Pivot.CurrentProfile.UserId.Equals("noel.nguyen@advantech.com") Then
                Me.trOptyManagement.Visible = True
            End If

            If _IsUSAonlineEC Then
                Me.trMyPipeline.Visible = True

                Me.rblPipelineYear.Items.Add(New ListItem(Date.Now.Year - 1, Date.Now.Year - 1))
                Me.rblPipelineYear.Items.Add(New ListItem(Date.Now.Year, Date.Now.Year))
                Me.rblPipelineYear.Items.Add(New ListItem(Date.Now.Year + 1, Date.Now.Year + 1))
                Me.rblPipelineYear.Items.Add(New ListItem(Date.Now.Year + 2, Date.Now.Year + 2))
                Me.rblPipelineYear.Items.FindByValue(Date.Now.Year).Selected = True
            End If

            'Frank 20150903 ANA iA usage report for Lynette only
            If Role.IsAACpowerUser(Pivot.CurrentProfile.UserId) Then
                trANAiAUsageReport.Visible = True
            End If

            'Ryan 20160331 Pipeline report visibility
            If Advantech.Myadvantech.Business.UserRoleBusinessLogic.CanSeePipelineReport(Pivot.CurrentProfile.UserId) Then
                trPipeline.Visible = True
            End If

            'Ryan 20160728 US AENC team menu visibility
            If Role.IsUsaAenc() Then
                trUsQuickConfig.Visible = False
                trUsSallBTOS.Visible = False
            End If

            If Role.IseQuotationBusinessOwner(Pivot.CurrentProfile.UserId) Then
                trGP.Visible = True
            End If

            'Frank 20180403
            'Display reporting function for AASC SCM team member
            If Advantech.Myadvantech.Business.UserRoleBusinessLogic.IsInGroupUsaAASCSCMOnly(Pivot.CurrentProfile.GroupBelTo) Then
                MainTable.Visible = False
                SubTableForANASCMTeamOnly.Visible = True
            End If


        End If
    End Sub

    Protected Sub btnUSPipelineYearSelect_Click(sender As Object, e As EventArgs)
        Dim pipeline As New Advantech.Myadvantech.DataAccess.Pipeline.general()

        ' Ryan 20170104 GetPIPELineExcelReport is deprecated, use GetPIPELineExcelReportByYear instead
        'Dim _excelfilebyte() As Byte = pipeline.GetPIPELineExcelReport(Pivot.CurrentProfile.UserId)

        Dim _excelfilebyte() As Byte = pipeline.GetPIPELineExcelReportByYearAndStage(Pivot.CurrentProfile.UserId, rblPipelineYear.SelectedValue, rblPipelineOppType.SelectedValue.ToString)
        Response.Clear()

        Dim MS As MemoryStream = New MemoryStream(_excelfilebyte)
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("content-disposition", "attachment;filename=MyPipeline.xls")
        Response.Buffer = True
        MS.WriteTo(Response.OutputStream)
        Response.End()
    End Sub
End Class