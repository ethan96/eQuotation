Public Class CustomBTOS_ATW
    Inherits System.Web.UI.Page

    Private UID As String = ""
    Private ORGID As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        UID = Request("UID")

        ORGID = Request("ORGID")
        Dim _SubORG As String = ORGID.Substring(0, 2)
        Dim _eStoreName As String = "A" + _SubORG + "eStore"

        If Not Page.IsPostBack Then
            Dim plines As New ArrayList
            'Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", _
            '                        " select a.PART_NO, SUBSTRING( a.PART_NO,0,CHARINDEX('-',a.PART_NO)) as ProductLine, sum(a.ORDER_QTY) as Qty, count(distinct a.COMPANY_ID) as Customers " + _
            '                        " from SAP_ORDER_HISTORY a with (nolock) inner join SAP_PRODUCT b with (nolock) on a.PART_NO=b.PART_NO  " + _
            '                        " where a.PART_NO like '%-BTO' and a.PART_NO<>'PTRADE-BTO' and b.MATERIAL_GROUP in ('BTOS')  " + _
            '                        " and a.SALES_ORG='TW01' and a.ORDER_DATE>=getdate()-1000 and b.STATUS='A' " + _
            '                        " and SUBSTRING( a.PART_NO,0,CHARINDEX('-',a.PART_NO)) not in ('LCDP','LEDP','CAMPAIGN07') " + _
            '                        " and a.PART_NO in (select distinct z.CATEGORY_ID from CBOM_CATALOG_CATEGORY z with (nolock) where z.ORG='TW' and z.PARENT_CATEGORY_ID='Root') " + _
            '                        " group by a.PART_NO having sum(a.ORDER_QTY)>10 and count(distinct a.COMPANY_ID)>3 " + _
            '                        " order by SUBSTRING( a.PART_NO,0,CHARINDEX('-',a.PART_NO)), sum(a.ORDER_QTY) desc, count(distinct a.COMPANY_ID) desc, a.PART_NO ")
            'Ming 20150447 合并最新maintain的btos
            Dim sb As New StringBuilder
            With sb
                .Append(" select distinct partno,ProductLine,Qty, Customers ")
                .Append(" from (  ")
                .Append(" select a.CATALOG_NAME  as partno, SUBSTRING(a.CATALOG_NAME,0,CHARINDEX('-',a.CATALOG_NAME)) as ProductLine, 0 as Qty,0 as Customers  ")
                .Append(" from  CBOM_CATALOG  a  ")
                .Append(" where a.CATALOG_ORG='" & _SubORG & "' ")
                .Append(" and a.CATALOG_NAME in (select PARENT_CATEGORY_ID from CBOM_CATALOG_CATEGORY where ORG='" & _SubORG & "')")
                .Append(" and a.CATALOG_NAME not like 'C-CTOS-%' ")
                .Append(" and ( a.CREATED > dateadd(dd,-day(dateadd(month,-2,getdate()))+1,dateadd(month,-2,getdate())) ")
                .Append("       Or a.CATALOG_NAME Like 'IPC-%' ")
                .Append("       Or a.CATALOG_NAME Like 'EIS%' ) ")

                .Append(" union  ")
                .Append(" select a.PART_NO, SUBSTRING( a.PART_NO,0,CHARINDEX('-',a.PART_NO)) as ProductLine, sum(a.ORDER_QTY) as Qty, count(distinct a.COMPANY_ID) as Customers  ")
                .Append(" from SAP_ORDER_HISTORY a with (nolock) inner join SAP_PRODUCT b with (nolock) on a.PART_NO=b.PART_NO   ")
                .Append(" where a.PART_NO like '%-BTO' and a.PART_NO<>'PTRADE-BTO' and b.MATERIAL_GROUP in ('BTOS')  ")
                .Append(" and a.SALES_ORG='" & ORGID & "' and a.ORDER_DATE>=getdate()-1000 and b.STATUS='A'  ")
                .Append(" and SUBSTRING( a.PART_NO,0,CHARINDEX('-',a.PART_NO)) not in ('LCDP','LEDP','CAMPAIGN07')  ")
                .Append(" and a.PART_NO in (select distinct z.CATEGORY_ID from CBOM_CATALOG_CATEGORY z with (nolock) where z.ORG='" & _SubORG & "' and z.PARENT_CATEGORY_ID='Root')  ")
                '.Append(" group by a.PART_NO having sum(a.ORDER_QTY)>10 and count(distinct a.COMPANY_ID)>3  ")
                'Frank 2016/3/2 release all the IPC- system
                .Append(" group by a.PART_NO")

                If Not Role.IsHQDCSales Then
                    '.Append(" having(sum(a.ORDER_QTY) > 10 And count(distinct a.COMPANY_ID) > 3) Or a.PART_NO Like 'IPC-%'  ")
                    .Append(" having(sum(a.ORDER_QTY) > 10 And count(distinct a.COMPANY_ID) > 3) ")
                    '.Append(" Or a.PART_NO Like 'IPC-%' ")
                    '.Append(" Or a.PART_NO Like 'EIS%' ")
                End If

                .Append(" )  as T ")
                .Append(" order by ProductLine, Qty desc, Customers desc, partno ")
            End With
            Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", sb.ToString())
            Tree1.Nodes.Add(New TreeNode("eStore Pre-Config System", _eStoreName, "", "~/Quote/EstoreCBOMCategory.aspx?UID=" & UID, ""))

            For Each r As DataRow In dt.Rows
                If Not plines.Contains(r.Item("ProductLine").ToString()) Then
                    Tree1.Nodes.Add(New TreeNode(r.Item("ProductLine").ToString(), r.Item("ProductLine").ToString()))
                    plines.Add(r.Item("ProductLine").ToString())
                Else
                    plines.Add(r.Item("ProductLine").ToString())
                End If
            Next

            For Each n As TreeNode In Tree1.Nodes
                Dim rs() As DataRow = dt.Select("ProductLine='" + Replace(n.Value, "'", "''") + "'")
                For Each r As DataRow In rs
                    Dim btoNode As New TreeNode(r.Item("partno"), r.Item("partno"))

                    'btoNode.Target = "_blank" : btoNode.NavigateUrl = "../Order/Configurator.aspx?BTOITEM=" + btoNode.Value

                    'Dim str As String = "~/quote/Configurator.aspx?BTOITEM=" & id & "&QTY=" & intQty & "&UID=" & UID
                    btoNode.NavigateUrl = "~/quote/Configurator.aspx?BTOITEM=" + btoNode.Value & "&QTY=1&UID=" & UID

                    n.ChildNodes.Add(btoNode)
                Next
            Next
            Me.Tree1.CollapseAll()
        End If
    End Sub

    Protected Sub Tree1_SelectedNodeChanged(sender As Object, e As EventArgs) Handles Tree1.SelectedNodeChanged
        If Tree1.SelectedNode.Depth = 1 Then
            Response.Redirect("../Order/Configurator.aspx?BTOITEM=" + Tree1.SelectedNode.Value)
        End If
    End Sub
End Class