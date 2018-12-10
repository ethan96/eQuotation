Public Class CustomBtos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            buildTree(Me.treeBTOSList, "Root", getDTOnce("Root"))
        End If
    End Sub

    Function getDTOnce(ByVal PID As String) As DataTable
        Dim str As String = String.Format("WITH CTE AS (SELECT ID,PID,[TYPE],isnull([Description],'') as [Description] FROM CUSTOMBTO WHERE PID = '{0}' UNION ALL SELECT T.ID,T.PID,T.TYPE,isnull(T.[Description],'') as [Description] FROM CUSTOMBTO T INNER JOIN CTE A ON T.pid = A.id) SELECT * FROM CTE", PID)
        Dim DT As New DataTable
        DT = tbOPBase.dbGetDataTable("eq", str)
        Return DT

    End Function

    Sub buildTree(ByVal o As Object, ByVal PID As String, ByVal dt As DataTable)
        Dim oRows As DataRow() = dt.Select("PID='" & PID & "' and type=0", "id")
        If oRows.Length > 0 Then
            For Each r As DataRow In oRows
                Dim t As New TreeNode
                t.Text = r.Item("id")
                If Not String.IsNullOrEmpty(r.Item("Description").ToString) Then
                    t.Text &= " (" & r.Item("Description").ToString & ")"
                End If
                t.Value = r.Item("id")
                If o.GetType = GetType(TreeNode) Then
                    't.ImageUrl = "~/images/Config.gif"
                    CType(o, TreeNode).ChildNodes.Add(t)
                ElseIf o.GetType = GetType(TreeView) Then
                    t.ImageUrl = "~/images/Config.gif"
                    CType(o, TreeView).Nodes.Add(t)
                End If

                buildTree(t, r.Item("id"), dt)
            Next
        End If
    End Sub

    Protected Sub treeBTOSList_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If IsNothing(CType(sender, TreeView).SelectedNode.Parent) Then
            Exit Sub
        End If

        Me.lbItem.Text = Me.treeBTOSList.SelectedValue
        Dim STR As String = String.Format("select id from CUSTOMBTO WHERE TYPE='" & COMM.Fixer.eItemType.Parent & "' AND PID='{0}'", Me.treeBTOSList.SelectedValue)
        Dim DT As New DataTable
        DT = tbOPBase.dbGetDataTable("EQ", STR)

        If DT.Rows.Count > 0 Then
            Dim strB As String = "Consist of the following:<br /><br />"
            For Each R As DataRow In DT.Rows
                strB = strB & R.Item("id") & " , "
            Next
            Me.divC.InnerHtml = strB.Trim.Trim(",")
        Else
            Me.divC.InnerHtml = ""
        End If
        Me.MPA.Show()
    End Sub

    Protected Sub btnConfig_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        'Nada commented for comb order no need delete existing items.
        'myQD.Delete(String.Format("quoteId='{0}'", Request("UID")))
        'Business.ADD2QUOTE_V2(Request("UID"), Me.treeBTOSList.SelectedValue, 1, 0, COMM.Fixer.eItemType.Parent, "", 0, 0, Now, 0, 0, "")
        Business.ADD2QUOTE_V2_1(Request("UID"), Me.treeBTOSList.SelectedValue, 1, 0, COMM.Fixer.eItemType.Parent, "", 0, 0, Now, 0, "", 0, "", Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
        Response.Redirect("~/quote/QuotationDetail.aspx?VIEW=0&UID=" & Request("UID"))
    End Sub

End Class