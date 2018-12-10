Imports System.Reflection
Public Class PickAttention
    Inherits System.Web.UI.UserControl

    Public Sub getData(ByVal sid As String, ByVal Name As String)
        Dim SQLSTR As String = SiebelTools.GET_Attention_By_AccountRowID(sid, Name)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("CRM", SQLSTR)
        Me.GridView1.DataSource = dt
    End Sub

    Public Sub ShowData(ByVal sid As String, ByVal name As String)
        getData(sid, name)
        Me.GridView1.DataBind()
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridView1.PageIndex = e.NewPageIndex
        ShowData(Me.txtID.Text.Trim.Replace("'", "''"), Me.txtName.Text.Trim.Replace("'", "''"))
    End Sub


    Protected Sub lbtnPick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim o As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType(o.NamingContainer, GridViewRow)
        Dim key As Object = Me.GridView1.DataKeys(row.RowIndex).Values
        Dim P As Page = Me.Parent.Page
        Dim TP As Type = P.GetType()
        Dim MI As MethodInfo = TP.GetMethod("PickAttentionEnd")
        Dim para(0) As Object
        para(0) = key
        MI.Invoke(P, para)
    End Sub

    Protected Sub btnSH_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowData(Me.txtID.Text.Trim.Replace("'", "''"), Me.txtName.Text.Trim.Replace("'", "''"))
    End Sub

End Class