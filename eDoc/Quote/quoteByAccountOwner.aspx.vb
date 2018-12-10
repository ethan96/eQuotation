Imports System.Net

Public Class quoteByAccountOwner
    Inherits System.Web.UI.Page
    Public myQD As New quotationDetail("EQ", "quotationDetail")
    Public _IsANAUser As Boolean = True, _IsMyTeamsMode As Boolean = True, _TimeSpan As TimeSpan, _ServerPath As String = ""
    Public _IsFranchiser As Boolean = False, _IsJPAonline As Boolean = False
    Public _IsTWUser As Boolean = False, _IsCNAonline As Boolean = False
    Public _IsAACUser As Boolean = False, _IsKRAonline As Boolean = False
    Public _IsHQDCUser As Boolean = False, _IsAENCUser As Boolean = False
    Public _IsANAAOlineEP As Boolean = False, _IsAonlineIAG As Boolean = False, _IsAonlineISG As Boolean = False
    Public _IsABRUser As Boolean = False, _IsAAUUser As Boolean = False
    Public _IsCAPS As Boolean = False, _IsEUUser As Boolean = False

    Public Sub getMyTeamData(ByVal CustomId As String, ByVal CompanyName As String, ByVal CompanyID As String, ByVal Status As String, ByVal OptyProjectName As String)

        Dim _SONO As String = Trim(Me.TextBox_SONO.Text), _PONO As String = Trim(Me.TextBox_PONO.Text)
        Dim _QuoteIdIN As String = Business.getQuoteIDListByeQuotationSOPO(_SONO, _PONO)
        Dim QuoteFromDate As Date = Date.MinValue, QuoteToDate As Date = Date.MaxValue
        Dim _createdby As String = Trim(Me.txtCreatedBy.Text)
        GetQuoteFromTo(QuoteFromDate, QuoteToDate)

        If _IsCNAonline Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerCN(txtQuoteId.Text.Trim.Replace("'", "''"), Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, Me.drpDatePeriod.SelectedValue)

        ElseIf (_IsANAUser AndAlso Not _IsAENCUser) Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerUSAOnline(txtQuoteId.Text, "", CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, Me.drpDatePeriod.SelectedValue)
        ElseIf _IsAENCUser Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerUSAENC(txtQuoteId.Text, Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, OptyProjectName, Me.drpDatePeriod.SelectedValue)
        ElseIf _IsCAPS Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerCAPS(txtQuoteId.Text, Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, OptyProjectName, Me.drpDatePeriod.SelectedValue)

        ElseIf _IsTWUser Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerATW(txtQuoteId.Text, Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, OptyProjectName, Me.drpDatePeriod.SelectedValue)

        ElseIf _IsKRAonline Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerAKRV2(txtQuoteId.Text, Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, OptyProjectName, Me.drpDatePeriod.SelectedValue)

        ElseIf _IsJPAonline Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerAJPV2(txtQuoteId.Text, Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, Me.drpDatePeriod.SelectedValue)

        ElseIf _IsHQDCUser Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerHQDC(txtQuoteId.Text, Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, OptyProjectName, Me.drpDatePeriod.SelectedValue)
        ElseIf _IsABRUser Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerABR(txtQuoteId.Text, Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, OptyProjectName, Me.drpDatePeriod.SelectedValue)
        ElseIf _IsAAUUser Then
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwnerAAU(txtQuoteId.Text, Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, OptyProjectName, Me.drpDatePeriod.SelectedValue)
        Else
            Me.SqlDataSource1.SelectCommand = Business.getMyQuoteRecordByAccountOwner(txtQuoteId.Text.Trim.Replace("'", "''"), Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, _createdby, Me.drpDatePeriod.SelectedValue)
        End If
    End Sub

    Sub GetQuoteFromTo(ByRef FromDate As Date, ByRef ToDate As Date)

        Dim fd As Date = Date.MinValue, td As Date = Date.MaxValue
        If Date.TryParse(txtQuoteCreateFrom.Text, Now) Then
            fd = CDate(txtQuoteCreateFrom.Text)
        Else

            fd = Nothing

            'If _IsANAUser Then
            '    fd = DateAdd(DateInterval.Month, -6, Now)
            'Else
            '    fd = DateAdd(DateInterval.Month, -6, Now)
            'End If
        End If
        If Date.TryParse(txtQuoteCreateTo.Text, Now) Then
            td = CDate(txtQuoteCreateTo.Text)
        Else
            td = Nothing
            'td = Now
        End If
        FromDate = fd : ToDate = td
    End Sub



    Private Sub BindGridViewV2(ByVal CustomId As String, ByVal CompanyName As String, ByVal CompanyID As String, ByVal Status As String, ByVal OptyProjectName As String)
        Dim _SONO As String = Trim(Me.TextBox_SONO.Text), _PONO As String = Trim(Me.TextBox_PONO.Text)

        'Dim _SQL As String = Business.getQuoteListBySOPO(_SONO, _PONO), _QuoteIdIN As String = String.Empty
        'If String.IsNullOrEmpty(_SQL) = False Then
        '    Dim dt1 As DataTable = tbOPBase.dbGetDataTable("B2B", _SQL)
        '    If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then
        '        For Each _row As DataRow In dt1.Rows
        '            _QuoteIdIN &= "'" & _row.Item("QuoteID").ToString.Replace("'", "''") & "',"
        '        Next
        '        _QuoteIdIN = _QuoteIdIN.TrimEnd(",")
        '    End If
        'End If

        Dim _QuoteIdIN As String = Business.getQuoteIDListByeQuotationSOPO(_SONO, _PONO)

        Dim QuoteFromDate As Date = Date.MinValue, QuoteToDate As Date = Date.MaxValue
        GetQuoteFromTo(QuoteFromDate, QuoteToDate)
        Dim dt As DataTable = Nothing

        Dim strSearchQuoteSql As String = Business.getMyQuoteRecordV3(
                                         txtQuoteId.Text.Trim.Replace("'", "''"), Pivot.CurrentProfile.UserId, CustomId, CompanyName,
                                         CompanyID, Status, _QuoteIdIN, QuoteFromDate, QuoteToDate, OptyProjectName, Me.drpDatePeriod.SelectedValue)
        Me.SqlDataSource1.SelectCommand = strSearchQuoteSql
    End Sub


    Protected Function GetData(ByVal obj As Object) As DataTable
        'Dim myQM As New QuotationMaster1("EQ", "quotationMaster")
        'Dim dt As DataTable = myQM.GetDT(String.Format("OriginalQuoteID='{0}'", obj.ToString), " quoteDate desc ")

        Dim sql As New StringBuilder
        With sql
            .AppendLine("")
            .AppendLine(" select distinct a.QuoteID,a.QuoteNo,a.Revision_Number, a.quoteToName, a.quoteDate, a.customId, a.quoteToErpId")
            .AppendLine(" , a.siebelRBU, a.DOCSTATUS, a.org ,a.expiredDate,a.createdby,a.isRepeatedOrder,a.docReg,qstatus ")

            'Ryan 20160726 Revised for pipeline status
            If _IsANAAOlineEP Then
                .AppendLine(" ,(SELECT top 1 Probability FROM eQuotation.dbo.pipelineReport where pipelineReport.quoteId=a.Quoteid and isactive=1) as Probability ")
                .AppendLine(" ,case when (SELECT top 1 QuoteId FROM eQuotation.dbo.pipelineReport where pipelineReport.quoteId=a.Quoteid) is null then 0 else 1 end as isPipelined ")
            End If

            'Ryan 20170413 Add latest order log 
            .AppendLine(" ,IsNull((select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID=a.quoteId order by z.ORDER_DATE desc for xml path('')),'') as latest_order_log")

            .AppendLine(" FROM QUOTATIONMASTER a ")

            .AppendLine(" WHERE DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "' and Active=0 and a.QuoteNo='" & obj.ToString & "' ")
            .AppendLine(" Order by Revision_Number ")
        End With


        'Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", String.Format("SELECT a.QuoteID,a.QuoteNo,a.Revision_Number FROM QUOTATIONMASTER a WHERE DOCSTATUS <>'" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "' and Active=0 and QuoteNo='" & obj.ToString & "' order by Revision_Number"))
        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", sql.ToString)
        Return dt
    End Function




    Public Sub ShowData(ByVal CustomId As String, ByVal CompanyName As String, ByVal CompanyID As String, ByVal Status As String, ByVal OptyProjectName As String, Optional ByVal IsReSearch As Boolean = False)
        If _IsMyTeamsMode Then
            getMyTeamData(CustomId, CompanyName, CompanyID, Status, OptyProjectName)
        Else
            BindGridViewV2(CustomId, CompanyName, CompanyID, Status, OptyProjectName)
        End If

        'Frank 2012/07/23: If user is aonline sales then hiding below columns
        If _IsANAUser Then
            Me.GridView1.Columns(1).Visible = False : Me.GridView1.Columns(6).Visible = False
            'Frank 20180307: show GP flow info. for ANA Aonline IIoT sales team
            If _IsAonlineIAG OrElse _IsAonlineISG Then
                Me.GridView1.Columns(6).Visible = True
            End If
        End If

        'Frank 2015/11/17 Show GP flow info. for Intercon user
        If _IsHQDCUser Then
            Me.GridView1.Columns(1).Visible = False
            Me.GridView1.Columns(6).Visible = True
        End If

        'Frank 2015/11/17 Show GP flow info. for ABR user
        If _IsABRUser Then
            Me.GridView1.Columns(1).Visible = False
            Me.GridView1.Columns(6).Visible = True
        End If

        If _IsAACUser Then
            Me.GridView1.Columns(1).Visible = True
            Me.GridView1.Columns(6).Visible = True
        End If
        If _IsAENCUser OrElse _IsCAPS OrElse _IsJPAonline Then
            Me.GridView1.Columns(3).Visible = False : Me.GridView1.Columns(5).Visible = False
            Me.GridView1.Columns(6).Visible = True
        End If
        If _IsAAUUser Then
            Me.GridView1.Columns(1).Visible = False : Me.GridView1.Columns(6).Visible = False
        End If

        'Ryan 20160804 Comment out for USAonline GP process cancelation
        'If _IsANAAOlineEP Then
        '    Me.GridView1.Columns(6).Visible = True
        'End If

        If _IsTWUser Then
            Me.GridView1.Columns(6).Visible = False

            'Ryan 20180208 Revise ATW column header text
            Me.GridView1.Columns(1).HeaderText = "Customer Quote No."
        End If
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtOPTYProjectName.Text.Trim.Replace("'", "''"))
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtOPTYProjectName.Text.Trim.Replace("'", "''"), True)
    End Sub


    'Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    Dim recordcount As Integer = Relics.dbUtil.dbExecuteScalar("EQ", "Select count(*) from QuotationMaster where (quoteId like 'AUSQ%' or quoteId like 'AMXQ%'  or quoteId like 'GQ%' ) and quoteNo is null")
    '    If recordcount > 0 Then
    '        Dim sql As String = "update QuotationMaster set quoteNo=quoteId,Revision_Number=1,Active=1 where (quoteId like 'AUSQ%' or quoteId like 'AMXQ%'  or quoteId like 'GQ%' ) and quoteNo is null"
    '        sql &= ";update QuotationMaster set DOCSTATUS=0 where (quoteId like 'AUSQ%' or quoteId like 'AMXQ%'  or quoteId like 'GQ%' ) and qstatus='DRAFT' and DOCSTATUS is null"
    '        sql &= ";update QuotationMaster set DOCSTATUS=1 where (quoteId like 'AUSQ%' or quoteId like 'AMXQ%'  or quoteId like 'GQ%' ) and qstatus='FINISH' and DOCSTATUS is null"
    '        sql &= ";update QuotationMaster set DOCSTATUS=2 where (quoteId like 'AUSQ%' or quoteId like 'AMXQ%'  or quoteId like 'GQ%' ) and qstatus='DELETED' and DOCSTATUS is null"
    '        sql &= ";update QuotationMaster set DOCSTATUS=3 where (quoteId like 'AUSQ%' or quoteId like 'AMXQ%'  or quoteId like 'GQ%' ) and qstatus='EXPIRED' and DOCSTATUS is null"
    '        sql &= ";UPDATE QuotationMaster set DocReg = 96 where (quoteId like 'AUSQ%') and DocReg is null"
    '        sql &= ";UPDATE QuotationMaster set DocReg = 32 where ( quoteId like 'AMXQ%') and DocReg is null"
    '        sql &= ";UPDATE QuotationMaster set DocReg = 31 where ( quoteId like 'GQ%') and DocReg is null"
    '        Relics.dbUtil.dbExecuteNoQuery("EQ", sql)
    '    End If
    'End Sub


    'public function GetTimeSpan() As TimeSpan
    '    If ViewState("TimeSpan") Is Nothing Then
    '        If Me._IsANAUser Then ViewState("TimeSpan") = Util.GetTimeSpan("AUS")
    '    End If
    '    Return ViewState("TimeSpan")
    'End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Pivot.CurrentProfile) Then
            Session.Abandon() : Util.ClearCookie_Login("ADEQCOOK")
            Response.Redirect(String.Format("~/login.aspx?RURL={0}", HttpContext.Current.Server.UrlEncode(Request.RawUrl)))
        End If

        Me._ServerPath = Util.GetRuntimeSiteUrl
        Me._IsEUUser = Role.IsEUSales()
        Me._IsFranchiser = Role.IsFranchiser()
        Me._IsANAUser = Role.IsUsaUser
        Me._IsJPAonline = Role.IsJPAonlineSales()
        Me._IsTWUser = Role.IsTWAonlineSales()
        Me._IsCNAonline = Role.IsCNAonlineSales()
        'Me._IsAACUser = Role.IsUSAACSales()
        If Role.IsUSAACSales() AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2100") Then
            _IsAACUser = True
        End If
        Me._IsKRAonline = Role.IsKRAonlineSales()
        Me._IsHQDCUser = Role.IsHQDCSales()
        Me._IsAENCUser = Role.IsUsaAenc()
        Me._IsCAPS = Role.IsCAPS
        Me._IsANAAOlineEP = Role.IsAonlineUsa()
        Me._IsABRUser = Role.IsABRSales
        Me._IsAAUUser = Role.IsAAUSales
        Me._IsAonlineIAG = Role.IsAonlineUsaIag
        Me._IsAonlineISG = Role.IsAonlineUsaISystem

        'This code must under Me._IsANAUser = GetLoginUserIsANAUser()
        'Me._TimeSpan = GetTimeSpan()
        Dim _Mode As String = Request("Mode")

        If Not String.IsNullOrEmpty(_Mode) AndAlso _Mode.Equals("MyTeam", StringComparison.InvariantCultureIgnoreCase) Then
            Me._IsMyTeamsMode = True
        Else
            Me._IsMyTeamsMode = False
            Me.LbCreatedBy.Visible = False
            Me.txtCreatedBy.Visible = False
        End If

        If Not IsPostBack Then
            If _IsTWUser Or _IsCNAonline Then
                Me.OptyProjectNameRow.Visible = True
            End If

            ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.txtOPTYProjectName.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue)
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

        'myQM.Delete(String.Format("quoteId='{0}'", id)) : myQD.Delete(String.Format("quoteId='{0}'", id))
        Dim _QM As IBUS.iDocHeader = Pivot.NewObjDocHeader
        'If Me._IsANAUser Then
        _QM.Update(id, String.Format("DOCSTATUS='{0}'", CInt(COMM.Fixer.eDocStatus.QDELETED)), COMM.Fixer.eDocType.EQ)
        tbOPBase.adddblog("UPDATE QuotationMaster SET DOCSTATUS ='" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "' WHERE (quoteId = '" & id & "')")
        'Else
        '    _QM.Update(id, String.Format("DOCSTATUS='{0}'", CInt(COMM.Fixer.eDocStatus.QEXPIRED)), COMM.Fixer.eDocType.EQ)
        '    tbOPBase.adddblog("UPDATE QuotationMaster SET DOCSTATUS ='" & CInt(COMM.Fixer.eDocStatus.QEXPIRED) & "' WHERE (quoteId = '" & id & "')")
        'End If

        'Ryan 20161017 If is ANAOnline quotes, also delete corresponding pipeline report.
        If _IsANAAOlineEP Then
            Try
                tbOPBase.dbExecuteNoQuery2("Pipeline", "delete from pipelineReport where quoteid = '" + id + "'")
            Catch ex As Exception
            End Try
        End If

        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtOPTYProjectName.Text.Trim.Replace("'", "''"), True)
    End Sub

    Protected Sub ibtnDelete1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)


        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim gv As GridView = CType(row.NamingContainer, GridView)
        Dim id As String = gv.DataKeys(row.RowIndex).Value

        'myQM.Delete(String.Format("quoteId='{0}'", id)) : myQD.Delete(String.Format("quoteId='{0}'", id))
        Dim _QM As IBUS.iDocHeader = Pivot.NewObjDocHeader
        ' If Me._IsANAUser Then
        _QM.Update(id, String.Format("DOCSTATUS='{0}'", CInt(COMM.Fixer.eDocStatus.QDELETED)), COMM.Fixer.eDocType.EQ)
        tbOPBase.adddblog("UPDATE QuotationMaster SET DOCSTATUS ='" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "' WHERE (quoteId = '" & id & "')")
        'Else
        '    _QM.Update(id, String.Format("DOCSTATUS='{0}'", CInt(COMM.Fixer.eDocStatus.QDELETED)), COMM.Fixer.eDocType.EQ)
        '    tbOPBase.adddblog("UPDATE QuotationMaster SET DOCSTATUS ='" & CInt(COMM.Fixer.eDocStatus.QEXPIRED) & "' WHERE (quoteId = '" & id & "')")
        'End If

        'Ryan 20161017 If is ANAOnline quotes, also delete corresponding pipeline report.
        If _IsANAAOlineEP Then
            Try
                tbOPBase.dbExecuteNoQuery2("Pipeline", "delete from pipelineReport where quoteid = '" + id + "'")
            Catch ex As Exception
            End Try
        End If

        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtOPTYProjectName.Text.Trim.Replace("'", "''"), True)
    End Sub



    Protected Sub ibtnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value
        Dim QM As Quote_Master = MyQuoteX.GetQuoteMaster(id)
        If QM IsNot Nothing AndAlso String.Equals(QM.org, "EU10", StringComparison.InvariantCultureIgnoreCase) AndAlso GPControl.IsWaitingForApproval(id) Then
            Util.showMessage("This quote cannot be modified due to it is in the manager approval stage")
            Exit Sub
        End If
        If _IsTWUser Or _IsCNAonline Then
            Response.Redirect(String.Format("~/Quote/QuotationDetail.aspx?UID={0}", id))
        Else
            Response.Redirect(String.Format("~/Quote/QuotationMaster.aspx?UID={0}", id))
        End If
    End Sub

    Protected Sub ibtnEdit1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim gv As GridView = CType(row.NamingContainer, GridView)
        Dim id As String = gv.DataKeys(row.RowIndex).Value
        Response.Redirect(String.Format("~/Quote/QuotationMaster.aspx?UID={0}", id))
    End Sub

    ' Ryan 20161228 EU user will not use ibtnCopy_Click anymore, will use btnCopyPurpose_Click instead
    Protected Sub ibtnCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim QuoteID As String = Me.GridView1.DataKeys(row.RowIndex).Value
        'BTCopyNext.Text = QuoteID
        'Ming add for copy order information 20140529
        Dim SoNo As String = String.Empty
        If _IsANAUser Then
            If MyQuoteX.GetSoNoByQuoteID(QuoteID, SoNo) Then
                If Not String.IsNullOrEmpty(SoNo.Trim) Then
                    Response.Redirect("~/quote/Order2Quote.aspx?SoNo=" & SoNo)
                    Response.End()
                End If
            End If
        End If
        Dim NewQuoteID As String = String.Empty, ErrorStr As String = String.Empty

        'Ryan 20161228 EU quotes copy will no longer use below function. Already move to new function 
        Dim retint As Integer = Business.CopyQuotation(QuoteID, NewQuoteID, ErrorStr, COMM.Fixer.eDocType.EQ)
        If retint = 0 Then
            LabCopymessage.Text = ErrorStr
            BTCopyNext.Visible = False
            MPCopy.Show()
            Exit Sub
        Else
            BTCopyNext.CommandArgument = NewQuoteID
            If String.IsNullOrEmpty(ErrorStr.Trim) Then
                BTCopyNext_Click(Me.BTCopyNext, Nothing)
            Else
                LabCopymessage.Text = ErrorStr
                BTCopyNext.Visible = True
                MPCopy.Show()
            End If
        End If
    End Sub

    Protected Sub ibtnCopy1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim gv As GridView = CType(row.NamingContainer, GridView)
        Dim QuoteID As String = gv.DataKeys(row.RowIndex).Value
        'BTCopyNext.Text = QuoteID
        Dim NewQuoteID As String = String.Empty, ErrorStr As String = String.Empty
        Dim retint As Integer = Business.CopyQuotation(QuoteID, NewQuoteID, ErrorStr, COMM.Fixer.eDocType.EQ)
        If retint = 0 Then
            LabCopymessage.Text = ErrorStr
            BTCopyNext.Visible = False
            MPCopy.Show()
            Exit Sub
        Else
            BTCopyNext.CommandArgument = NewQuoteID
            If String.IsNullOrEmpty(ErrorStr.Trim) Then
                BTCopyNext_Click(Me.BTCopyNext, Nothing)
            Else
                LabCopymessage.Text = ErrorStr
                BTCopyNext.Visible = True
                MPCopy.Show()
            End If
        End If
    End Sub

    Public Function isShowCreateCust(ByVal ERPID As String) As Boolean
        If ERPID <> "" Then
            Return False
        ElseIf COMM.Util.IsTesting() Then
            Return True
        End If
        Return False
    End Function


    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)


        If e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.DataRow Then

            If _IsTWUser Then
                e.Row.Cells(7).Visible = False
            Else
                e.Row.Cells(8).Visible = False
                e.Row.Cells(9).Visible = False
            End If

        End If

        'Ryan 20160720 If is not US users, hide pipeline status column
        If e.Row.RowType = DataControlRowType.Header Then
            If Not _IsANAAOlineEP Then
                e.Row.Cells(10).Visible = False
            End If

            'Ryan 20170731 Change AKR desc column text to "Quote Name"
            If _IsKRAonline Then
                e.Row.Cells(1).Text = "Quote Name"
            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim data As DataRowView = e.Row.DataItem

            Dim ID As String = Me.GridView1.DataKeys(e.Row.RowIndex).Value
            Dim _IsExpired As Boolean = False
            'Dim _IsOrderable As Boolean = Business.IsOrderable(data("siebelRBU"), data("quoteToErpId"), data("DOCSTATUS"), ID, data("DocReg"))
            Dim _IsOrderable As Boolean = Business.isOrderable(data("siebelRBU"), data("quoteToErpId"), data("DOCSTATUS"), ID)
            If _IsOrderable Then
                If _IsANAUser Then
                    '_IsExpired = DateDiff(DateInterval.Day, Now(), CDate(data("quoteDate"))) > 60
                Else
                    _IsExpired = Business.isExpired(ID, data("quoteDate"), data("expiredDate"), data("IsRepeatedOrder"))
                End If

                If _IsExpired Then CType(e.Row.FindControl("btnOrder"), Button).Visible = False

                'Ryan 20170515 AJP quotation is still able to be ordered even if is expired
                'Ming 20150716 與主管確認後同意，對ATW  AOnline的expired報價單，開放轉訂單功能。
                If _IsTWUser OrElse _IsCNAonline OrElse _IsJPAonline Then
                    CType(e.Row.FindControl("btnOrder"), Button).Visible = True
                End If
            Else
                CType(e.Row.FindControl("btnOrder"), Button).Visible = False
            End If

            If String.IsNullOrEmpty(data("quoteToErpId")) = False Then
                CType(e.Row.FindControl("btnGetErpId"), Button).Visible = False
            End If
            If IsDBNull(data("org")) Or data("org").ToString().ToUpper() = "US01" Then
                CType(e.Row.FindControl("lbGpFlow"), Label).Text = ""
            Else
                CType(e.Row.FindControl("lbGpFlow"), Label).Text = GPControl.getApproverStr(ID)
            End If
            'Ryan 20160804 Comment out for USAonline GP process cancelation
            'If _IsAACUser OrElse _IsKRAonline OrElse _IsHQDCUser OrElse _IsAENCUser
            'Frank 20180307 apply GP control function for ANA Aonline IIoT
            If _IsAACUser OrElse _IsKRAonline OrElse _IsHQDCUser OrElse _IsAENCUser OrElse _IsAonlineIAG OrElse _IsAonlineISG OrElse _IsABRUser Then
                Dim statustxt As String = String.Empty : Dim isGP As Boolean = False
                Dim IsWaitApprove As Boolean = Advantech.Myadvantech.Business.QuoteBusinessLogic.getApproverStatusforAAC(ID, statustxt, isGP)
                If IsWaitApprove Then
                    CType(e.Row.FindControl("btnOrder"), Button).Visible = False
                    CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = False
                    'Ryan 20160804 Comment out for USAonline GP process cancelation
                    'Jay 20160718
                    'If _IsANAAOlineEP Then
                    '    CType(e.Row.FindControl("ibtnPdf_ForANAUser"), ImageButton).Enabled = False
                    '    CType(e.Row.FindControl("ibtn_Pipeline"), HtmlInputImage).Disabled = True
                    '    CType(e.Row.FindControl("ibtnSendMail"), ImageButton).Enabled = False
                    '    CType(e.Row.FindControl("ibtnEdit"), ImageButton).Enabled = True
                    '    CType(e.Row.FindControl("ibtnDelete"), ImageButton).Enabled = False
                    'End If
                End If

                If isGP Then
                    CType(e.Row.FindControl("lbGpFlow"), Label).Text = statustxt
                    'CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = True
                    CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = False
                End If
            End If
            '20150106 暂时将Siebel Opportunity Id是Null不显示出来
            CType(e.Row.FindControl("lbSiebelQuoteId"), Label).Text = IIf((IsDBNull(data("org")) OrElse data("siebelQuoteId").ToString().ToUpper.Contains("NEW ID") OrElse data("siebelQuoteId").ToString().ToUpper.Contains("NULL")), String.Empty, data("siebelQuoteId").ToString())

            CType(e.Row.FindControl("lbStatusV"), Label).Text = COMM.EnumHelper.getDescription(CType(data("DOCSTATUS"), COMM.Fixer.eDocStatus))
            'Ming 20150717 Quote还没有转成finish状态就过期,ATW與ACN改顯示為draft的狀態
            Dim _IsCheckingExpiredQuote As Boolean = True
            If (_IsCNAonline OrElse _IsTWUser) AndAlso String.Equals(data("DOCSTATUS"), "0", StringComparison.CurrentCultureIgnoreCase) Then
                _IsCheckingExpiredQuote = False
            End If
            If _IsCheckingExpiredQuote AndAlso Business.isExpired(ID, data("quoteDate"), data("expiredDate"), data("IsRepeatedOrder")) Then
                CType(e.Row.FindControl("lbStatusV"), Label).Text = "Expired Quotation"
                'CType(e.Row.FindControl("btnOrder"), Button).Visible = False
                'Mike 2012/08/27:hidden edit button if expired for ANA
                CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = False

                If _IsAACUser AndAlso Not _IsAonlineIAG Then
                    CType(e.Row.FindControl("btnOrder"), Button).Visible = False
                End If
            End If

            'Frank 20150318
            'Display version information for AUS and ATW quotes
            If _IsANAUser OrElse _IsTWUser OrElse _IsCNAonline OrElse _IsKRAonline OrElse _IsHQDCUser OrElse _IsABRUser Then
                CType(e.Row.FindControl("lbRevisionNumberTitle"), Label).Visible = True
                CType(e.Row.FindControl("lbRevisionNumber"), Label).Visible = True
            Else
                CType(e.Row.FindControl("lbRevisionNumberTitle"), Label).Visible = False
                CType(e.Row.FindControl("lbRevisionNumber"), Label).Visible = False
            End If


            'Frank 2012/07/27:Transfer qoute time to local time
            If _IsANAUser Then
                e.Row.Cells(7).Text = Util.GetLocalTime("AUS", data("quoteDate")).ToString("MM/dd/yyyy")
            Else

                'Frank 20150313 ATW已改為自已決定報價單顯示格式機制(lineNo., PartNo., UnitPrice與Qty)
                '所以不用美國的PDF預覽方式，改直接下載。
                'If _IsTWUser Then
                '    CType(e.Row.FindControl("ibtnPdf_ForNotANAUser"), ImageButton).Visible = False
                '    CType(e.Row.FindControl("ibtnPdf_ForANAUser"), ImageButton).Visible = True
                'Else
                '    CType(e.Row.FindControl("ibtnPdf_ForNotANAUser"), ImageButton).Visible = True
                '    CType(e.Row.FindControl("ibtnPdf_ForANAUser"), ImageButton).Visible = False
                'End If

                If _IsTWUser Then
                    CType(e.Row.FindControl("ibtnPdf_ForNotANAUser"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ibtnPdf_ForANAUser"), ImageButton).Visible = True
                ElseIf _IsHQDCUser Then
                    CType(e.Row.FindControl("ibtnPdf_ForNotANAUser"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ibtnPdf_ForANAUser"), ImageButton).Visible = True
                ElseIf _IsJPAonline Then
                    CType(e.Row.FindControl("ibtnPdf_ForNotANAUser"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ibtnPdf_ForANAUser"), ImageButton).Visible = True
                Else
                    CType(e.Row.FindControl("ibtnPdf_ForNotANAUser"), ImageButton).Visible = True
                    CType(e.Row.FindControl("ibtnPdf_ForANAUser"), ImageButton).Visible = False
                End If

                If Not Business.isEidtableQuote(ID, data("org"), data("DOCSTATUS"), data("createdBy")) Then
                    CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = False
                End If
            End If


            'Frank 2012/06/23: Display the last order log
            Dim _latest_order_log As String = data("latest_order_log")
            If Not String.IsNullOrEmpty(_latest_order_log) Then
                Dim xDoc As New System.Xml.XmlDocument
                xDoc.LoadXml("<root>" & _latest_order_log & "</root>")
                CType(e.Row.FindControl("Label_SO_NO"), Label).Text = xDoc.GetElementsByTagName("SO_NO")(0).InnerText.Trim
                CType(e.Row.FindControl("Label_PO_NO"), Label).Text = xDoc.GetElementsByTagName("PO_NO")(0).InnerText.Trim
                CType(e.Row.FindControl("Label_ORDER_DATE"), Label).Text = xDoc.GetElementsByTagName("ORDER_DATE")(0).InnerText.Trim
                If Not IsDBNull(data("DOCSTATUS")) AndAlso data("DOCSTATUS") = CInt(COMM.Fixer.eDocStatus.QFINISH) Then
                    CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = False
                End If
            Else
                Dim _HyperLink_CheckAll As HyperLink = CType(e.Row.FindControl("HyperLink_CheckAll"), HyperLink)
                _HyperLink_CheckAll.Visible = False
                Dim _Last_Order_Log As HtmlTable = CType(e.Row.FindControl("Last_Order_Log"), HtmlTable)
                _Last_Order_Log.Visible = False
            End If

            'show mail name only Mike 08/01/2012
            Dim createdby As String = data("createdBy")
            If Not String.IsNullOrEmpty(createdby) AndAlso createdby.IndexOf("@") > 0 Then
                createdby = createdby.Substring(0, createdby.IndexOf("@"))
            End If
            Dim lbCreatedby As Label = e.Row.FindControl("lbCreatedby")
            lbCreatedby.Text = createdby
            If _IsFranchiser AndAlso data("DOCSTATUS") = CInt(COMM.Fixer.eDocStatus.QFINISH) Then
                CType(e.Row.FindControl("btnOrder"), Button).Visible = True
            End If
            Dim BTdelete As ImageButton = CType(e.Row.FindControl("ibtnDelete"), ImageButton)
            If BTdelete IsNot Nothing Then
                If _IsOrderable Then BTdelete.Visible = False
                If _IsExpired Then BTdelete.Visible = True
                '20150323 Ming 隱藏母節點delete功能
                If Role.IsTWAonlineSales Then
                    Dim GridView2 As GridView = CType(e.Row.FindControl("GridView2"), GridView)
                    If GridView2 IsNot Nothing AndAlso GridView2.Rows.Count > 0 Then
                        BTdelete.Visible = False
                    End If
                End If
            End If
            'ming hide fro Quote2.0   2013-08-22
            If data("quoteId") IsNot Nothing AndAlso Not IsDBNull(data("quoteNo")) AndAlso data("quoteNo") IsNot Nothing Then
                If String.Equals(data("quoteId"), data("quoteNo"), StringComparison.CurrentCultureIgnoreCase) Then
                    CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = False
                    ' CType(e.Row.FindControl("ibtnDelete"), ImageButton).Visible = True
                    CType(e.Row.FindControl("ibtnCopy"), ImageButton).Visible = True
                    CType(e.Row.FindControl("lbRevisionNumberTitle"), Label).Visible = False 'lbRevisionNumberTitle
                    CType(e.Row.FindControl("lbRevisionNumber"), Label).Visible = False 'lbRevisionNumber
                    ' If Not IsDBNull(data("qstatus")) AndAlso data("qstatus") IsNot Nothing AndAlso String.Equals(data("qstatus"), "FINISH", StringComparison.CurrentCultureIgnoreCase) Then
                    If (Not IsDBNull(data("qstatus")) AndAlso data("qstatus") IsNot Nothing AndAlso String.Equals(data("qstatus"), "FINISH", StringComparison.CurrentCultureIgnoreCase)) _
                             AndAlso (data("quoteToErpId") IsNot Nothing AndAlso Not String.IsNullOrEmpty(data("quoteToErpId"))) Then
                        CType(e.Row.FindControl("btnOrder"), Button).Visible = True
                        CType(e.Row.FindControl("ibtnDelete"), ImageButton).Visible = False
                    Else
                        CType(e.Row.FindControl("btnOrder"), Button).Visible = False
                    End If
                    'qstatus
                    Dim _qstatua As String = data("qstatus") & ""
                    Select Case _qstatua.ToUpper
                        Case "DRAFT"
                            CType(e.Row.FindControl("lbStatusV"), Label).Text = "Draft Quotation"
                        Case "DELETED"
                            CType(e.Row.FindControl("lbStatusV"), Label).Text = "Deleted Quotation"
                        Case "FINISH"
                            CType(e.Row.FindControl("lbStatusV"), Label).Text = "Finish Quotation"
                        Case "EXPIRED"
                            CType(e.Row.FindControl("lbStatusV"), Label).Text = "Expired Quotation"
                        Case ""
                            CType(e.Row.FindControl("lbStatusV"), Label).Text = ""
                    End Select
                End If
            End If
            'end

            'Frank 20150506 ATW, ACN Do not need to hide edit button
            If _IsTWUser Or _IsCNAonline Then
                CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = True
                '    CType(e.Row.FindControl("ibtnWord"), ImageButton).Visible = True
            End If

            If _IsAACUser Then
                CType(e.Row.FindControl("ibtnExcel"), ImageButton).Visible = True
            End If

            'Ryan 20160331 Control pipeline button visibility
            If _IsANAAOlineEP Then
                CType(e.Row.FindControl("ibtn_Pipeline"), HtmlInputImage).Visible = True
                If data("isPipelined").ToString = "1" Then
                    CType(e.Row.FindControl("ibtn_Pipeline"), HtmlInputImage).Src = "~/Images/pipelined_icon.png"
                End If

                'Ryan 20160720 Add field for Pipeline status     
                If String.IsNullOrEmpty(data("Probability").ToString) Then
                    CType(e.Row.FindControl("lbPipelineStatus"), Label).Text = " "
                Else
                    CType(e.Row.FindControl("lbPipelineStatus"), Label).Text = (Decimal.Round(Convert.ToDecimal(data("Probability").ToString) * 100)).ToString() + "%"
                End If
            Else
                e.Row.Cells(10).Visible = False
            End If

        End If
        If _IsFranchiser AndAlso (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow) Then
            Dim g As GridView = CType(sender, GridView)
            Dim nCompanyID As Integer = 0
            Dim nGetERPIDfromSiebel As Integer = 0
            Dim nGPFlow As Integer = 0
            Dim n As Integer = 0
            For Each c As DataControlField In g.Columns
                n += 1
                If c.HeaderText.Contains("ERP ID") Then
                    nCompanyID = n - 1
                End If
                If c.HeaderText.Contains("Get ERP ID from Siebel") Then
                    nGetERPIDfromSiebel = n - 1
                End If
                If c.HeaderText.Contains("GP Flow") Then
                    nGPFlow = n - 1
                End If
            Next
            e.Row.Cells(nCompanyID).Visible = False : e.Row.Cells(nGetERPIDfromSiebel).Visible = False : e.Row.Cells(nGPFlow).Visible = False
        End If


    End Sub

    Protected Sub GridView2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)


        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim data As DataRowView = e.Row.DataItem

            If _IsANAUser And _IsHQDCUser Then
                e.Row.Cells(1).Visible = False
            End If

            If _IsAACUser Then
                e.Row.Cells(1).Visible = True
            End If

            Dim gv As GridView = CType(e.Row.NamingContainer, GridView)

            Dim ID As String = gv.DataKeys(e.Row.RowIndex).Value

            'Dim _IsOrderable As Boolean = Business.IsOrderable(data("siebelRBU"), data("quoteToErpId"), data("DOCSTATUS"), ID, data("DocReg"))
            Dim _IsOrderable As Boolean = Business.isOrderable(data("siebelRBU"), data("quoteToErpId"), data("DOCSTATUS"), ID)
            If _IsOrderable Then
                Dim _IsExpired As Boolean = False
                If _IsANAUser Then
                    '_IsExpired = DateDiff(DateInterval.Day, Now(), CDate(data("quoteDate"))) > 60
                Else
                    _IsExpired = Business.isExpired(ID, data("quoteDate"), data("expiredDate"), data("IsRepeatedOrder"))
                End If
                If _IsExpired Then CType(e.Row.FindControl("btnOrder1"), Button).Visible = False

                'Ryan 20170515 AJP quotation is still able to be ordered even if is expired
                'Ming 20150716 與主管確認後同意，對ATW  AOnline的expired報價單，開放轉訂單功能。
                If _IsTWUser OrElse _IsCNAonline OrElse _IsJPAonline Then
                    CType(e.Row.FindControl("btnOrder1"), Button).Visible = True
                End If
            Else
                CType(e.Row.FindControl("btnOrder1"), Button).Visible = False
            End If

            'If String.IsNullOrEmpty(data("quoteToErpId")) = False Then
            '    CType(e.Row.FindControl("btnGetErpId"), Button).Visible = False
            'End If
            'If IsDBNull(data("org")) Or data("org").ToString().ToUpper() = "US01" Then
            '    CType(e.Row.FindControl("lbGpFlow"), Label).Text = ""
            'Else
            '    CType(e.Row.FindControl("lbGpFlow"), Label).Text = GPControl.getApproverStr(ID)
            'End If

            'CType(e.Row.FindControl("lbSiebelQuoteId"), Label).Text = IIf((IsDBNull(data("org")) OrElse data("siebelQuoteId").ToString().ToUpper.Contains("NEW ID")), String.Empty, data("siebelQuoteId").ToString())

            CType(e.Row.FindControl("lbStatusV_2"), Label).Text = COMM.EnumHelper.getDescription(CType(data("DOCSTATUS"), COMM.Fixer.eDocStatus))
            'Ming 20150717 Quote还没有转成finish状态就过期,ATW與ACN改顯示為draft的狀態
            Dim _IsCheckingExpiredQuote As Boolean = True
            If (_IsCNAonline OrElse _IsTWUser) AndAlso String.Equals(data("DOCSTATUS"), "0") Then
                _IsCheckingExpiredQuote = False
            End If
            If _IsCheckingExpiredQuote AndAlso Business.isExpired(ID, data("quoteDate"), data("expiredDate"), data("IsRepeatedOrder")) Then
                CType(e.Row.FindControl("lbStatusV_2"), Label).Text = "Expired"
                CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Visible = False
            End If

            'CType(e.Row.FindControl("ImageButton1"), ImageButton).OnClientClick = "openUpdateformatwin('" & data("quoteId") & "','" & data("quoteNo") & "');return false;"

            'Frank 2012/07/27:Transfer qoute time to local time
            If _IsANAUser Then

                CType(e.Row.FindControl("ImageButton1"), ImageButton).Visible = True
                CType(e.Row.FindControl("ImageButton1"), ImageButton).OnClientClick = "openUpdateformatwin('" & data("quoteId") & "','" & data("quoteNo") & "');return false;"

                'Ryan 20160804 Comment out for USAonline GP process cancelation
                'Dim statustxt As String = String.Empty : Dim isGP As Boolean = False
                'Dim IsWaitApprove As Boolean = Advantech.Myadvantech.Business.QuoteBusinessLogic.getApproverStatusforAAC(ID, statustxt, isGP)
                'If IsWaitApprove Then
                '    CType(e.Row.FindControl("btnOrder1"), Button).Visible = False
                '    CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Visible = False
                '    'Jay 20160718
                '    If _IsANAAOlineEP Then
                '        CType(e.Row.FindControl("ImageButton1"), ImageButton).Enabled = False
                '        CType(e.Row.FindControl("ibtn_Pipeline"), HtmlInputImage).Disabled = True
                '        CType(e.Row.FindControl("ImageButton2"), ImageButton).Enabled = False
                '        CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Enabled = True
                '        CType(e.Row.FindControl("ImageButton5"), ImageButton).Enabled = False
                '    End If
                'End If

                'Ryan 20160804 Comment out for USAonline GP process cancelation
                'Ryan 20160722 Hide edit & delete button in Gridview2 if quote is under GP approval process.
                'If isGP Then
                '    If _IsANAAOlineEP Then
                '        CType(e.Row.FindControl("lbGpFlow1"), Label).Text = statustxt
                '    End If
                '    CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Visible = True
                '    CType(e.Row.FindControl("ImageButton5"), ImageButton).Visible = False
                'End If

                'CType(e.Row.FindControl("btnOrder"), Button).Visible = True
                ''Frank 2012/09/11:Not orderable or quotedate is away from now more than 60 days, can not show up the order button
                'If (Not _IsOrderable) OrElse (DateDiff(DateInterval.Day, Now(), CDate(data("quoteDate"))) > 60) Then
                '    CType(e.Row.FindControl("btnOrder"), Button).Visible = False
                'End If

                'e.Row.Cells(7).Text = Util.TransferToLocalTime(Me._TimeSpan, data("quoteDate")).ToString("yyyy/MM/dd")
                e.Row.Cells(3).Text = Util.GetLocalTime("AUS", data("quoteDate")).ToString("MM/dd/yyyy")


            Else

                CType(e.Row.FindControl("ImageButton1"), ImageButton).Visible = False
                CType(e.Row.FindControl("ibtn_SubGrid_Pdf_ForNotANAUser"), ImageButton).Visible = True

                'If _IsTWUser Or _IsCNAonline Then
                '    CType(e.Row.FindControl("ibtn_SubGrid_Word"), ImageButton).Visible = True
                'End If

                'Frank 20151028
                If _IsHQDCUser Then
                    CType(e.Row.FindControl("ibtn_SubGrid_Pdf_ForNotANAUser"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ImageButton1"), ImageButton).Visible = True
                    CType(e.Row.FindControl("ImageButton1"), ImageButton).OnClientClick = "openUpdateformatwin('" & data("quoteId") & "','" & data("quoteNo") & "');return false;"


                    'Frank 
                    Dim statustxt As String = String.Empty : Dim isGP As Boolean = False
                    Dim IsWaitApprove As Boolean = Advantech.Myadvantech.Business.QuoteBusinessLogic.getApproverStatusforAAC(ID, statustxt, isGP)
                    If IsWaitApprove Then
                        CType(e.Row.FindControl("btnOrder1"), Button).Visible = False
                        CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Visible = False
                        'Jay 20160718
                        If _IsANAAOlineEP Then
                            CType(e.Row.FindControl("ImageButton1"), ImageButton).Enabled = False
                            CType(e.Row.FindControl("ibtn_Pipeline"), HtmlInputImage).Disabled = True
                            CType(e.Row.FindControl("ImageButton2"), ImageButton).Enabled = False
                            CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Enabled = True
                            CType(e.Row.FindControl("ImageButton5"), ImageButton).Enabled = False
                        End If
                    End If

                    If isGP Then
                        CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Visible = False
                        CType(e.Row.FindControl("ImageButton5"), ImageButton).Visible = False
                    End If





                End If

                'CType(e.Row.FindControl("ibtnPdf_ForNotANAUser"), ImageButton).Visible = True
                'CType(e.Row.FindControl("ibtnPdf_ForANAUser"), ImageButton).Visible = False

                'If Not Business.isEidtableQuote(ID, data("org"), data("DOCSTATUS"), data("createdBy")) Then
                'CType(e.Row.FindControl("ibtnEdit"), ImageButton).Visible = False
                'CType(e.Row.FindControl("ibtnDelete"), ImageButton).Visible = False
                'End If
            End If

            If _IsAACUser Then
                CType(e.Row.FindControl("ibtn_SubGrid_Excel"), ImageButton).Visible = True
            End If


            '    'Frank 2012/06/23: Display the last order log
            Dim _latest_order_log As String = data("latest_order_log")
            If Not String.IsNullOrEmpty(_latest_order_log) Then
                Dim xDoc As New System.Xml.XmlDocument
                xDoc.LoadXml("<root>" & _latest_order_log & "</root>")
                CType(e.Row.FindControl("Label_SO_NO1"), Label).Text = xDoc.GetElementsByTagName("SO_NO")(0).InnerText.Trim
                CType(e.Row.FindControl("Label_PO_NO1"), Label).Text = xDoc.GetElementsByTagName("PO_NO")(0).InnerText.Trim
                CType(e.Row.FindControl("Label_ORDER_DATE1"), Label).Text = xDoc.GetElementsByTagName("ORDER_DATE")(0).InnerText.Trim
                If Not IsDBNull(data("DOCSTATUS")) AndAlso data("DOCSTATUS") = CInt(COMM.Fixer.eDocStatus.QFINISH) Then
                    CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ImageButton5"), ImageButton).Visible = False
                End If
            Else
                Dim _HyperLink_CheckAll As HyperLink = CType(e.Row.FindControl("HyperLink_CheckAll1"), HyperLink)
                _HyperLink_CheckAll.Visible = False
                Dim _Last_Order_Log As HtmlTable = CType(e.Row.FindControl("Last_Order_Log1"), HtmlTable)
                _Last_Order_Log.Visible = False
            End If

            'show mail name only Mike 08/01/2012
            Dim createdby As String = data("createdBy")
            If Not String.IsNullOrEmpty(createdby) AndAlso createdby.IndexOf("@") > 0 Then
                createdby = createdby.Substring(0, createdby.IndexOf("@"))
            End If
            Dim lbCreatedby As Label = e.Row.FindControl("lbCreatedby1")
            lbCreatedby.Text = createdby
            If _IsFranchiser AndAlso data("DOCSTATUS") = CInt(COMM.Fixer.eDocStatus.QFINISH) Then
                CType(e.Row.FindControl("btnOrder1"), Button).Visible = True
            End If
            'End If
            'If _IsFranchiser AndAlso (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow) Then
            '    Dim g As GridView = CType(sender, GridView)
            '    Dim nCompanyID As Integer = 0
            '    Dim nGetERPIDfromSiebel As Integer = 0
            '    Dim nGPFlow As Integer = 0
            '    Dim n As Integer = 0
            '    For Each c As DataControlField In g.Columns
            '        n += 1
            '        If c.HeaderText.Contains("Company ID") Then
            '            nCompanyID = n - 1
            '        End If
            '        If c.HeaderText.Contains("Get ERP ID from Siebel") Then
            '            nGetERPIDfromSiebel = n - 1
            '        End If
            '        If c.HeaderText.Contains("GP Flow") Then
            '            nGPFlow = n - 1
            '        End If
            '    Next
            '    e.Row.Cells(nCompanyID).Visible = False : e.Row.Cells(nGetERPIDfromSiebel).Visible = False : e.Row.Cells(nGPFlow).Visible = False

            'ming hide fro Quote2.0   2013-08-22
            If data("quoteId") IsNot Nothing AndAlso data("quoteNo") IsNot Nothing Then
                If String.Equals(data("quoteId"), data("quoteNo"), StringComparison.CurrentCultureIgnoreCase) Then
                    CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Visible = False
                    CType(e.Row.FindControl("ImageButton5"), ImageButton).Visible = True
                    If data("qstatus") IsNot Nothing AndAlso String.Equals(data("qstatus"), "FINISH", StringComparison.CurrentCultureIgnoreCase) Then
                        CType(e.Row.FindControl("btnOrder1"), Button).Visible = True
                    Else
                        CType(e.Row.FindControl("btnOrder1"), Button).Visible = False
                    End If
                End If
            End If

            'Frank 20150506 ATW, ACN Do not need to hide edit button
            If _IsTWUser Or _IsCNAonline Then
                CType(e.Row.FindControl("ibtnEdit1"), ImageButton).Visible = True
            End If
            'end

            'Ryan 20160503 Control pipeline button visibility for "GridView2"
            If _IsANAAOlineEP Then
                CType(e.Row.FindControl("ibtn_Pipeline"), HtmlInputImage).Visible = True
                If data("isPipelined").ToString = "1" Then
                    CType(e.Row.FindControl("ibtn_Pipeline"), HtmlInputImage).Src = "~/Images/pipelined_icon.png"
                End If

                'Ryan 20160720 Add field for Pipeline status     
                If String.IsNullOrEmpty(data("Probability").ToString) Then
                    CType(e.Row.FindControl("lbPipelineStatus"), Label).Text = " "
                Else
                    CType(e.Row.FindControl("lbPipelineStatus"), Label).Text = "Pipeline Status: " + (Decimal.Round(Convert.ToDecimal(data("Probability").ToString) * 100)).ToString() + "%"
                End If

                'Ryan 20160722 Hide quoteid field for ANA AONLINE Users
                e.Row.Cells(1).Visible = False
            Else
                e.Row.Cells(5).Visible = False
            End If

        End If
    End Sub

    Protected Sub ibtnPdf_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim obj As ImageButton = CType(sender, ImageButton), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        Dim _QuoteNo As String = dt.quoteNo

        If Not IsNothing(dt) Then
            Dim myContentByte As Byte() = Nothing

            If _IsANAUser OrElse _IsJPAonline OrElse _IsTWUser OrElse _IsCNAonline _
                OrElse _IsKRAonline OrElse _IsHQDCUser OrElse _IsABRUser OrElse _IsAAUUser _
                OrElse _IsCAPS Then

                Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
                If _IsJPAonline Then
                    myContentByte = Util.DownloadQuotePDFByHtmlString(strUSAOnlineQuoteTemplateHtml, True)
                Else
                    'ElseIf _IsANAUser OrElse _IsTWUser OrElse _IsCNAonline OrElse _IsKRAonline OrElse _IsHQDCUser Then
                    myContentByte = Util.DownloadQuotePDFByHtmlString(strUSAOnlineQuoteTemplateHtml, False)
                End If
            ElseIf Role.IsEUSales Then
                myContentByte = Util.DownloadQuotePDFforAEU(id)
            Else
                'Dim URL As String = String.Format("http://{0}{1}{2}/quote/{3}?UID={4}&ROLE=1", _
                '   Request.ServerVariables("SERVER_NAME"), _
                '   IIf(Request.ServerVariables("SERVER_PORT") = "80", "", ":" + Request.ServerVariables("SERVER_PORT")), _
                '   HttpRuntime.AppDomainAppVirtualPath, _
                '   Business.getPiPage(id), id)
                'Frank 2012/08/13: I keep the server url in _ServerPath 
                Dim URL As String = String.Format("{0}/quote/{1}?UID={2}&ROLE=1", Me._ServerPath, Business.getPiPage(id, dt.DocReg), id)
                myContentByte = Util.DownloadQuotePDF(URL)
            End If
            'Dim fname As String = id.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            Dim fname As String = _QuoteNo.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            Response.Clear()
            Response.AddHeader("Content-Type", "binary/octet-stream")
            Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".pdf;size = " & myContentByte.Length.ToString())
            Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
        End If
    End Sub


    Protected Sub ibtn_SubGrid_Pdf_ForNotANAUser_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        'Dim obj As ImageButton = CType(sender, ImageButton), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        'Dim id As String = Me.GridView2.DataKeys(row.RowIndex).Value

        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim gv As GridView = CType(row.NamingContainer, GridView)
        Dim id As String = gv.DataKeys(row.RowIndex).Value

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        Dim _QuoteNo As String = dt.quoteNo

        If Not IsNothing(dt) Then
            Dim myContentByte As Byte() = Nothing
            'If Role.IsInMailGroup("AOnline.USA", Then
            If _IsANAUser OrElse _IsJPAonline OrElse _IsTWUser OrElse _IsCNAonline Then
                Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
                If _IsJPAonline Then
                    myContentByte = Util.DownloadQuotePDFByHtmlString(strUSAOnlineQuoteTemplateHtml, True)
                ElseIf _IsANAUser OrElse _IsTWUser OrElse _IsCNAonline Then
                    myContentByte = Util.DownloadQuotePDFByHtmlString(strUSAOnlineQuoteTemplateHtml, False)
                End If
            ElseIf Role.IsEUSales Then
                myContentByte = Util.DownloadQuotePDFforAEU(id)
            Else
                'Dim URL As String = String.Format("http://{0}{1}{2}/quote/{3}?UID={4}&ROLE=1", _
                '   Request.ServerVariables("SERVER_NAME"), _
                '   IIf(Request.ServerVariables("SERVER_PORT") = "80", "", ":" + Request.ServerVariables("SERVER_PORT")), _
                '   HttpRuntime.AppDomainAppVirtualPath, _
                '   Business.getPiPage(id), id)
                'Frank 2012/08/13: I keep the server url in _ServerPath 
                Dim URL As String = String.Format("{0}/quote/{1}?UID={2}&ROLE=1", Me._ServerPath, Business.getPiPage(id, dt.DocReg), id)
                myContentByte = Util.DownloadQuotePDF(URL)
            End If
            'Dim fname As String = id.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            Dim fname As String = _QuoteNo.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            Response.Clear()
            Response.AddHeader("Content-Type", "binary/octet-stream")
            Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".pdf;size = " & myContentByte.Length.ToString())
            Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
        End If
    End Sub


    Protected Sub ibtnWord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim obj As ImageButton = CType(sender, ImageButton), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        Dim _QuoteNo As String = dt.quoteNo

        If Not IsNothing(dt) Then
            Dim myContentByte As Byte() = Nothing
            Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
            myContentByte = Util.DownloadQuoteWordByHtmlString(strUSAOnlineQuoteTemplateHtml, False)

            Dim fname As String = _QuoteNo.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            Response.Clear()
            Response.AddHeader("Content-Type", "binary/octet-stream")
            Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".doc;size = " & myContentByte.Length.ToString())
            Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
        End If
    End Sub


    Protected Sub ibtn_SubGrid_Word_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim gv As GridView = CType(row.NamingContainer, GridView)
        Dim id As String = gv.DataKeys(row.RowIndex).Value

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        Dim _QuoteNo As String = dt.quoteNo

        If Not IsNothing(dt) Then
            Dim myContentByte As Byte() = Nothing
            Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
            myContentByte = Util.DownloadQuoteWordByHtmlString(strUSAOnlineQuoteTemplateHtml, False)

            Dim fname As String = _QuoteNo.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
            Response.Clear()
            Response.AddHeader("Content-Type", "binary/octet-stream")
            Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".doc;size = " & myContentByte.Length.ToString())
            Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
        End If

    End Sub


    Protected Sub ibtnExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim obj As ImageButton = CType(sender, ImageButton), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)

        If Not IsNothing(dt) Then
            MyQuoteX.ExportQuoteToExcel(id)
        End If
    End Sub

    Protected Sub ibtn_SubGrid_Excel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim gv As GridView = CType(row.NamingContainer, GridView)
        Dim id As String = gv.DataKeys(row.RowIndex).Value

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)

        If Not IsNothing(dt) Then
            MyQuoteX.ExportQuoteToExcel(id)
        End If

    End Sub


    'Protected Sub ibtnPdf_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    '    Dim obj As ImageButton = CType(sender, ImageButton), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
    '    Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value
    '    Dim O As New BigText("eq", "BigText")
    '    Dim dtC As New DataTable
    '    dtC = O.GetDT(String.Format("quoteId='{0}'", id), "")


    '    Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
    '    Dim _QuoteNo As String = dt.quoteNo

    '    'If dtC.Rows.Count > 0 AndAlso Not Role.IsInMailGroup("AOnline.USA",
    '    If dtC.Rows.Count > 0 AndAlso Not Role.IsUsaUser AndAlso Not Role.IsEUSales AndAlso Not Role.IsJPAonlineSales Then
    '        Dim co As String = dtC.Rows(0).Item("content")

    '        'Dim myContentByte As Byte() = Util.DownloadQuotePDFbyStr(co)
    '        Dim myContentByte As Byte() = Nothing, IsShowPageNum As Boolean = False
    '        If Me._IsJPAonline Then IsShowPageNum = True
    '        myContentByte = Util.DownloadQuotePDFByHtmlString(co, IsShowPageNum)

    '        If Not IsNothing(dt) Then
    '            Dim fname As String = dt.CustomId.ToString.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
    '            Response.Clear()
    '            Response.AddHeader("Content-Type", "binary/octet-stream")
    '            Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".pdf;size = " & myContentByte.Length.ToString())
    '            Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
    '        End If
    '    Else
    '        If Not IsNothing(dt) Then
    '            Dim myContentByte As Byte() = Nothing
    '            'If Role.IsInMailGroup("AOnline.USA", Then
    '            If Role.IsUsaUser OrElse Role.IsJPAonlineSales Then
    '                Dim strUSAOnlineQuoteTemplateHtml As String = Business.getPageStrInternal(id, dt.DocReg, False)
    '                If _IsJPAonline Then
    '                    myContentByte = Util.DownloadQuotePDFByHtmlString(strUSAOnlineQuoteTemplateHtml, True)
    '                Else
    '                    myContentByte = Util.DownloadQuotePDFByHtmlString(strUSAOnlineQuoteTemplateHtml, False)
    '                End If
    '            ElseIf Role.IsEUSales Then
    '                myContentByte = Util.DownloadQuotePDFforAEU(id)
    '            Else
    '                'Dim URL As String = String.Format("http://{0}{1}{2}/quote/{3}?UID={4}&ROLE=1", _
    '                '   Request.ServerVariables("SERVER_NAME"), _
    '                '   IIf(Request.ServerVariables("SERVER_PORT") = "80", "", ":" + Request.ServerVariables("SERVER_PORT")), _
    '                '   HttpRuntime.AppDomainAppVirtualPath, _
    '                '   Business.getPiPage(id), id)
    '                'Frank 2012/08/13: I keep the server url in _ServerPath 
    '                Dim URL As String = String.Format("{0}/quote/{1}?UID={2}&ROLE=1", Me._ServerPath, Business.getPiPage(id, dt.DocReg), id)
    '                myContentByte = Util.DownloadQuotePDF(URL)
    '            End If
    '            'Dim fname As String = id.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
    '            Dim fname As String = _QuoteNo.Replace("/", "_").ToString.Replace("\", "_").ToString.Replace(":", "_").ToString.Replace("?", "_").ToString.Replace("*", "_").ToString.Replace("""", "_").ToString.Replace("<", "_").ToString.Replace(">", "_").ToString.Replace("|", "_").ToString.Replace(" ", "_")
    '            Response.Clear()
    '            Response.AddHeader("Content-Type", "binary/octet-stream")
    '            Response.AddHeader("Content-Disposition", "attachment; filename=" & HttpContext.Current.Server.UrlEncode(fname) & ".pdf;size = " & myContentByte.Length.ToString())
    '            Response.Flush() : Response.BinaryWrite(myContentByte) : Response.Flush() : Response.End()
    '        End If
    '    End If
    'End Sub

    Protected Sub btnGetErpId_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value

        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(dt) Then
            Business.updateQuoteToErpId(dt.AccRowId, id, COMM.Fixer.eDocType.EQ)
            ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtOPTYProjectName.Text.Trim.Replace("'", "''"))

        End If
    End Sub

    Protected Sub btnOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button), row As GridViewRow = CType(obj.NamingContainer, GridViewRow), id As String = Me.GridView1.DataKeys(row.RowIndex).Value
        'Ming 20150505 检测ERPID 是否已变更
        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(dt) Then
            'Dim ERPID As String = SiebelTools.getErpIdFromSiebelByRowId(dt.AccRowId)
            Dim ERPID As String = String.Empty

            'Frank 20160608 This is because AENC team does not use Siebel to maintain account's profile.
            'So do not need to check if the ERPID is existed in Siebel
            'If _IsAENCUser Then
            If _IsAENCUser OrElse _IsCAPS OrElse _IsJPAonline Then
                ERPID = dt.AccErpId
            Else
                ERPID = SiebelTools.getErpIdFromSiebelByRowId(dt.AccRowId)
            End If

            If Not String.Equals(ERPID, dt.AccErpId, StringComparison.CurrentCultureIgnoreCase) Then
                Dim jscript As String = String.Format("ShowMasterErr('{0}','{1}',0);", "Warning", "This quotation cannot be flipped to order because quote-to-account’s ERPID has been changed in Siebel.")
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "JSAlert", jscript, True)
            Else
                If _IsANAUser And Business.isExpired(id, COMM.Fixer.eDocType.EQ) Then
                    Dim newDate As DateTime = Now
                    newDate = DateAdd(DateInterval.Day, EnumSetting.ExpiredDurationDay.USDuration, newDate)
                    Pivot.NewObjDocHeader.Update(id, String.Format("quoteDate = GETDATE(),expiredDate = '{0}'", newDate), COMM.Fixer.eDocType.EQ)
                    tbOPBase.adddblog(String.Format("UPDATE QuotationMaster SET quoteDate = GETDATE(), expiredDate ='{0}' WHERE (quoteId = {1})", newDate, id))
                End If
                Response.Redirect("~/quote/quoteModify.aspx?UID=" & id)
            End If
        End If
    End Sub

    Protected Sub btnOrder1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button), row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim gv As GridView = CType(row.NamingContainer, GridView), id As String = gv.DataKeys(row.RowIndex).Value
        'Ming 20150505 检测ERPID 是否已变更
        Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(id, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(dt) Then
            Dim ERPID As String = SiebelTools.getErpIdFromSiebelByRowId(dt.AccRowId)
            If Not String.Equals(ERPID, dt.AccErpId, StringComparison.CurrentCultureIgnoreCase) Then
                Dim jscript As String = String.Format("ShowMasterErr('{0}','{1}',0);", "Warning", "This quotation cannot be flipped to order because quote-to-account’s ERPID has been changed in Siebel.")
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "JSAlert", jscript, True)
            Else
                If _IsANAUser And Business.isExpired(id, COMM.Fixer.eDocType.EQ) Then
                    Dim newDate As DateTime = Now
                    newDate = DateAdd(DateInterval.Day, EnumSetting.ExpiredDurationDay.USDuration, newDate)
                    Pivot.NewObjDocHeader.Update(id, String.Format("quoteDate = GETDATE(),expiredDate = {0}", newDate), COMM.Fixer.eDocType.EQ)
                    tbOPBase.adddblog(String.Format("UPDATE QuotationMaster SET quoteDate = GETDATE(), expiredDate ='{0}' WHERE (quoteId = {1})", newDate, id))
                End If
                Response.Redirect("~/quote/quoteModify.aspx?UID=" & id)
            End If
        End If
    End Sub


    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtOPTYProjectName.Text.Trim.Replace("'", "''"))
    End Sub

    'Function getPageStr(ByVal UID As String) As String
    '    Return Business.getPageStr(UID)
    'End Function
    Protected Sub btnDeleteBatch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oDate As New Date
        If IsNumeric(Me.txtMonth.Text.Trim) Then
            Dim N As Integer = "-" & Me.txtMonth.Text.Trim
            oDate = DateAdd(DateInterval.Month, N, Now.Date)

            Dim DT As New DataTable

            DT = tbOPBase.dbGetDataTable("EQ", String.Format("SELECT * FROM QUOTATIONMASTER WHERE QuoteDate<'{0}' and createdBy='{1}' and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QDRAFT) & "'", oDate, Pivot.CurrentProfile.UserId))
            If DT.Rows.Count > 0 Then
                Dim mm As Integer = 0
                For Each R As DataRow In DT.Rows
                    If Business.isEidtableQuote(R.Item("quoteId")) Then
                        mm = mm + 1
                        'Pivot.NewObjDocHeader.Remove(R.Item("quoteId"), COMM.Fixer.eDocType.EQ)
                        'myQD.Delete(String.Format("quoteid='{0}'", R.Item("quoteId")))
                        'Ming 20141219  只改变状态，不删除数据
                        If Not GPControl.IsWaitingForApproval(ID) Then
                            Dim sql As String = "UPDATE QuotationMaster SET DOCSTATUS ='" & CInt(COMM.Fixer.eDocStatus.QDELETED) & "' WHERE (quoteId = '" & R.Item("quoteId") & "')"
                            tbOPBase.dbExecuteNoQuery("EQ", sql)
                            tbOPBase.adddblog(sql)
                        End If
                    End If
                Next
                Util.showMessage("Delete Success! total: " & mm & " pic.")
            End If
        End If
    End Sub

    Protected Sub SqlDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs)
        e.Command.CommandTimeout = 999
    End Sub

    Protected Sub BTCopyNext_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        If obj.CommandArgument IsNot Nothing AndAlso Not String.IsNullOrEmpty(obj.CommandArgument) Then
            Response.Redirect(String.Format("~/Quote/QuotationMaster.aspx?UID={0}", obj.CommandArgument))
        End If
    End Sub

    Protected Sub btnewquote_Click(sender As Object, e As EventArgs) Handles btnewquote.Click
        Response.Redirect("~/Quote/QuotationMaster.aspx")
    End Sub

    'Ryan 20161228 Add for EU user
    Protected Sub btnCopyPurpose_Click(sender As Object, e As EventArgs)
        Dim QuoteID As String = Me.hfCopyPurpose.Value
        Dim NewQuoteID As String = String.Empty, ErrorStr As String = String.Empty

        Dim retint As Integer = Business.CopyAEUQuotationV2(QuoteID, NewQuoteID, ErrorStr, COMM.Fixer.eDocType.EQ, Convert.ToInt32(CopyPurpose.SelectedValue))
        If retint = 0 Then
            LabCopymessage.Text = ErrorStr
            BTCopyNext.Visible = False
            MPCopy.Show()
            Exit Sub
        Else
            BTCopyNext.CommandArgument = NewQuoteID
            If String.IsNullOrEmpty(ErrorStr.Trim) Then
                BTCopyNext_Click(Me.BTCopyNext, Nothing)
            Else
                LabCopymessage.Text = ErrorStr
                BTCopyNext.Visible = True
                MPCopy.Show()
            End If
        End If

    End Sub
End Class
