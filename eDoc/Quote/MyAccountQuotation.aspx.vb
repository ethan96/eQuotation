Public Class MyAccountQuotation
    Inherits System.Web.UI.Page

    Function GetMyTeamQuoteSql() As String

        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" SELECT TOP 20 quoteId, quoteNo, Revision_Number, createdBy, customId, quoteToRowId, quoteToErpId, quoteToName, office, currency,  "))
            .AppendLine(String.Format(" salesEmail, quoteDate, deliveryDate, expiredDate  "))
            .AppendLine(String.Format(" FROM QuotationMaster AS a "))
            .AppendLine(String.Format(" WHERE Active=1 and createdBy='" & Pivot.CurrentProfile.UserId & "' or "))
            .AppendLine(String.Format(" ( "))
            .AppendLine(String.Format(" 	quoteToRowId IN "))
            .AppendLine(String.Format(" 	( "))
            .AppendLine(String.Format(" 		SELECT DISTINCT ACCOUNT_ROW_ID "))
            .AppendLine(String.Format(" 		FROM SIEBEL_ACCOUNT_OWNER_EMAIL AS z "))
            .AppendLine(String.Format(" 		WHERE (ACCOUNT_ROW_ID IS NOT NULL) AND (EMAIL_ADDRESS = '" & Pivot.CurrentProfile.UserId & "') "))
            .AppendLine(String.Format(" 	) "))
            .AppendLine(String.Format(" ) OR "))
            .AppendLine(String.Format(" ( "))
            .AppendLine(String.Format(" 	quoteToErpId IN "))
            .AppendLine(String.Format(" 	( "))
            .AppendLine(String.Format(" 		SELECT DISTINCT COMPANY_ID "))
            .AppendLine(String.Format(" 		FROM SAP_COMPANY_OWNER_EMAIL AS z "))
            .AppendLine(String.Format(" 		WHERE (COMPANY_ID IS NOT NULL) AND (email = '" & Pivot.CurrentProfile.UserId & "') "))
            .AppendLine(String.Format(" 	) "))
            .AppendLine(String.Format(" ) "))
            .AppendLine(String.Format(" ORDER BY quoteDate DESC, quoteToRowId "))
        End With
        Response.Write(sb) : Response.End()
        Return sb.ToString()
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            src1.SelectCommand = GetMyTeamQuoteSql()
        End If
    End Sub

    Protected Sub gv1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        src1.SelectCommand = GetMyTeamQuoteSql()
    End Sub

    Protected Sub gv1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        src1.SelectCommand = GetMyTeamQuoteSql()
    End Sub

End Class