Public Class CustomBTOS_ACN
    Inherits System.Web.UI.Page
    Private UID As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        UID = Request("UID")

        If Not Page.IsPostBack Then
            'Dim plines As New ArrayList
            Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", _
                                    " SELECT distinct CATALOG_TYPE  from   dbo.CBOM_CATALOG   where CATALOG_ORG='CN' and CATALOG_TYPE is not null and CATALOG_TYPE <> '' order by CATALOG_TYPE   ")
            For Each r As DataRow In dt.Rows
                'If Not plines.Contains(r.Item("CATALOG_TYPE").ToString()) Then
                ' Tree1.Nodes.Add(New TreeNode(r.Item("CATALOG_TYPE").ToString(), r.Item("CATALOG_TYPE").ToString()))
                Dim btoNode As New TreeNode(r.Item("CATALOG_TYPE").ToString(), r.Item("CATALOG_TYPE").ToString())
                btoNode.NavigateUrl = "javascript:void(0)"
                Tree1.Nodes.Add(btoNode)
                ' plines.Add(r.Item("CATALOG_TYPE").ToString())
                '  Else
                ' plines.Add(r.Item("CATALOG_TYPE").ToString())
                ' End If
            Next
            Dim SB As New StringBuilder()
            SB.Append(" select  distinct CATALOG_ID,CATALOG_TYPE   from  CBOM_CATALOG  where  CATALOG_TYPE in ( ")
            SB.Append(" SELECT distinct CATALOG_TYPE  from   dbo.CBOM_CATALOG   where CATALOG_ORG='CN' and CATALOG_TYPE is not null and CATALOG_TYPE <> '' ")
            SB.Append(" ) and CATALOG_ORG='CN' ORDER BY CATALOG_ID ")
            Dim BtosDt As DataTable = tbOPBase.dbGetDataTable("MY", SB.ToString())
            For Each n As TreeNode In Tree1.Nodes
                Dim rs() As DataRow = BtosDt.Select("CATALOG_TYPE='" + Replace(n.Value, "'", "''") + "'")
                For Each r As DataRow In rs
                    Dim btoNode As New TreeNode(r.Item("CATALOG_ID"), r.Item("CATALOG_ID"))
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