Imports System.Reflection
Public Class pickBTO
    Inherits System.Web.UI.UserControl

    Public Sub getData(ByVal org As String, ByVal RBU As String, ByVal partNo As String)
        Dim dt As DataTable = Business.getBTOPrtItem(partNo, RBU, org)
        Me.GridView1.DataSource = dt
    End Sub

    Public Sub ShowData(ByVal org As String, ByVal RBU As String, ByVal partNo As String)
        getData(org, RBU, partNo)
        Me.GridView1.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(Me.HORG.Value, Me.HRBU.Value, Me.txtName.Text.Trim.Replace("'", "''"))
    End Sub


    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As Object = Me.GridView1.DataKeys(row.RowIndex).Values
        Dim P As Page = Me.Parent.Page
        Dim TP As Type = P.GetType()
        Dim MI As MethodInfo = TP.GetMethod("PickBTOPrtEnd")
        Dim para(0) As Object
        para(0) = key
        MI.Invoke(P, para)
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        ShowData(Me.HORG.Value, Me.HRBU.Value, Me.txtName.Text.Trim.Replace("'", "''"))

    End Sub

End Class