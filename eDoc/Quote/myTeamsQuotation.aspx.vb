Public Class myTeamsQuotation
    Inherits System.Web.UI.Page


    Dim myQD As New QuotationDetail("EQ", "quotationDetail")
    Public Sub getData(ByVal CustomId As String, ByVal CompanyName As String, ByVal CompanyID As String, ByVal Status As String, ByVal state As String, ByVal province As String)

        Me.SqlDataSource1.SelectCommand = Business.getMyTeamsQuoteRecord(txtQuoteId.Text.Trim.Replace("'", "''"), Pivot.CurrentProfile.UserId, CustomId, CompanyName, CompanyID, Status, state, province)
    End Sub

    Public Sub ShowData(ByVal CustomId As String, ByVal CompanyName As String, ByVal CompanyID As String, ByVal Status As String, ByVal state As String, ByVal province As String)
        getData(CustomId, CompanyName, CompanyID, Status, state, province)
        Me.GridView1.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtState.Text.Trim.Replace("'", "''"), Me.txtProvince.Text.Trim.Replace("'", "''"))
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtState.Text.Trim.Replace("'", "''"), Me.txtProvince.Text.Trim.Replace("'", "''"))
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Pivot.CurrentProfile) Then
            Session.Abandon() : Util.ClearCookie_Login("ADEQCOOK")
            Response.Redirect(String.Format("~/login.aspx?RURL={0}", HttpContext.Current.Server.UrlEncode(Request.RawUrl)))
        End If

        If Not IsPostBack Then
            ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtState.Text.Trim.Replace("'", "''"), Me.txtProvince.Text.Trim.Replace("'", "''"))
        End If
    End Sub

    Protected Sub ibtnDetail_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim obj As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = Me.GridView1.DataKeys(row.RowIndex).Value
        Response.Redirect(String.Format("~/Quote/QuotationPreview.aspx?UID={0}", id))
    End Sub



    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ID As String = Me.GridView1.DataKeys(e.Row.RowIndex).Value
            CType(e.Row.FindControl("lbGpFlow"), Label).Text = GPControl.getApproverStr(ID)
            'CType(e.Row.FindControl("lbProcess"), Label).Text = "<a href=""http://" & HttpContext.Current.Request.ServerVariables("SERVER_NAME") & ":" & HttpContext.Current.Request.ServerVariables("SERVER_PORT") & "/quote/QuoteApproval.aspx?UID=" & ID & """>Process</a>"
            Dim _RunTimeSiteUrl As String = Business.GetRuntimeSiteUrl()
            CType(e.Row.FindControl("lbProcess"), Label).Text = "<a href=""" & _RunTimeSiteUrl & "/quote/QuoteApproval.aspx?UID=" & ID & """>Process</a>"
        End If
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        'Me.GridView1.S
        ShowData(Me.txtCustomId.Text.Trim.Replace("'", "''"), Me.txtAccountName.Text.Trim.Replace("'", "''"), Me.txtCompanyID.Text.Trim.Replace("'", "''"), Me.drpStatus.SelectedValue, Me.txtState.Text.Trim.Replace("'", "''"), Me.txtProvince.Text.Trim.Replace("'", "''"))
    End Sub

End Class