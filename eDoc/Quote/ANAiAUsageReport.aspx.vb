Public Class ANAiAUsageReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim _dateFrom As String = DateAdd(DateInterval.Month, -1, Date.Now).ToString("yyyy-MM-dd")
        Dim _dateTo As String = Date.Now.ToString("yyyy-MM-dd")

        If Not Page.IsPostBack Then
            Me.txtFromDate.Text = _dateFrom
            Me.txtToDate.Text = _dateTo
        End If


    End Sub

    Function GetSQL() As String
        Dim _fromdate As String = Me.txtFromDate.Text
        Dim _todate As String = Me.txtToDate.Text

        Dim sb As New StringBuilder
        sb.AppendLine(" select distinct a.createdBy as [Quote Created By],  ")
        sb.AppendLine(" case ")
        sb.AppendLine("    when a.createdBy='nathans@advantech.com' then 'nathans@advantech.com' ")
        sb.AppendLine("    else ")
        sb.AppendLine("    ( ")
        sb.AppendLine("       select top 1 z2.EMAIL_ADDR   ")
        sb.AppendLine("       from MyAdvantechGlobal.dbo.EZ_EMPLOYEE z1 (nolock) inner join MyAdvantechGlobal.dbo.EZ_EMPLOYEE z2 (nolock) on z1.MANAGER=z2.EZROWID   ")
        sb.AppendLine("       where z1.EMAIL_ADDR=a.createdBy  ")
        sb.AppendLine("     )    ")
        sb.AppendLine("     end as SalesManager, a.quoteNo,Convert(varchar(10),getdate(),120) as createdDate, a.quoteToErpId,  ")
        sb.AppendLine("     (select top 1 z.company_name from MyAdvantechGlobal.dbo.sap_dimcompany z (nolock) where z.company_id=a.quoteToErpId) as AccountName, ")
        sb.AppendLine("     (select sum(z.newUnitPrice*z.qty) from QuotationDetail z (nolock) where z.quoteId=a.quoteId) as QuoteAmount,  ")
        sb.AppendLine("     case     ")
        sb.AppendLine("        when c.SO_NO is null then null ")
        sb.AppendLine("        else ")
        sb.AppendLine("        (select sum(z.UNIT_PRICE*z.QTY) from MyAdvantechGlobal.dbo.ORDER_DETAIL z (nolock) where z.ORDER_ID=c.SO_NO)  ")
        sb.AppendLine("        end as OrderAmount, ")
        sb.AppendLine("     case     ")
        sb.AppendLine("        when c.SO_NO is null then 'N' ")
        sb.AppendLine("        else 'Y' ")
        sb.AppendLine("     end as [Is To Order?],    ")
        '20151214
        'Hi Frank-
        'The revised report is okay with me. It’s more important to get Carley and Amy’s records to show up in the report than it is to have “sales code” and “sales sector” included. Thanks for your help!
        '-Lynette
        'sb.AppendLine("     c.SO_NO, c.ORDER_DATE, d.SALES_CODE, e.id_rbu as SalesSector    ")
        sb.AppendLine("     c.SO_NO, c.ORDER_DATE   ")
        sb.AppendLine("     From QuotationMaster a (nolock)    ")
        sb.AppendLine("     left join QUOTE_TO_ORDER_LOG c (nolock) on a.quoteId=c.QUOTEID    ")
        'sb.AppendLine("     left join MyAdvantechGlobal.dbo.SAP_EMPLOYEE d (nolock) on a.createdBy=d.EMAIL     ")
        'sb.AppendLine("     left join MyAdvantechGlobal.dbo.EAI_IDMAP e (nolock) on d.SALES_CODE=e.id_sap     ")
        sb.AppendLine("     where a.quoteNo like 'AACQ%' and (a.createdDate >= '" & _fromdate & "' and a.createdDate <='" & _todate & "')    ")
        'sb.AppendLine("     and e.id_rbu like 'AAC%' and a.quoteToErpId not in ('UEPP5001')    ")
        sb.AppendLine("     and a.createdBy in  (  ")

        Dim SB_sub As New StringBuilder
        'SB_sub.AppendLine(" Select DISTINCT b.Email From [ACLSQL6\SQL2008R2].MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK a  ")
        'SB_sub.AppendLine(" Left join [ACLSQL6\SQL2008R2].MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID  ")
        'SB_sub.AppendLine(" Inner join [ACLSQL6\SQL2008R2].MyAdvantechGlobal.dbo.ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID  ")
        SB_sub.AppendLine(" Select DISTINCT b.ALIAS_EMAIL From MyAdvantechGlobal.dbo.AD_MEMBER a  ")
        SB_sub.AppendLine(" Left join MyAdvantechGlobal.dbo.AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL  ")
        SB_sub.AppendLine(" Inner join MyAdvantechGlobal.dbo.AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL  ")

        SB_sub.AppendLine(" Where 1=1 ")
        'SB_sub.AppendLine(" And b.Email not like '%O365.mail.onmicrosoft.%' ")
        SB_sub.AppendLine(" And b.ALIAS_EMAIL not like '%O365.mail.onmicrosoft.%' ")
        Dim GroupA As New ArrayList
        GroupA.Add("N'SALES.IAG.USA'")
        'SB_sub.AppendFormat(" and c.Name IN ({0}) ", String.Join(",", GroupA.ToArray()))
        SB_sub.AppendFormat(" and c.Group_Name IN ({0}) ", String.Join(",", GroupA.ToArray()))
        sb.AppendLine(SB_sub.ToString + ")")

        sb.AppendLine("     and a.quoteToErpId not in ('UEPP5001')    ")
        sb.AppendLine("     order by a.createdBy, a.quoteNo    ")

        Return sb.ToString

    End Function




    Protected Sub btnQuery_Click(sender As Object, e As EventArgs) Handles btnQuery.Click

        Dim dt As DataTable = tbOPBase.dbGetDataTable("eQ", GetSQL())
        Me.GV1.DataSource = dt
        Me.GV1.DataBind()

    End Sub

    Protected Sub imgXls_Click(sender As Object, e As ImageClickEventArgs) Handles imgXls.Click
        Dim dt As DataTable = tbOPBase.dbGetDataTable("eQ", GetSQL())
        Util.DataTable2ExcelDownload(dt, "ANAiAUsageReport")

    End Sub
End Class